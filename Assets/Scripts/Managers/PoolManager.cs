using System.Collections.Generic;
using UnityEngine;


public class PoolManager : MonoBehaviour
{
    /// <summary>
    ///  // -- To Do -- //
    ///  What do if Null object passed to ReturnObjectToPool
    /// 
    /// 
    ///  // -- Finished -- //
    /// Maybe 'GetObjectFromPool' can be renamed.
    /// Code Review 9.25.25
    /// </summary>


    public static PoolManager Instance { get; private set; }
    [SerializeField] int preloadEnemyPoolCount;
    [SerializeField] Transform miscPool; // permanent transform.

    // -- Specifically 'Enemy' Things -- //
    Stack<int> enemyIndexStack = new Stack<int>();
    List<GameObject> enemiesList = new List<GameObject>();
    Transform parentTransform; // recycled variable so we can set the parent transform to whatever pool it needs to be.
    [SerializeField] Transform enemyParentPool;
    [SerializeField] GameObject preLoadEnemyPrefab; //preload transform.
    
    // -- Specialty Methods -- //
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else { Debug.LogError("Multiple PoolManagers"); Destroy(gameObject); }
        
    }
    private void Start()
    {
        // -- Subscribe -- //
        if (TheDirector.Instance == null) { Debug.LogError("[PoolManager] TheDirector Instance not available"); }
        else { TheDirector.Instance.OnGameStateChanged += Restart; }            

        // -- preload -- //
        for (int i = 0; i < preloadEnemyPoolCount; i++)
        {
            CreatePooledObject(preLoadEnemyPrefab, Vector3.zero, Quaternion.identity, false);
        }

    }
    private void OnDisable()
    {
        if (TheDirector.Instance == null) { Debug.Log("[PoolManager] The Director is null. Can't unsub"); }
        else { TheDirector.Instance.OnGameStateChanged -= Restart; }
    }

    // -- Main Methods -- //

    public void CreatePooledObject(GameObject prefab, Vector3 position, Quaternion rotation, bool activate = true)
    {
        // Bool activate is nice because it allows me to preload without making a separate method.
        GameObject enemy = Instantiate(prefab, position, rotation);

        // -- Prep before pushing information about the enemy to places -- //
        SetParentTransform(prefab.name); // figures out what pool to go to
        enemiesList.Add(enemy);
        int index = enemiesList.Count - 1;

        // -- Do all the things -- //
        enemy.transform.SetParent(parentTransform);
        enemy.SetActive(activate);
        if (!activate) enemyIndexStack.Push(index); // meaning it's disabled then push index becaue it's available.
        if (enemy.TryGetComponent<Poolable>(out var poolable))
        {
            // Validate index is in range
            if (index < 0 || index >= enemiesList.Count)
            {
                Debug.LogError($"[PoolManager] Invalid pool index {index} for {enemy.name}");
            }
            else
            {
                poolable.PoolIndex = index;
                if(activate)
                {
                    poolable.OnSpawn();
                }
            }
        }
        else
        {
            Debug.LogError($"Enemy prefab missing Poolable component at index {index}");
        }
    }
    public void GetObjectFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        // -- if we can return, return -- //
        if (enemyIndexStack.Count > 0)
        {            
            int index = enemyIndexStack.Pop();
            GameObject enemy = enemiesList[index];
            enemy.transform.SetPositionAndRotation(position, rotation);
            enemy.SetActive(true);
            if(enemy.TryGetComponent<Poolable>(out var poolable))
            {
                poolable.OnSpawn();
            }
            else { Debug.LogError("[PoolManager] Trying to GetObjectFromPool but poolable doesn't exist"); }


        }
        // -- else we just create -- //
        else
        {            
            CreatePooledObject(prefab, position, rotation);
        }
    }
    public void ReturnObjectToPool(GameObject obj)
    {

        // -- Error checking and skipping inactive objects. This is mostly for the restart sequence.
        if (obj == null)
        {
            Debug.LogError("You passed in a null gameobject to the ReturnObjectToPool method.");
            return;
            // what data is even useful ehre to help debug that? Also I want to return, right?I always forget.
        }
        else if (!obj.activeSelf)
        {
            return;
        }

        // -- Now we just push things back to where they need to be. -- //
        string name = obj.name.ToLower();
        obj.SetActive(false);
        switch (name[..^7]) // pull out this switch and make it a method for ease of reading??
                            // suggested to get rid of name[..^7] and use name.Replace("(Clone)", "").ToLower().
        {
            case "enemy":
                if(obj.TryGetComponent<Poolable>(out var poolable))
                {
                    enemyIndexStack.Push(poolable.PoolIndex);
                    poolable.OnDespawn();
                }

                break;
            default:
                Debug.LogWarning($"{obj.name} missing switch case to return index. Substring: {name[..^7]}");
                break;
        }
    }

    // -- Supplemental Methods -- //
    private void Restart(TheDirector.GameState state)
    {
        // I really just want to reuse an existing method. This seemed the easiest way to grab everything quickly.
        // Later if I want to do this in batches or something, we have more control over WHEN the objects get disabled.

        if (state == TheDirector.GameState.Restart)
        {
            for (int i = 0; i < enemiesList.Count; i++)
            {
                ReturnObjectToPool(enemiesList[i]);
            }
            Debug.Log("[PoolManager] Restart");
            SyncCoordinator.Instance.RestartReady();
            Debug.Log("[PoolManager] Restart Ready");
            // rest everything relating to the pool.
        }
    }
    private void SetParentTransform(string name)
    {
        // trying to be scale-able and maybe there's a better way. Unsure.
        switch(name.ToLower())
        {
            case "enemy":
                parentTransform = enemyParentPool;
                break;
            default:
                Debug.LogWarning($"{name.ToLower()} doesn't have a parent transform. Does it need it?");
                parentTransform = miscPool;
                break;
        }
    }


    
}
