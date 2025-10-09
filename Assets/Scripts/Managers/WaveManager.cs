using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]GameObject enemyPrefab;
    [SerializeField] int numberOfEnemeis;
    [SerializeField] float spawnInterval;

    // -- Tracking end of Wave things -- //
    bool isSubscribedToEnemyEvent = false; // enemies push event when death occurs
    int aliveEnemies = 0;
    bool isFinishedSpawningEnemies = false;

    // -- Spawn area 1 -- //
    readonly float xMinRange = 1f;
    readonly float xMaxRange = 12f;
    readonly float zMinRange = -30f;
    readonly float zMaxRange = -25f;

    // -- Specialty Methods -- //
    void Start()
    {
        if (TheDirector.Instance == null) { Debug.LogError("[WaveManager] TheDirector Instance not available"); }
        else { TheDirector.Instance.OnGameStateChanged += OnGameStateChange; }        
    }
    private void OnDisable()
    {
        if(TheDirector.Instance == null){ Debug.Log("[WaveManager] The Director is null. Can't unsub");}
        else{ TheDirector.Instance.OnGameStateChanged -= OnGameStateChange; }
        if (isSubscribedToEnemyEvent)
        {
            Enemy.OnEnemyKilled -= CheckForEndOfWave;
            isSubscribedToEnemyEvent = false;
        }

    }

    // -- Wave Methods -- //
    private Vector3 RandomPosition()
    {
        float x = Random.Range(xMinRange, xMaxRange);
        float z = Random.Range(zMinRange, zMaxRange);

        return new Vector3(x, 1, z);
    }
    IEnumerator SpawnWave(GameObject prefab, int numberOfEnemiesToSpawn, float delayBetweenSpawns, float delayBeforeStartingWave = 0)
    {

        if (prefab == null) { Debug.LogError("[WaveManager] Prefab is null when calling SpawnWave()"); yield break; }

        // -- Check if Subscribed to Enemy Death event -- //
        if(!isSubscribedToEnemyEvent) 
        { 
            Enemy.OnEnemyKilled += CheckForEndOfWave; 
            isSubscribedToEnemyEvent = true; 
            Debug.Log("[WaveManager] Subscribed to Enemy.OnEnemyKilled");
        }
        // -- Spawning the prefab -- //
        if(delayBeforeStartingWave > 0) yield return new WaitForSeconds(delayBeforeStartingWave);
        for (int i = 0; i < numberOfEnemiesToSpawn; i++)
        {
            yield return new WaitForSeconds(delayBetweenSpawns);            
            if (PoolManager.Instance == null) { Debug.LogError("[WaveManager] PoolManager is null when trying to SpawnWave()"); }
            else { PoolManager.Instance.GetObjectFromPool(prefab, RandomPosition(), Quaternion.identity); aliveEnemies++; }
        }
        isFinishedSpawningEnemies = true;
        numberOfEnemeis += 8;
    }
    private void CheckForEndOfWave(Enemy enemy)
    {
        aliveEnemies--;
        if (aliveEnemies == 0 && isFinishedSpawningEnemies)
        {

            TheDirector.Instance.SetGameState(TheDirector.GameState.Shop);
        }
    }

    // -- TheDirector Methods -- // ???
    private void OnGameStateChange(TheDirector.GameState state)
    {
        if (state == TheDirector.GameState.Restart)
        {
            Debug.Log("[WaveManager] Restart");
            SyncCoordinator.Instance.RestartReady();
            Debug.Log("[WaveManager] Reastart Ready");
            // rest everything relating to the pool.
        }
        else if(state == TheDirector.GameState.Wave)
        {
            isFinishedSpawningEnemies = false;
            aliveEnemies = 0;
            StartCoroutine(SpawnWave(enemyPrefab, numberOfEnemeis, spawnInterval));
        }
    }

}
