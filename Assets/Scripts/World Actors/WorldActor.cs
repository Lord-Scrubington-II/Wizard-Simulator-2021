using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// World Actors are backing containers for the statistics of all 
/// world combatants and destructible objects, chief among them HP.
/// </summary>
public abstract class WorldActor : MonoBehaviour
{
    [SerializeField] int hitPoints = 10;

    public int HitPoints { get => hitPoints; private set => hitPoints = value; }

    virtual protected void Start() { }
    virtual protected void Update() { }
    
    virtual public void HealBy(int hp) {
        hitPoints += hp;
    }

    virtual public void DamageBy(int dmg) {
        hitPoints -= dmg;
        // Broadcast a message telling other modules that damage has been taken
        BroadcastMessage(nameof(OnDamageTaken), dmg);

        if (hitPoints <= 0) {
            DeathProcedure();
        }
    }

    virtual protected void OnDamageTaken(int amount) {
        Debug.Log($"O o f, my name is {gameObject.name} and I been hit. F in the chat.");
    }

    abstract protected void DeathProcedure();
}
