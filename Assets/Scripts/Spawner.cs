using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InsaneBar insaneBar;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform player;
    public InsaneBar insaneBarScript;

    [Header("Difficulty")]
    [Range(0.1f, 5f)]
    [SerializeField] private float difficultyMultiplier = 1f;

    private float spawnTimer;
    private int currentEnemies;

    private bool gameOverMode = false;

    private const float MIN_SPAWN_INTERVAL = 0.3f;
    private const float MAX_SPAWN_INTERVAL = 4f;

    private const int BASE_MAX_ENEMIES = 3;
    private const int MAX_ENEMIES_LIMIT = 50;

    void Start()
    {
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
        if (insaneBar == null) return;
        if (enemyPrefabs.Length == 0) return;

        float insanity = insaneBar.GetInsanity();

        float spawnInterval = Mathf.Lerp(
            MAX_SPAWN_INTERVAL,
            MIN_SPAWN_INTERVAL,
            insanity * difficultyMultiplier
        );

        if (gameOverMode)
        {
            spawnInterval /= 5f;
        }

        int maxEnemies = Mathf.RoundToInt(
            Mathf.Lerp(
                BASE_MAX_ENEMIES,
                MAX_ENEMIES_LIMIT,
                insanity * difficultyMultiplier
            )
        );

        if (gameOverMode)
        {
            maxEnemies *= 5;
        }

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval && currentEnemies < maxEnemies)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }
    }

    void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject enemyPrefab =
            enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        GameObject enemyObj = Instantiate(
            enemyPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        Enemy enemy = enemyObj.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.player = player;
            enemy.spawner = this;
            enemy.insaneBar = insaneBarScript;
        }

        currentEnemies++;
    }

    public void OnEnemyKilled()
    {
        currentEnemies--;

        if (currentEnemies < 0)
        {
            currentEnemies = 0;
        }
    }

    public void EnableGameOverMode()
    {
        gameOverMode = true;
    }
}