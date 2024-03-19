using System.Collections;
using System.Collections.Generic;
using Unity.BossRoom.Infrastructure;
using Unity.Netcode;
using UnityEngine;

public class EnemyHealth : NetworkBehaviour
{
    [SerializeField] private int health = 1;
    public GameObject prefab;

    public delegate void EnemyTakeDamage();

    public EnemyTakeDamage enemyTakeDamageDelegate;
    
    public void DealDamage(int damage)
    {
        enemyTakeDamageDelegate?.Invoke();
        
        if (!NetworkManager.Singleton.IsServer) return;

        health -= damage > health ? health : damage;
        print("Damage Dealt. Health: " + health);
        if (health <= 0)
        {
            // Destroy(gameObject);
            if (prefab)
            {
                // For object pooled enemies
                NetworkObject.transform.parent = null;
                NetworkObject.Despawn(false);
                NetworkObjectPool.Singleton.ReturnNetworkObject(NetworkObject, prefab);
            }
            else
            {
                if (NetworkObject) NetworkObject.Despawn(false);
            }
        }
    }

    // public void DealDamage(int damage)
    // {
    //     if (!NetworkManager.Singleton.IsServer) return;
    //
    //     enemyTakeDamageDelegate?.Invoke();
    //     health -= damage > health ? health : damage;
    //
    //     print("Damage Dealt. Health: " + health);
    //     if (health <= 0)
    //     {
    //         // Destroy(gameObject);
    //         NetworkObjectPool.Singleton.ReturnNetworkObject(NetworkObject, prefab);
    //         NetworkObject.();
    //     }
    // }
}
