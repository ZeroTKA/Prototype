using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int maxHealth;
    [SerializeField] float wallStopZ;
    public int Health { get; private set; }

    bool isAtWall = false;

    private float moveSpeed = 4f;

    private void OnEnable()
    {
        Reset();
        StartCoroutine(WalkTowardWall());
    }

    // -- Main Methods -- //
    private void Reset()
    {
        Health = maxHealth;
        isAtWall = false;
        WallThings.instance.WallIsGone += WallIsDestroyed;
    }
    public void TakeDamage(int damageAmount)
    {
        // -- Taking Damage Logic -- //
        if (damageAmount < 0)
        {
            Debug.LogError($"{damageAmount} is trying to give damage to {gameObject.name}. damage must be positive");
        }
        else if (Health - damageAmount > 0) Health -= damageAmount;
        else PoolManager.Instance.ReturnObjectToPool(gameObject);
        WallThings.instance.WallIsGone -= WallIsDestroyed;
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
        while(true)
        {
            yield return new WaitForSeconds(3);
            WallThings.instance.ChangeHealth(-3);
            Debug.Log("ATTACKED");
        }

    }
    private void WallIsDestroyed()
    {
        StopAllCoroutines();
    }
    
    IEnumerator Death()
    {
        yield return new WaitForSeconds(4);
        PoolManager.Instance.ReturnObjectToPool(gameObject);
    }
}

