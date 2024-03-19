using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int health = 1;

    public delegate void EnemyTakeDamage();

    public EnemyTakeDamage enemyTakeDamageDelegate;

    public void DealDamage(int damage)
    {
        health -= damage > health ? health : damage;
        enemyTakeDamageDelegate?.Invoke();

        print("Damage Dealt. Health: " + health);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
