using System;
using UnityEngine;

public class WallThings : MonoBehaviour
{
    [SerializeField] int startingHealth;
    public int Health {  get; private set; }
    
    private int maxHealth;
    public event Action WallIsGone;
    public static WallThings instance;

    float timer = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // -- Subscribe -- //
        if(TheDirector.Instance == null)
        {
            Debug.LogError("[WallThings] TheDirector is null.");
        }
        else { TheDirector.Instance.OnGameStateChanged += WallRestart; }
        ResetHealth();
            

    }
    private void OnDisable()
    {
        if (TheDirector.Instance == null)
        {
            Debug.LogWarning("[WallThings] TheDirector is null and we can't unsubscribe. That's probably bad.");
        }
        else { TheDirector.Instance.OnGameStateChanged -= WallRestart; }
    }

    private void WallRestart(TheDirector.GameState state)
    {
        if(state == TheDirector.GameState.Restart)
        {
            Debug.Log("[WallThings] Restart");
            ResetHealth();
            SyncCoordinator.Instance.RestartReady();
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    public void ChangeHealth(int healthChangeAmount)
    {
        Health += healthChangeAmount;
        if (Health <= 0)
        {
            if(WallIsGone == null)
            {
                Debug.LogWarning("[WallThings] Nothing was subscribed to WallIsGone when health reached 0. " +
                    "How did we do that? Is that a problem?");
            }
            WallIsGone?.Invoke();
            if(TheDirector.Instance == null)
            {
                Debug.LogError("[WallThings] TheDirector is null. Can't change gamestate");
            }
            else
            {
                TheDirector.Instance.SetGameState(TheDirector.GameState.GameOver);
            }                
        }
        if (timer >= .2f)
        {
            if(UIManager.Instance == null)
            {
                Debug.LogError("[WallThings] UIManager is null. Can't change health");
            }
            else
            {
                UIManager.Instance.ChangeWallHealth(Health, maxHealth);
                timer = 0;
            }

        }
    }
    private void ResetHealth()
    {
        maxHealth = startingHealth;
        if (maxHealth <= 0)
        {
            Debug.LogError("[WallThings] maxHealth isn't a positive number. What's up with that?");
        }
        Health = maxHealth;
        if (UIManager.Instance == null)
        {
            Debug.LogError("[WallThings] UIManager is null. That's not good.");
        }
        else
        {
            UIManager.Instance.ChangeWallHealth(Health, startingHealth);
        }
    }

}
