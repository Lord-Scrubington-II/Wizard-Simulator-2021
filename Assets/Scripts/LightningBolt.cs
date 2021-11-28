using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class LightningBolt : Spell {

    [SerializeField] float range = 100f;
    [SerializeField] float spellEffectLifetime = 2.0f; // in seconds
    LineRenderer lineRenderer;

    protected override bool ThrowSpell() {

        Vector3 origin = FPCamera.transform.position;
        Vector3 direction = FPCamera.transform.forward;
        RaycastHit hit;
        Physics.Raycast(origin, direction, out hit, range);

        if (USING_DEBUG) {

            Vector3 destination = hit.transform.position;
            Debug.Log($"Ray hit {hit.transform.name}.");
            // Debug.DrawLine(origin, destination, Color.blue, spellEffectLifetime, true);

            // use a unity line renderer to display the raycast to player
            RenderLine(origin, destination);
            Invoke(nameof(HideLine), spellEffectLifetime);
        } else {
            

        }

        return true;
    }

    override protected void Start() {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    protected override bool ApplyEffectToPlayer() {
        return false;
    }

    private void RenderLine(Vector3 origin, Vector3 destination) {
        lineRenderer.enabled = true;
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.blue;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetPosition(1, destination);


    }

    private void HideLine() {
        lineRenderer.enabled = false;
    }
}
