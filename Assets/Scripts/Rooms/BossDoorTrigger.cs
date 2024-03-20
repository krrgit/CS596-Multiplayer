using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossDoorTrigger : MonoBehaviour
{
    [SerializeField] private bool doorUnlocked;
    [SerializeField] private GameObject topDoor;
    [SerializeField] private RoomManager roomManager;

    void Start()
    {
        topDoor.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        // If not player, door is already unlocked, or room is active, ignore
        if (!other.gameObject.CompareTag("Player") || doorUnlocked || roomManager.RoomActive) return;

        // get itemmanager
        var itemManager = other.transform.parent.GetComponent<ItemManager>();
        
        // check for key
        if (itemManager.HasBossKey())
        {
            // unlock if check succeeds
            UnlockDoor();
        }
        else
        {
            print("Need Boss Key");
        }
        // do nothing if fails
    }

    void UnlockDoor()
    {
        topDoor.SetActive(false);
        doorUnlocked = true;
        SoundManager.Instance.PlayClip("explosion02");
        print("Boss Room Unlocked");
    }
}
