using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealPlayerDamage : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    public delegate void HitPlayerDelegate();
    public HitPlayerDelegate hitDelegate;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return; // Ignore collision if not enemy

        PlayerHealth player = other.transform.parent.GetComponent<PlayerHealth>();
        if (player)
        {
            player.DealDamage(damage);
            hitDelegate?.Invoke();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return; // Ignore collision if not enemy
        print("PlayerDamage: OnCollisionEnter " + collision.gameObject.name);
        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
        if (player)
        {
            player.DealDamage(damage);
            hitDelegate?.Invoke();
        }
    }
}
