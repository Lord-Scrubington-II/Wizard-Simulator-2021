using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : WorldActor
{
    private void Start() { }

    public void OnAttackHitEvent() {
        Debug.Log("oof");
    }

    protected override void DeathProcedure() {
        Debug.Log("You dead, my blip blop.");

        // invoke procedure to stop game and display game over screen
    }
}
