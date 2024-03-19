using System.Collections;
using Unity.BossRoom.Infrastructure;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

// TEST SPAWNER SCRIPT
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Singleton.OnServerStarted += SpawnEnemyStart;
        
    }

    void SpawnEnemyStart()
    {
        NetworkManager.Singleton.OnServerStarted -= SpawnEnemyStart;

        for (int i = 0; i < 3; i++)
        {
            SpawnEnemy();
        }

        StartCoroutine(SpawnOverTime());
    }

    IEnumerator SpawnOverTime()
    {
        while (NetworkManager.Singleton.ConnectedClients.Count > 0)
        {
            yield return new WaitForSeconds(2);
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        NetworkObject obj =
            NetworkObjectPool.Singleton.GetNetworkObject(prefab, GetRandomPosition(), quaternion.identity);

        obj.GetComponent<EnemyHealth>().prefab = prefab;
        if (!obj.IsSpawned) obj.Spawn(true);
    }

    Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
    }
    
    
}
