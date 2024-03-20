using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSpawnPointManager : NetworkBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private NetworkVariable<int> netSpawnPoint;
    [SerializeField] private int point;
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

    public override void OnNetworkSpawn()
    {
        if (NetworkObject.IsOwner)
        {
            netSpawnPoint.Value = point;
        }
        else
        {
            point = netSpawnPoint.Value;
            print("read spawn point: " + point);
        }
    }

    public Vector3 GetSpawnPoint()
    {
        if (NetworkObject.IsOwner)
        {
            Vector3 position = spawnPoints[point++].position;
            
            point = point >= spawnPoints.Length ? 0 : point;
            netSpawnPoint.Value = point;
            print("Update Spawn point: " + netSpawnPoint.Value);
            return position;
        }
        else
        {
            point = netSpawnPoint.Value;
            print("Don't Update Spawn Point: " + netSpawnPoint.Value);
            return spawnPoints[point].position;
        }
    }
}
