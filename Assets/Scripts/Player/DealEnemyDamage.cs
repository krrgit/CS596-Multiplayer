using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealEnemyDamage : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    public delegate void HitEnemyDelegate();
    public HitEnemyDelegate hitDelegate;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy")) return; // Ignore collision if not enemy

        EnemyHealth enemy = other.transform.parent.GetComponent<EnemyHealth>();
        if (enemy)
        {
            enemy.DealDamage(damage);
            hitDelegate?.Invoke();
        }

    }
}
