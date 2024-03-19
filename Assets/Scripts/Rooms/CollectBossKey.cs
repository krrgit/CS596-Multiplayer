using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectBossKey : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        print("Collected Boss Key");
        
        other.transform.parent.GetComponent<ItemManager>().CollectBossKey(transform);
        StartCoroutine(CollectAnim());
    }

    IEnumerator CollectAnim()
    {
        yield return new WaitForSeconds(1.2f);
        Destroy(gameObject);
    }
}
