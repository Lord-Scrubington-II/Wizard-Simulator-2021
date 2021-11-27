using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class LightningBolt : Spell {

    [SerializeField] float range = 100f;

    /// <summary>
    /// Start() Override
    /// </summary>
    protected override void Start() {

    }

    protected override bool ThrowSpell() {

        RaycastHit hit;
        Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward, out hit, range);

        if (USING_DEBUG) {
            Debug.Log($"Ray hit {hit.transform.name}.");
        }

        return true;
    }

    protected override bool ApplyEffectToPlayer() {
        return false;
    }
}
