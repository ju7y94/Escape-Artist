using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class NetObjectSpawn : NetworkBehaviour
{
    public GameObject[] spawnablePrefabs;
    public int numObjectsToSpawn = 6;
    public float spawnRange = 10f;

    private void Start()
    {
        if (IsServer)
        {
            SpawnObjects();
        }
    }

    private void SpawnObjects()
    {
        List<Vector3> spawnPoints = new List<Vector3>();
        for (int i = 0; i < numObjectsToSpawn; i++)
        {
            spawnPoints.Add(new Vector3(Random.Range(-spawnRange, spawnRange), 0f, Random.Range(-spawnRange, spawnRange)));
        }

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            int index = Random.Range(0, spawnablePrefabs.Length);
            GameObject spawnedObject = Instantiate(spawnablePrefabs[i], spawnPoints[i], Quaternion.identity);
            NetworkObject networkObject = spawnedObject.GetComponent<NetworkObject>();
            if (networkObject != null)
            {
                networkObject.Spawn();
            }
            else
            {
                Debug.LogWarning("Spawned object does not have a NetworkObject component");
            }
        }
    }
}