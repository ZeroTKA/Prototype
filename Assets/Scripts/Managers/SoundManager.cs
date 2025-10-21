using UnityEngine;
using System.Reflection;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] private AudioSource m_SFXSource;
    [SerializeField] private AudioSource m_MusicSource;

    [SerializeField] AudioClip gunshots;
    [SerializeField] GameObject m_GunshotPrefab;
    [SerializeField] AudioClip[] stoneWalking;
    [SerializeField] AudioClip[] gravelWalking;
    [SerializeField] AudioClip[] reloading;
    [SerializeField] AudioClip fireLoop;
    [SerializeField] AudioClip rainLoop;
    
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            Debug.LogWarning("Sound Manager already exists. Destroying this");
        }
    }
    private void Start()
    {
        CheckSounds();
    }

    public void PlaySound(SoundType type, Vector3 position)
    {
        AudioClip clip = null;
        switch(type)
        {
            case SoundType.Gunshot:
                PoolManager.Instance.GetObjectFromPool(m_GunshotPrefab, position, Quaternion.identity);
                break;
            case SoundType.ReloadingBegin:
                clip = reloading[0];
                break;
            case SoundType.ReloadingMid:
                clip = reloading[1];
                break;
            case SoundType.ReloadingEnd:
                clip = reloading[2];
                break;
            case SoundType.Rain:
                clip = rainLoop;
                break;
            case SoundType.Fire:
                clip = fireLoop;
                break;
            case SoundType.Gravel:
                clip = gravelWalking[Random.Range(0, gravelWalking.Length)];
                break;
            case SoundType.Stone:
                clip = stoneWalking[Random.Range(0,stoneWalking.Length)];
                break;
        }
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, position);
        }
    }
    private void CheckSounds()
    {
        var fields = typeof(SoundManager).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        foreach (var field in fields)
        {
            // -- Single clips -- //
            if (field.FieldType == typeof(AudioClip))
            {
                var clip = field.GetValue(this) as AudioClip;
                if (clip == null)
                {
                    Debug.LogWarning($"[SoundManager] '{field.Name}' is null.");
                }
            }

            // -- Audio clip lists -- //
            else if (field.FieldType == typeof(AudioClip[]))
            {
                var clips = field.GetValue(this) as AudioClip[];
                if (clips != null)
                {
                    for (int i = 0; i < clips.Length; i++)
                    {
                        if (clips[i] == null)
                        {
                            Debug.LogWarning($"[SoundManager] '{field.Name}' is null at index {i}.");
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"[SoundManager] AudioClip array '{field.Name}' is null.");
                }
            }
        }

    }
    public enum SoundType
    {
        Gunshot,
        ReloadingBegin,
        ReloadingMid,
        ReloadingEnd,
        Rain,
        Fire,
        Gravel,
        Stone
    }
}
