using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    /// <summary>
    ///  Maybe 'GetObjectFromPool' can be renamed.
    /// </summary>


    public static PoolManager instance;
    [SerializeField] Transform miscPool; // permanent transform.

    // -- Enemy Prefab -- //
    Stack<int> enemyIndexStack = new Stack<int>();
    List<GameObject> enemiesList = new List<GameObject>();
    Transform parentTransform;
    [SerializeField] Transform enemyParentPool;
    [SerializeField] GameObject preLoadEnemyPrefab; //preload transform.
    
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        // -- preload -- //
        for(int i = 0; i < 16; i++)
        {
            CreatePooledObject(preLoadEnemyPrefab, Vector3.zero, Quaternion.identity, false);
        }
    }

    // -- Main Methods -- //
    public void ReturnObjectToPool(GameObject obj)
    {
        var poolable = obj.GetComponent<Poolable>();
        string name = obj.name.ToLower();
        obj.SetActive(false);
        switch (name[..^7]) // pull out this switch and make it a method for ease of reading??
        {
            case "enemy":
                enemyIndexStack.Push(poolable.PoolIndex);
                break;
            default:
                Debug.LogWarning($"{obj.name} missing switch case to return index. Substring: {name[..^7]}");
                break;
        }
    }
    public void CreatePooledObject(GameObject prefab, Vector3 position, Quaternion rotation, bool activate = true)
    {
        // Bool activate is nice because it allows me to preload without making a separate method.
        GameObject enemy = Instantiate(prefab, position, rotation);
        enemiesList.Add(enemy);
        int index = enemiesList.Count - 1;
        SetParentTransform(prefab.name);
        enemy.transform.SetParent(parentTransform);
        enemy.SetActive(activate);
        if (!activate) enemyIndexStack.Push(index);
        if (enemy.TryGetComponent<Poolable>(out var poolable))
        {
            poolable.PoolIndex = index;
        }
        else
        {
            Debug.LogWarning($"Enemy prefab missing Poolable component at index {index}");
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
        }
        // -- else we just create -- //
        else
        {            
            CreatePooledObject(prefab, position, rotation);
        }
    }

    // -- Supplemental Methods -- //
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
