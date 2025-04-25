using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    [Serialize] private float spawnInterval = 1;
    private float timeSinceLastSpawn = 0;
    [Serialize] private Transform enemiesContainer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn > spawnInterval)
        {
            timeSinceLastSpawn = 0;
            // Instantiate(); we want to instantiate the prefab here
            Debug.Log("Enemy spawned");
        }
    }
}
