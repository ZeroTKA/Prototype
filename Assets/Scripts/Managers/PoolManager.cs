using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;
    [SerializeField] Transform miscPool;

    // -- Enemy Prefab -- //
    Stack<int> enemyIndexStack = new Stack<int>();
    List<GameObject> enemiesList = new List<GameObject>();
    Transform parentTransform;
    [SerializeField] Transform enemyParentPool;
    [SerializeField] GameObject preLoadEnemyPrefab;



    
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        // -- preload -- //
        for(int i = 0; i < 15; i++)
        {
            CreatePooledObject(preLoadEnemyPrefab, Vector3.zero, Quaternion.identity, false);
        }
    }
    public void ReturnObjectToPool()
    {

    }
    public void CreatePooledObject(GameObject prefab, Vector3 position, Quaternion rotation, bool activate = true)
    {
        GameObject enemy = Instantiate(prefab, position, rotation);
        enemiesList.Add(enemy);
        int index = enemiesList.Count - 1;    
        enemy.SetActive(activate);
        if(!activate)
        {
            enemyIndexStack.Push(index);
        }
        var poolable = enemy.GetComponent<Poolable>();
        if (poolable != null)
        {
            poolable.PoolIndex = index;
        }
        else
        {
            Debug.LogWarning($"Enemy prefab missing Poolable component at index {index}");
        }
        SetParentTransform(prefab.name);
        enemy.transform.SetParent(parentTransform);
    }

    public void GetObjectFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (enemyIndexStack.Count > 0)
        {            
            int index = enemyIndexStack.Pop();
            GameObject enemy = enemiesList[index];
            enemy.transform.SetPositionAndRotation(position, rotation);
            enemy.SetActive(true);
        }
        else
        {
            // Optionally instantiate a new one if pool is empty
            CreatePooledObject(prefab, position, rotation);

        }
    }
    private void SetParentTransform(string name)
    {
        switch(name.ToLower())
        {
            case "enemy":
                parentTransform = enemyParentPool;
                break;
            default:
                parentTransform = miscPool;
                break;
        }
    }
}
