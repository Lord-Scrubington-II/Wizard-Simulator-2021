using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int hitPoints = 10;

    public void DamageBy(int dmg) {
        hitPoints -= dmg;

        if (hitPoints <= 0) {
            GameObject.Destroy(gameObject);
        }
    }
}
