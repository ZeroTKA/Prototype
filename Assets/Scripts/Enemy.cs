using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int maxHealth;
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
            yield return null;
        }
        // insert the coroutine of attacking.
    }
    private void Reset()
    {
        Health = maxHealth;
    }
}

