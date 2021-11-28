using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.LightningGenerator;
using UnityStandardAssets.CrossPlatformInput;

/// <summary>
/// The Lightning Bolt is a hitscan spell that is sent out and damages a single enemy
/// after being charged for a brief period.
/// </summary>
public class LightningBolt : Spell {

    [SerializeField] float range = 100f;
    [SerializeField] float spellEffectLifetime = 1.5f; // in seconds
    [SerializeField] GameObject boltGenerator;
    LightningBoltScript boltGeneratorScript;
    LineRenderer lightningRenderer;
    LineRenderer debugLineRenderer;

    protected override bool ThrowSpell() {

        Vector3 origin = FPCamera.transform.position;
        Vector3 direction = FPCamera.transform.forward;

        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, range)) {
            Vector3 destination = hit.transform.position;
            Enemy target = hit.transform.GetComponent<Enemy>();

            if (USING_DEBUG) {
                Debug.Log($"Ray hit {hit.transform.name}.");
                // Debug.DrawLine(origin, destination, Color.blue, spellEffectLifetime, true);

                // use a unity line renderer to display the raycast to player
                RenderLine(origin, destination);
                Invoke(nameof(HideLine), spellEffectLifetime);
            }

            //FIXME: invoke the lightning generator here
            RenderLightning(origin, destination);
            Invoke(nameof(HideLightning), spellEffectLifetime);

            return true;
        } else {
            return false;
        }
    }

    protected override bool ApplyEffectToPlayer() {
        return false;
    }

    override protected void Start() {
        if (USING_DEBUG) {
            debugLineRenderer = gameObject.GetComponent<LineRenderer>();
            debugLineRenderer.enabled = false;
        }
        boltGeneratorScript = boltGenerator.GetComponent<LightningBoltScript>();
        boltGeneratorScript.enabled = true;
        lightningRenderer = boltGenerator.GetComponent<LineRenderer>();
        lightningRenderer.enabled = false;
    }

    private void RenderLightning(Vector3 origin, Vector3 destination) {
        lightningRenderer.enabled = true;
        boltGeneratorScript.StartPosition = FPCamera.transform.forward * 2;
        boltGeneratorScript.EndPosition = destination;
    }

    private void HideLightning() {
        lightningRenderer.enabled = false;
    }

    private void RenderLine(Vector3 origin, Vector3 destination) {
        debugLineRenderer.enabled = true;
        debugLineRenderer.startColor = Color.white;
        debugLineRenderer.endColor = Color.blue;
        debugLineRenderer.startWidth = 0.05f;
        debugLineRenderer.endWidth = 0.05f;

        debugLineRenderer.SetPosition(0, origin);
        debugLineRenderer.SetPosition(1, destination);
    }

    private void HideLine() {
        debugLineRenderer.enabled = false;
    }
}
