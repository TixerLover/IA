using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("References")]
    public GameObject enemyPrefab;
    public Transform player;

    [Header("Spawn Timing")]
    public float initialSpawnRate = 2.0f;  
    public float minimumSpawnRate = 0.2f;  
    public float difficultyIncrease = 0.05f; 

    private float currentSpawnRate;
    private float nextSpawnTime;

    void Start()
    {
        currentSpawnRate = initialSpawnRate;
    }

    void Update()
    {
        if (player == null) return;

        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + currentSpawnRate;

            if (currentSpawnRate > minimumSpawnRate)
            {
                currentSpawnRate -= difficultyIncrease;
            }
        }
    }

    void SpawnEnemy()
    {
        float spawnDistance = 50f;
        GameObject enemy = ObjectPooler.Instance.GetPooledEnemy();

        if (enemy != null)
        {
            float angle = Random.Range(0f, Mathf.PI * 2);
            Vector2 spawnPos = new Vector2(
                player.position.x + Mathf.Cos(angle) * spawnDistance,
                player.position.y + Mathf.Sin(angle) * spawnDistance
            );

            enemy.transform.position = spawnPos;
            enemy.SetActive(true); 
        }
    }
}
