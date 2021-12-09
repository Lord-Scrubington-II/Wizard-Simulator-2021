using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : WorldActor
{
    [SerializeField] Canvas gameOverCanvas;

    private void Start() {
        gameOverCanvas.enabled = false;
    }

    public void OnAttackHitEvent() {
        Debug.Log("oof");
    }

    /// <summary>
    /// The player's death procedure is the only one that can
    /// invoke the game over screen.
    /// </summary>
    protected override void DeathProcedure() {
        Debug.Log("You dead, my blip blop.");
        EnableDeathUI();

        // invoke procedure to stop game and display game over screen
    }

    private void EnableDeathUI() {
        gameOverCanvas.enabled = true;
        Time.timeScale = 0; // no gaming allowed
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
