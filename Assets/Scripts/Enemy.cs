using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{


    private void OnEnable()
    {
        StartCoroutine(DestroyMe());
    }
    IEnumerator DestroyMe()
    {
        yield return new WaitForSeconds(.1f);
        PoolManager.instance.ReturnObjectToPool(gameObject);
    }
}

