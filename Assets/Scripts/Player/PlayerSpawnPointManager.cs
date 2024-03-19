using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSpawnPointManager : NetworkBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private NetworkVariable<int> netSpawnPoint;
    public static PlayerSpawnPointManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public Vector3 GetSpawnPoint()
    {
        if (NetworkObject.IsOwner)
        {
            int current = netSpawnPoint.Value;
            Vector3 position = spawnPoints[current++].position;
            
            current = current >= spawnPoints.Length ? 0 : current;
            netSpawnPoint.Value = current;
            print("Update Spawn point");
            return position;
        }
        else
        {
            int current = netSpawnPoint.Value;
            print("Don't Update Spawn Point");
            return spawnPoints[current].position;
        }
    }
}
