using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]GameObject enemyPrefab;    
    float xMinRange = 1f;
    float xMaxRange = 12f;
    float zMinRange = -30f;
    float zMaxRange = -25f;

    int counter = 0;
    void Start()
    {
        StartCoroutine(SpawnWave(enemyPrefab, 10000, .005f));
    }

    private Vector3 RandomPosition()
    {
        float x = Random.Range(xMinRange, xMaxRange);
        float z = Random.Range(zMinRange, zMaxRange);

        return new Vector3(x, 1, z);
    }
    
    IEnumerator SpawnWave(GameObject prefab, int numberOfEnemies, float delayBetweenSpawns, float delayBeforeStartingWave = 0)
    {

        if(delayBeforeStartingWave > 0) yield return new WaitForSeconds(delayBeforeStartingWave);
        for (int i = 0; i < numberOfEnemies; i++)
        {
            yield return new WaitForSeconds(delayBetweenSpawns);
            counter++;
            PoolManager.Instance.GetObjectFromPool(prefab, RandomPosition(), Quaternion.identity);
            Debug.Log($"Enemy #: {counter}");
        }
    }
}
