using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public float spawnRate = 1f;
    public int level = 1;
    public int maxEnemies = 100;
    public int enemiesSpawned = 0;

    void Start()
    {
        // Subscribe to enemy died event
        EventBus.Instance.OnEnemyDied += OnEnemyDied;
    }

    void OnEnable()
    {
        StartCoroutine(SpawnEnemy());
        // Subscribe to enemy died event
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    // Clean up
    void OnDestroy()
    {
        // Unsubscribe from enemy died event
        EventBus.Instance.OnEnemyDied -= OnEnemyDied;
        // Stop spawning enemies
        StopAllCoroutines();
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

                // Spawn enemy at calculated position
                GameObject enemy = Instantiate(
                    enemyPrefabs[Random.Range(0, enemyPrefabs.Length)],
                    spawnPosition,
                    Quaternion.identity
                );
                // Make enemy a child of the spawner
                enemy.transform.SetParent(transform);
                // Increase enemies spawned
                enemiesSpawned++;
            }
            yield return null;
        }
    }

    // decrease enemies spawned when an enemy dies
    void OnEnemyDied(Enemy enemy)
    {
        enemiesSpawned--;
    }
}
