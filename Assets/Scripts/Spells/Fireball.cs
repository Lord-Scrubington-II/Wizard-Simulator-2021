using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Spell {
    [SerializeField] float projectileSpeed = 8.0f;
    [SerializeField] float blastRadius = 5.0f;
    [SerializeField] GameObject payloadPrefab;

    protected override bool ApplyEffectToPlayer() {
        return false;
    }

    protected override bool ThrowSpell() {

        Vector3 origin = FPCamera.transform.position;
        Vector3 direction = FPCamera.transform.forward;

        GameObject thisFireball = GameObject.Instantiate(payloadPrefab, origin, Quaternion.identity);
        // ProjectileControlScript pcs = thisFireball.getComponent<ProjectileControlScript>();
        // pcs.velocity = direction * projectileVelocity;

        return true;
    }
}

