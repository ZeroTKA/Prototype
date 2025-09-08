using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PoolManager : MonoBehaviour
{
    Stack<int> enemyIndexStack = new Stack<int>();
    List<GameObject> enemiesList = new List<GameObject>();

    [SerializeField] private GameObject preLoadEnemyPrefab;

    public static PoolManager instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        // -- preload -- //
        for(int i = 0; i < 10; i++)
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
        enemyIndexStack.Push(index);
        enemy.SetActive(activate);
        var poolable = enemy.GetComponent<Poolable>();
        if (poolable != null)
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
}
