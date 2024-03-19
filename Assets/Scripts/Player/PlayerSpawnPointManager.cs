using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPointManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int currentSpawnPoint;
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
        Vector3 position = spawnPoints[currentSpawnPoint++].position;

        currentSpawnPoint = currentSpawnPoint >= spawnPoints.Length ? 0 : currentSpawnPoint;
        return position;
    }
}
