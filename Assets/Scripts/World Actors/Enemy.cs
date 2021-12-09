using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
public class Enemy : WorldActor
{
    // [SerializeField] private Transform attackTarget;
    private WorldActor attackTargetActor;
    [SerializeField] int damage = 20;

    public bool HasDied { get; private set; }

    override protected void Start() {
        HasDied = false;
        attackTargetActor = FindObjectOfType<Player>(); // probably not super smart but it works?
    }

    public void OnAttackHitEvent() {

        // if (attackTarget == null) return;
        // attackTargetActor = attackTarget.GetComponent<WorldActor>();
        
        attackTargetActor.DamageBy(damage);
        Debug.Log("bang bang");
    }

    protected override void DeathProcedure() {
        //GameObject.Destroy(gameObject);
        if (HasDied) return;
        HasDied = true;
        GetComponent<Animator>().SetTrigger("onDead");
    }
}
