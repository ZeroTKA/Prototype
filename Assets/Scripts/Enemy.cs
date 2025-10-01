using System.Collections;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    [SerializeField] int maxHealth;
    [SerializeField] float wallStopZ;
    public int Health { get; private set; }

    bool isAtWall = false;

    private readonly float moveSpeed = 4f;

    // -- Main Methods -- //
    public void Reset()
    {
        StopAllCoroutines();
        Health = maxHealth;
        isAtWall = false;
        StartCoroutine(WalkTowardWall());
    }
    public void TakeDamage(int damageAmount)
    {
        // -- Taking Damage Logic -- //
        if (damageAmount < 0)
        {
            Debug.LogError($"[Enemy] {damageAmount} is what's trying to damage this enemy. Damage must be positive");
        }
        else if (Health - damageAmount > 0) Health -= damageAmount;
        else
        {
            if (PoolManager.Instance == null)
            {
                Debug.LogError("[Enemy] PoolManager is null. Can't return object.");
            }
            else { PoolManager.Instance.ReturnObjectToPool(gameObject); }
        }
    }

    // -- Coroutines -- //
    IEnumerator WalkTowardWall()
    {
        while(!isAtWall)
        {
            gameObject.transform.position += new Vector3(0, 0, moveSpeed * Time.deltaTime);
            if(gameObject.transform.position.z > wallStopZ)
            {
                isAtWall = true;
                transform.position = new Vector3(transform.position.x, transform.position.y, wallStopZ);
            }
            yield return null;
        }
        StartCoroutine(Attack());
    }
    IEnumerator Attack()
    {
        if (WallThings.instance == null)
        { 
            Debug.LogError("[Enemy] WallThings.instance is empty.Stopping Attack Coroutine"); yield break; 
        }
        while (true)
        {
            yield return new WaitForSeconds(3);
            WallThings.instance.ChangeHealth(-3);
        }

    }
    public void WallIsDestroyed()
    {
        StopAllCoroutines();
    }
    
    IEnumerator Death()
    {
        yield return new WaitForSeconds(4);
        PoolManager.Instance.ReturnObjectToPool(gameObject);
    }
}

