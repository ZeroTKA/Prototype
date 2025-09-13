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
        StartCoroutine(Death());
        // insert the coroutine of attacking.
    }
    private void Reset()
    {
        Health = maxHealth;
        isAtWall = false;
    }
    IEnumerator Death()
    {
        yield return new WaitForSeconds(4);
        PoolManager.Instance.ReturnObjectToPool(gameObject);
    }
}

