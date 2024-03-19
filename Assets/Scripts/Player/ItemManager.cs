using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private bool hasBossKey;
    [SerializeField] private Transform itemParent;

    public void CollectBossKey(Transform bossKey)
    {
        hasBossKey = true;
        bossKey.parent = itemParent;
        bossKey.position = itemParent.position;
        print("Collected Boss Key!");
    }

    public bool HasBossKey()
    {
        return hasBossKey;
    }
}
