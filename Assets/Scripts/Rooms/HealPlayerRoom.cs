using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPlayerRoom : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        other.GetComponent<PlayerHealth>().RestoreMaxHealth();
    }
}
