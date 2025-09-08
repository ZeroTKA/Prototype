using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]GameObject enemyPrefab;    
    float xMinRange = 1f;
    float xMaxRange = 12f;
    float zMinRange = -30f;
    float zMaxRange = -25f;
    void Start()
    {
        StartCoroutine(SpawnWave(enemyPrefab, 10, .5f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector3 RandomPosition()
    {
        float x = Random.Range(xMinRange, xMaxRange);
        float z = Random.Range(zMinRange, zMaxRange);

        return new Vector3(x, 1, z);
    }
    
    IEnumerator SpawnWave(GameObject prefab, int numberOfEnemies, float delayBetweenSpawns)
    {        
        for (int i = 0; i < numberOfEnemies; i++)
        {
            yield return new WaitForSeconds(delayBetweenSpawns);
            Instantiate(prefab, RandomPosition(), Quaternion.identity);
        }
    }
    IEnumerator SpawnWave(GameObject prefab, int numberOfEnemies, float delayBetweenSpawns, 
                          GameObject prefabTwo, int numberOfEnemiesTwo, float delayBetweenSpawns2)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            yield return new WaitForSeconds(delayBetweenSpawns);
            Instantiate(prefab, RandomPosition(), Quaternion.identity);
        }
        for (int i = 0; i < numberOfEnemiesTwo; i++)
        {
            yield return new WaitForSeconds(delayBetweenSpawns2);
            Instantiate(prefabTwo, RandomPosition(), Quaternion.identity);
        }
    }
}
