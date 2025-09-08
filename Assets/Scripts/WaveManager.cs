using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]GameObject enemyPrefab;    
    int maxEnemyCount = 4;
    float xMinRange = 1f;
    float xMaxRange = 12f;
    float zMinRange = -30f;
    float zMaxRange = -25f;
    void Start()
    {
        for(int i = 0; i < maxEnemyCount; i++)
        {
            Instantiate(enemyPrefab, RandomPosition(), Quaternion.identity);
        }
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
}
