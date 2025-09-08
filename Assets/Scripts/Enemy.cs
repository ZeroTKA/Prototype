using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private void OnEnable()
    {
        //StartCoroutine(DestroyMe());
    }
    IEnumerator DestroyMe()
    {
        yield return new WaitForSeconds(.5f);
        GameObject.Destroy(gameObject);
    }
}
