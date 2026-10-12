using UnityEngine;

[CreateAssetMenu(fileName = "NewWave", menuName = "BulletHell/Wave")]
public class WaveData : ScriptableObject
{
    public GameObject enemyPrefab;
    public int enemyCount = 5;
    public float spawnRate = 0.5f;

    // Pattern Points
    public Vector3 startPoint;
    public Vector3 controlPoint; // For the curve
    public Vector3 endPoint;
}