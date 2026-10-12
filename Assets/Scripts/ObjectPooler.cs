using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance; 

    public GameObject enemyPrefab;
    public int poolSize = 20;
    private List<GameObject> pooledEnemies;

    void Awake()
    {
        Instance = this;
        pooledEnemies = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(enemyPrefab);
            obj.SetActive(false);
            pooledEnemies.Add(obj);
        }
    }

    public GameObject GetPooledEnemy()
    {
        foreach (GameObject enemy in pooledEnemies)
        {
            if (!enemy.activeInHierarchy)
            {
                return enemy;
            }
        }

        GameObject newObj = Instantiate(enemyPrefab);
        newObj.SetActive(false);
        pooledEnemies.Add(newObj);
        return newObj;
    }
}