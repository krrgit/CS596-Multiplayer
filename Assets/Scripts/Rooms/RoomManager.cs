using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private bool roomCleared;
    [SerializeField] private bool roomActive;
    [SerializeField] private SpawnEnemies spawner;
    [SerializeField] private GameObject activeRoomElements;
    [SerializeField] private GameObject clearedRoomElements;
    [SerializeField] private SpawnEnemies spawnEnemies;
    [SerializeField] private GameObject doors;
    [SerializeField] private GameObject[] doorPlugs;
    [SerializeField] private int openDoors;

    private void OnEnable()
    {
        if (spawnEnemies) spawnEnemies.enemiesClearedDelegate += ClearRoom;
    }
    
    private void OnDisable()
    {
        if (spawnEnemies) spawnEnemies.enemiesClearedDelegate -= ClearRoom;
    }

    public bool RoomActive
    {
        get { return roomActive; }
    }

    private void Start()
    {
        doors.SetActive(false);
    }

    public void StartRoom(Transform triggerPlayer)
    {
        if (roomCleared) return;
        roomActive = true;
        print("RoomManager: Started");
        if (spawner)
        {
            spawner.SetOpenDoors(openDoors);
            spawner.SetTriggerPlayer(triggerPlayer);
            spawner.StartSpawning();
            // spawner.gameObject.SetActive(true);
        }
        if (activeRoomElements) activeRoomElements.SetActive(true);
        if (doors)
        {
            doors.SetActive(true);
            SoundManager.Instance.PlayClip("door");
        }
        if (clearedRoomElements) clearedRoomElements.SetActive(false);
        
        
    }

    public void ClearRoom()
    {
        if (spawner) spawner.gameObject.SetActive(false);
        if (doors)
        {
            doors.SetActive(false);
            SoundManager.Instance.PlayClip("door");
        }
        if (clearedRoomElements) clearedRoomElements.SetActive(true);
        roomActive = false;
        roomCleared = true;
        
        print("Room Cleared");
    }

    public void PlugDoors(int doors)
    {
        openDoors = doors ^ 0b_1111;
        for (int i = 0; i < 4; i++)
        {
            doorPlugs[i].SetActive((doors & 1) == 1); 
            doors = doors >> 1;
        }
    }
    
    
}
