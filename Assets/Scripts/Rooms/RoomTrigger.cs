using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField] private RoomManager roomManager;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        if (!roomManager)
        {
            print("Room Manager Not Referenced.");
            return;
        }

        if (roomManager.RoomActive)
        {
            print("Room already cleared.");
            return;
        }

        print("RoomTrigger: Triggered");
        roomManager.StartRoom(other.transform);
        gameObject.SetActive(false);
    }
}
