using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Setup")]
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    public Transform player;

    [Header("Spawning")]
    public float spawnInterval = 2f;
    public float spawnRadius = 5f; // fallback if no spawn points
    public int maxEnemies = 10;

    private float timer;
    private int currentEnemies = 0;

    void Start()
    {
        // Auto-find player if not assigned
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval && currentEnemies < maxEnemies)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0) return;

        // Pick random enemy prefab
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        Vector3 spawnPosition;

        // Use spawn points if available
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            spawnPosition = spawnPoint.position;
        }
        else
        {
            // fallback: random area around spawner
            Vector3 randomOffset = new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                0f,
                Random.Range(-spawnRadius, spawnRadius)
            );

            spawnPosition = transform.position + randomOffset;
        }

        // Spawn enemy
        GameObject enemyObj = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Assign references
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.player = player;
            enemy.spawner = this;
        }

        currentEnemies++;
    }

    public void OnEnemyKilled()
    {
        currentEnemies--;
    }
}