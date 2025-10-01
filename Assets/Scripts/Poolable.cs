using UnityEngine;

public class Poolable : MonoBehaviour
{
    public int PoolIndex { get; set; }
    public void OnSpawn()
    {
        if (WallThings.instance == null) { Debug.LogError("[Poolable] WallThings null. Can't sub"); }
        else { WallThings.instance.WallIsGone += WallIsDestroyed; }

        if (gameObject.TryGetComponent<Enemy>(out var enemy)) { enemy.Reset(); }
        else { Debug.LogError("[Poolable]trying to enemy Reset but Enemy is null?"); }
    }
    public void OnDespawn()
    {
        if(WallThings.instance == null) { Debug.LogError("[Poolable] WallThings null. Can't unsub"); }
        else { WallThings.instance.WallIsGone -= WallIsDestroyed; }
    }

    private void WallIsDestroyed()
    {
        if(gameObject.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.WallIsDestroyed();
        }
        else { Debug.LogError("[Poolable] wall destroyed but Enemy is null and can't call WallIsDestroyed()?"); }
    }
}
