using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatternSpawner : MonoBehaviour
{
    public List<WaveData> waves;
    public float timeBetweenWaves = 3f;

    void Start()
    {
        StartCoroutine(LevelRoutine());
    }

    IEnumerator LevelRoutine()
    {
        foreach (WaveData wave in waves)
        {
            yield return StartCoroutine(SpawnWave(wave));
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    IEnumerator SpawnWave(WaveData data)
    {
        for (int i = 0; i < data.enemyCount; i++)
        {
            GameObject enemy = Instantiate(data.enemyPrefab, data.startPoint, Quaternion.identity);

            // Pass the pattern points to the enemy
            EnemyMovement moveScript = enemy.GetComponent<EnemyMovement>();
            if (moveScript != null)
            {
                //moveScript.SetupPath(data.startPoint, data.controlPoint, data.endPoint);
            }

            yield return new WaitForSeconds(data.spawnRate);
        }
    }
}