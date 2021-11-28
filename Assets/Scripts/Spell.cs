using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

/// <summary>
/// This abstract class defines generic spell behaviour.
/// </summary>
abstract public class Spell : MonoBehaviour {

    [SerializeField] protected bool USING_DEBUG = true;

    [SerializeField] protected Camera FPCamera;
    [SerializeField] protected ParticleSystem chargingParticles;
    [SerializeField] private SpellDataModule spellData;

    /// <summary>
    /// Retrieve a reference to the Spell's Data Module
    /// </summary>
    public SpellDataModule Data { get => spellData; set => spellData = value; }

    /// <summary>
    /// Can this spell be cast on this frame?
    /// </summary>
    public bool CanCast {
        get => Data.CoolDownRemaining == 0 && Mathf.Abs(Data.ChargeTimeRemaining - Data.ChargeTime) <= float.Epsilon;
    }

    [System.Serializable]
    public class SpellDataModule { 
        // spell constants
        [SerializeField] private int damage = 5;
        [SerializeField] [Range(1.0f, 10.0f)] private float coolDownTime = 5.0f; // in seconds
        [SerializeField] [Range(0.1f, 6.0f)] private float chargeTime = 2.0f; // in seconds

        // these variables are used to implement timers
        [SerializeField][Tooltip("Starts at 0!")] private float coolDownRemaining = 0.0f;
        [SerializeField][Tooltip("Starts equal to chargeTime!")] private float chargeTimeRemaining = 2.0f;

        public SpellDataModule() { }

        public SpellDataModule(int damage, int coolDown, int charge) {
            this.damage = damage;
            this.coolDownTime = coolDown;
            this.chargeTime = charge;

            this.coolDownRemaining = 0.0f;
            this.chargeTimeRemaining = chargeTime;
        }
        
        public int Damage { get => damage; private set => damage = value; }
        public float CoolDownTime { get => coolDownTime; private set => coolDownTime = value; }
        public float ChargeTime { get => chargeTime; private set => chargeTime = value; }
        public float CoolDownRemaining { get => coolDownRemaining; set => coolDownRemaining = value; }
        public float ChargeTimeRemaining { get => chargeTimeRemaining; set => chargeTimeRemaining = value; }
    }

    // Input Consts:
    public const string fireButton = "Fire";

    /// <summary>
    /// Starts the full spell routine.
    /// </summary>
    virtual public void InvokeSpell() {
        if (USING_DEBUG) {
            Debug.Log($"Spell with name {this.name} has been invoked!");
        }
        // TODO: it remains to be seen how anims will be handled here.
        ThrowSpell();
        ApplyEffectToPlayer();
    }

    /// <summary>
    /// If this spell has effects on the Player, start them here.
    /// </summary>
    /// <returns>True if there are effects; false if not.</returns>
    abstract protected bool ApplyEffectToPlayer();

    /// <summary>
    /// If the spell is a hitscan or a projectile, fire it here.
    /// </summary>
    /// <returns>True if the spell is thrown; false if not.</returns>
    abstract protected bool ThrowSpell();

    private void Awake() {
        Data.ChargeTimeRemaining = Data.ChargeTime;
        Data.CoolDownRemaining = 0;
    }

    virtual protected void Start() { }

    virtual protected void Update() {
        if (CanCast && CrossPlatformInputManager.GetButton(fireButton)) {
            // TODO: Implement audio events
            StartCoroutine(nameof(SpellChargeClock));
        }

        else if (!CanCast && CrossPlatformInputManager.GetButtonDown(fireButton)) {
            Debug.LogWarning("This spell cannot be cast right now!");
        }
    }

    /// <summary>
    /// This is a Coroutine that is to be started after the player has finished casting a spell.
    /// </summary>
    virtual protected IEnumerator CoolDownTimer() {

        // start the cooldown timer and let other procedures check if the spell can be cast
        Data.CoolDownRemaining = Data.CoolDownTime;
        while (Data.CoolDownRemaining >= 0) {
            Data.CoolDownRemaining -= Time.deltaTime;
            yield return null;
        }

        if(USING_DEBUG) {
            Debug.Log($"Spell with name {this.name} can be cast.");
        }

        // floating point funny
        Data.CoolDownRemaining = 0.0f;
    }

    /// <summary>
    /// This is a Coroutine that is to be started after the player has started to cast a spell.
    /// </summary>
    virtual protected IEnumerator SpellChargeClock() {
        // we expect the charge time remaining to be equal to the total charge time when the coroutine is invoked
        Debug.Assert(Mathf.Abs(Data.ChargeTimeRemaining - Data.ChargeTime) <= float.Epsilon, $"Unexpected Spell Charge Time Remaining for {this.name}.");
        if (USING_DEBUG) {
            Debug.Log($"Player has started charging {this.name}.");            
        }
        bool finishedCharging = false;
       
        // TODO: start charge anims

        ParticleSystem.EmissionModule em = chargingParticles.emission;
        em.enabled = true;

        // while the user is still holding down the spell button, decrease the charge time remaining
        while (CrossPlatformInputManager.GetButton(fireButton)) {
            if (Data.ChargeTimeRemaining >= float.Epsilon) {
                Data.ChargeTimeRemaining -= Time.deltaTime;
                if (USING_DEBUG) Debug.Log($"Spell is charging. Time remaining: {Data.ChargeTimeRemaining}");
            } else { // Player has finished charging & released mouse
                if (USING_DEBUG) Debug.Log($"Spell with name {this.name} can be fired.");
                finishedCharging = true;
            } 
            yield return null;

            if (finishedCharging && CrossPlatformInputManager.GetButtonUp(fireButton)) {
                // invoke the spell & start cooldown!
                InvokeSpell(); 
                StartCoroutine(nameof(CoolDownTimer));
            }
        }

        if (!finishedCharging && USING_DEBUG) {
            // TODO: stop charge anims
            Debug.Log($"Spell was cancelled.");
        }

        // disable emission of charging particles and reset charge time
        em.enabled = false;
        Data.ChargeTimeRemaining = Data.ChargeTime;

    }
}
