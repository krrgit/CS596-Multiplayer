using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn; // The GameObject to spawn
    [SerializeField] float width = 10f; // Width of the rectangle
    [SerializeField] float height = 5f; // Height of the rectangle
    [SerializeField] private float doorWidth = 3f;
    [SerializeField] Vector3 center = Vector3.zero; // Center of the rectangle
    [SerializeField] float spawnInterval = 2f; // Time interval between spawns
    [SerializeField] float waveInterval = 5f; // Time interval between waves
    [SerializeField] private bool waveActive = true;
    [SerializeField] private int totalWaves = 5;
    [SerializeField] private int wavesLeft = 0;
    [SerializeField] private bool enemiesCleared;
    [SerializeField] private int openDoors;

    [Header("Random Variables")] 
    [SerializeField] private float minWaveInterval = 0.5f;
    [SerializeField] private float maxWaveInterval = 3.5f;
    [SerializeField] private int minWaveCount = 1;
    [SerializeField] private int maxWaveCount = 5;

    private float timer; // Timer for spawn intervals

    public delegate void EnemiesCleared();
    public EnemiesCleared enemiesClearedDelegate;

    void Start()
    {
        timer = spawnInterval;
        RandomizeWaveInterval();
        totalWaves = Random.Range(minWaveCount, maxWaveCount + 1);
        wavesLeft = totalWaves;
        
        StartCoroutine(ISpawnWave());
    }

    void RandomizeWaveInterval()
    {
        waveInterval = Random.Range(minWaveInterval, maxWaveInterval);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        
        if (spawnInterval !=0 && timer <= 0f)
        {
            SpawnEnemy(RandomizePosition());
            timer = spawnInterval;
        }

        ClearRoomCheck();
    }

    void ClearRoomCheck()
    {
        if (wavesLeft > 0) return;
        if (!enemiesCleared && transform.childCount <= 0)
        {
            enemiesCleared = true;
            enemiesClearedDelegate();
        }
    }

    void SpawnEnemy(Vector3 position)
    {
        // Calculate the random edge position
        Vector3 spawnPosition = transform.position + center + position;

        // Instantiate the object at the calculated position
        var go = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        go.transform.parent = transform;
    }

    public void SetOpenDoors(int openDoors)
    {
        this.openDoors = openDoors;
    }

    IEnumerator ISpawnWave()
    {
        while (wavesLeft > 0)
        {
            
            // 0001 to 1111; Use bits to determine spawnpoint.
            // Mask with open doors to only spawn in doorways
            int sidesToSpawn = Random.Range(1, 16) & openDoors; 

            // In case open doors is unknown, just spawn from anywhere
            if (openDoors != 0)
            {
                // Ensure there's always an enemy to spawn.
                // Will spawn as long as sidesToSpawn > 1
                while (sidesToSpawn == 0)
                {
                    sidesToSpawn = Random.Range(1, 16) & openDoors;
                }
            }
            
            // Check each bit to see if an enemy is to be spawned
            for (int i = 0; i < 4; ++i)
            {
                if ((sidesToSpawn & 0b_0001) == 1)
                    SpawnEnemy(GetSpawnPosition(i));
                sidesToSpawn = sidesToSpawn >> 1;
            }

            wavesLeft--;
            yield return new WaitForSeconds(waveInterval);
            RandomizeWaveInterval();
        }

        waveActive = false;
    }

    Vector3 RandomizePosition()
    {
        Vector3 randomVector = Vector3.zero;
        float offset = Random.Range(-1f,1f) * doorWidth/2f;
        int side = Random.Range(-1, 1) == 0 ? -1 : 1;
        randomVector = Random.Range(-1,1) == 0 ? new Vector3(offset, 0,side * height * 0.5f) :  new Vector3(side * width * 0.5f, 0, offset);
        return randomVector;
    }

    Vector3 GetSpawnPosition(int spawnPosition)
    {
        // 0/1 - Top/Bottom; 2/3 - Left/Right
        bool isHorz = spawnPosition >= 2 ? true : false;
        int side = (spawnPosition == 0 || spawnPosition == 3) ? 1 : -1;
        
        Vector3 randomVector = Vector3.zero;
        float offset = Random.Range(-1f,1f) * doorWidth/2f;
        // Top/Bottom Spawn : Left/Right Spawn
        randomVector = isHorz ? new Vector3(side * width * 0.5f, 0, offset) : new Vector3(offset, 0,side * height * 0.5f);
        return randomVector;
    }
    
    void OnDrawGizmos()
    {
        // Draw rectangle perimeter
        Vector3 topLeft = transform.position + center + new Vector3(-width / 2, 0, height / 2);
        Vector3 topRight = transform.position + center + new Vector3(width / 2, 0, height / 2);
        Vector3 bottomRight = transform.position + center + new Vector3(width / 2, 0,-height / 2);
        Vector3 bottomLeft = transform.position + center + new Vector3(-width / 2, 0,-height / 2);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}
