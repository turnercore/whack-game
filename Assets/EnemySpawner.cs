using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyPrefab
{
    public GameObject enemy;
    public float spawnChance;
}

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private List<EnemyPrefab> enemyPrefabs = new List<EnemyPrefab>();

    [SerializeField]
    public float spawnRate = 1f;
    public int level = 1;
    public int maxEnemies = 100;
    public int enemiesSpawned = 0;

    private List<GameObject> enemySpawnBag = new List<GameObject>();

    void Start()
    {
        // Initialize the spawn bag
        InitializeSpawnBag();
        // Subscribe to enemy died event
        EventBus.Instance.OnEnemyDied += OnEnemyDied;
    }

    void OnEnable()
    {
        StartCoroutine(SpawnEnemy());
    }

    void OnDisable()
    {
        StopAllCoroutines();
        // kill all children and stop their coroutines
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    // Clean up
    void OnDestroy()
    {
        // Unsubscribe from enemy died event
        EventBus.Instance.OnEnemyDied -= OnEnemyDied;
        // Stop spawning enemies
        StopAllCoroutines();
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    // Initialize the spawn bag based on enemy spawn chances
    private void InitializeSpawnBag()
    {
        enemySpawnBag.Clear();
        foreach (var entry in enemyPrefabs)
        {
            for (int i = 0; i < entry.spawnChance; i++)
            {
                enemySpawnBag.Add(entry.enemy);
            }
        }
    }

    // Spawn enemies at random points off screen
    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            if (enemiesSpawned < maxEnemies)
            {
                // Wait for spawn rate
                yield return new WaitForSeconds(1 / spawnRate);

                // Choose a random edge (0: bottom, 1: top, 2: left, 3: right)
                int edge = Random.Range(0, 4);
                Vector2 spawnPosition = Vector2.zero;

                // Determine the spawn position based on the chosen edge
                switch (edge)
                {
                    case 0: // Bottom edge
                        spawnPosition = Camera.main.ViewportToWorldPoint(
                            new Vector3(Random.value, 0f, Camera.main.nearClipPlane)
                        );
                        spawnPosition.y -= 2f; // Move downwards to be off-screen
                        break;
                    case 1: // Top edge
                        spawnPosition = Camera.main.ViewportToWorldPoint(
                            new Vector3(Random.value, 1f, Camera.main.nearClipPlane)
                        );
                        spawnPosition.y += 2f; // Move upwards to be off-screen
                        break;
                    case 2: // Left edge
                        spawnPosition = Camera.main.ViewportToWorldPoint(
                            new Vector3(0f, Random.value, Camera.main.nearClipPlane)
                        );
                        spawnPosition.x -= 2f; // Move left to be off-screen
                        break;
                    case 3: // Right edge
                        spawnPosition = Camera.main.ViewportToWorldPoint(
                            new Vector3(1f, Random.value, Camera.main.nearClipPlane)
                        );
                        spawnPosition.x += 2f; // Move right to be off-screen
                        break;
                }

                // Pick a prefab from the spawn bag
                GameObject spawnedEnemy = GetEnemyFromSpawnBag();

                if (spawnedEnemy == null)
                {
                    Debug.LogError("No enemy prefab found");
                    continue;
                }

                // Spawn enemy at calculated position
                GameObject enemy = Instantiate(spawnedEnemy, spawnPosition, Quaternion.identity);
                // Make enemy a child of the spawner
                enemy.transform.SetParent(transform);
                // Increase enemies spawned
                enemiesSpawned++;
            }
            yield return null;
        }
    }

    // Get an enemy prefab from the spawn bag
    GameObject GetEnemyFromSpawnBag()
    {
        if (enemySpawnBag.Count == 0)
        {
            // Refill the bag when empty
            InitializeSpawnBag();
        }

        // Get a random enemy from the bag
        int randomIndex = Random.Range(0, enemySpawnBag.Count);
        GameObject chosenEnemy = enemySpawnBag[randomIndex];
        enemySpawnBag.RemoveAt(randomIndex);
        return chosenEnemy;
    }

    // Decrease enemies spawned when an enemy dies
    void OnEnemyDied(Enemy enemy)
    {
        enemiesSpawned--;
    }
}
