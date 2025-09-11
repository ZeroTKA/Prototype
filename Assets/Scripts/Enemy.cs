using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int maxHealth;
    public int Health { get; private set; }

    private void OnEnable()
    {
        Reset();
        StartCoroutine(DestroyMe());
    }
    IEnumerator DestroyMe()
    {
        yield return new WaitForSeconds(.1f);
        PoolManager.instance.ReturnObjectToPool(gameObject);
    }
    private void Reset()
    {
        Health = maxHealth;
    }
}

