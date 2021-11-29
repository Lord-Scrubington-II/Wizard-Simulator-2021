using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Spell {
    protected override bool ApplyEffectToPlayer() {
        return false;
    }

    protected override bool ThrowSpell() {
        throw new System.NotImplementedException();
    }
}

