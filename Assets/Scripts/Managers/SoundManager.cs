using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] private AudioSource m_SFXSource;
    [SerializeField] private AudioSource m_MusicSource;

    [SerializeField] AudioClip gunshots;
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
        
    void Update()
    {
        
    }
}
