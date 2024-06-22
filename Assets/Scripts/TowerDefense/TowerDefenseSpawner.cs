using System.Collections;
using UnityEngine;

public class TowerDefenseSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public float spawnInterval = 3f; // Base interval in seconds
    public float timeVariation = 1f; // Max variation in seconds
    public float positionVariation = 5f; // Max variation in position

    private void Start()
    {
        StartCoroutine(SpawnPrefab());
    }

    private IEnumerator SpawnPrefab()
    {
        while (true)
        {
            float interval = spawnInterval + Random.Range(-timeVariation, timeVariation);
            yield return new WaitForSeconds(interval);

            Vector3 spawnPosition = transform.position + new Vector3(
                Random.Range(-positionVariation, positionVariation),
                Random.Range(-positionVariation, positionVariation),
                Random.Range(-positionVariation, positionVariation)
            );

            Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        }
    }
}
