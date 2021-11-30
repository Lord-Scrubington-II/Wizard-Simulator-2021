using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldActor : MonoBehaviour
{
    [SerializeField] int hitPoints = 10;

    public int HitPoints { get => hitPoints; private set => hitPoints = value; }

    void Start() { }
    void Update() { }
    
    virtual public void HealBy(int hp) {
        hitPoints += hp;
    }

    virtual public void DamageBy(int dmg) {
        hitPoints -= dmg;
        
        if (hitPoints <= 0) {
            DeathProcedure();
        }
    }

    abstract protected void DeathProcedure();
}
