using System;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

public class SpellRouter : MonoBehaviour {
    [SerializeField] int currentSpell = 0;

    Dictionary<KeyCode, int> KeyToWeaponIndex = new Dictionary<KeyCode, int>() {
        { KeyCode.Alpha1, 0 }, {KeyCode.Alpha2, 1}, {KeyCode.Alpha3, 2}, {KeyCode.Alpha4, 3 }
    };

    // Start is called before the first frame update
    void Start()
    {
        SetSpellActive();
    }

    private void SetSpellActive() {
        int spellIndex = 0;

        foreach(Transform spell in this.transform) {
            if (spellIndex == currentSpell) {
                spell.gameObject.SetActive(true);
            } else {
                spell.gameObject.SetActive(false);
            }
            spellIndex++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        int previousSpell = currentSpell;
        RouteByKeyDown();
        RouteByScrollWheel();

        if(previousSpell != currentSpell) {
            SetSpellActive();
        }
        
    }

    private void RouteByScrollWheel() {
        if(Input.GetAxis("Mouse ScrollWheel") > 0) {
            currentSpell = (currentSpell + 1) % transform.childCount;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0) {
            
            if (currentSpell <= 0) {
                currentSpell = transform.childCount - 1;
            } else {
                currentSpell--;
            }
        }
    }

    private void RouteByKeyDown() {
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            currentSpell = KeyToWeaponIndex[KeyCode.Alpha1];
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            currentSpell = KeyToWeaponIndex[KeyCode.Alpha2];
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            currentSpell = KeyToWeaponIndex[KeyCode.Alpha3];
        }
    }
}
