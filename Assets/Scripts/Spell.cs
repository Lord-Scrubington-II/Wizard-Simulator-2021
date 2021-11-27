using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

abstract public class Spell : MonoBehaviour {

    [SerializeField] protected bool USING_DEBUG = true;

    [SerializeField] protected Camera FPCamera;
    [SerializeField] protected ParticleSystem idleParticles;
    [SerializeField] private SpellDataModule spellData;

    /// <summary>
    /// Retrieve a reference to the Spell's Data Module
    /// </summary>
    public SpellDataModule Data { get => spellData; set => spellData = value; }

    public bool CanCast {
        get => Data.CoolDownRemaining == 0 && Data.ChargeTimeRemaining == Data.ChargeTime;
    }

    [System.Serializable]
    public class SpellDataModule { 
        // spell constants
        [SerializeField] private int damage = 5;
        [SerializeField] [Range(1.0f, 10.0f)] private float coolDownTime = 3.0f; // in seconds
        [SerializeField] [Range(0.0f, 4.0f)] private float chargeTime = 0.3f; // in seconds

        // these variables are used to implement timers
        private float coolDownRemaining;
        private float chargeTimeRemaining;

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

    /// <summary>
    /// Starts the full spell routine.
    /// </summary>
    /// <returns>True if the spell can be invoked, false if not.</returns>
    virtual public bool InvokeNoCoroutine() {
        if(!CanCast) {
            return false;
        }
        ThrowSpell();
        ApplyEffectToPlayer();

        return true;
    }


    /*
    virtual public bool Invoke() {
        if (!CanCast) {
            return false;
        }
        if(CrossPlatformInputManager.GetButton(fireButton)) {
            StartCoroutine(nameof(SpellChargeClock));
            
        }
        return true;
    } */


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

    abstract protected void Start();
    virtual protected void Update() {
        if (CrossPlatformInputManager.GetButtonDown(fireButton)) {
            InvokeNoCoroutine();
        }
    }


    // Input Consts:
    public const string fireButton = "Fire";

    /// <summary>
    /// This is a Coroutine that is to be started after the player has finished casting a spell.
    /// </summary>
    virtual protected IEnumerable CoolDownTimer() {

        // start the cooldown timer and let other procedures check if the spell can be cast
        Data.CoolDownRemaining = Data.CoolDownTime;
        if (Data.CoolDownRemaining >= float.Epsilon) {
            Data.CoolDownRemaining += Time.deltaTime;
            yield return null;
        }

        if(USING_DEBUG) {
            Debug.Log($"Spell with name {this.name} can be cast.");
        }
    }

    /// <summary>
    /// This is a Coroutine that is to be started after the player has started to cast a spell.
    /// </summary>
    virtual protected IEnumerable SpellChargeClock() {
        // we expect the charge time remaining to be equal to the total charge time when the coroutine is invoked
        if (Data.ChargeTimeRemaining != Data.ChargeTime) {
            throw new System.FormatException($"Unexpected Spell Charge Time Remaining for {this.name}.");
        }
        
        if (Data.ChargeTimeRemaining > float.Epsilon && CrossPlatformInputManager.GetButtonDown(fireButton)) {
            Data.ChargeTimeRemaining -= Time.deltaTime;

            if(Data.ChargeTimeRemaining <= float.Epsilon) {
                // TODO: fire event saying that the spell can be released OR directly invoke
                if (USING_DEBUG) {
                    Debug.Log($"Spell with name {this.name} can be fired.");
                }
            } else {
                yield return null;
            }
        } else {
            // stop anims and reset charge time
            Data.ChargeTimeRemaining = Data.ChargeTime;
        }
    }
}
