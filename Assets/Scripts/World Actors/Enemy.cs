using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class Enemy : WorldActor
{
    // [SerializeField] private Transform attackTarget;
    private WorldActor attackTargetActor;
    [SerializeField] int damage = 20;

    private void Start() {
        attackTargetActor = FindObjectOfType<Player>(); // probably not super smart but it works?
    }

    public void OnAttackHitEvent() {

        // if (attackTarget == null) return;
        // attackTargetActor = attackTarget.GetComponent<WorldActor>();
        
        attackTargetActor.DamageBy(damage);
        Debug.Log("bang bang");
    }

    protected override void DeathProcedure() {
        GameObject.Destroy(gameObject);
    }
}
