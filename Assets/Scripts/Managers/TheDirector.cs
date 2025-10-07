using System;
using System.Collections;
using UnityEngine;

public class TheDirector : MonoBehaviour
{

    /// <summary>
    /// should probably get; private set to all instances?.
    /// </summary>

    [SerializeField] GameObject playerPrefab;
    Vector3 spawnPoint = new Vector3(6.85f, .35f, -2f);
    Vector3 spawnRotation = new Vector3(0, 180, 0);
    
    public static TheDirector Instance { get; private set; }
    public GameState CurrentState { get; private set; }
    public event Action<GameState> OnGameStateChanged;
    public enum GameState
    {
        MainMenu,
        Wave,
        Shop,
        GameOver,
        Restart
    }


    // -- Specialty Methods -- //
    void Awake()
    {
        if(Instance == null) { Instance = this; }      
        else { Debug.LogError("Multiple Directors"); Destroy(gameObject); }
        if(playerPrefab == null)
        {
            Debug.LogError("[TheDirector] Player prefab is null. Can't paly the game can we? Nerd.");
        }
        SpawnPlayer();
        SetGameState(GameState.Wave);
    }

    // -- Main Methods -- //
    public void Restart(float fadeTime)
    {
        StartCoroutine(Restarting(fadeTime));
    }
    public void SetGameState(GameState newState)
    {
        Debug.Log($"{CurrentState} to {newState}");
        if(CurrentState != newState)
        {
            CurrentState = newState;
            OnGameStateChanged?.Invoke(newState);
            Debug.Log($"{newState} state activated.");
        }
    }
    private void SpawnPlayer()
    {
        Instantiate(playerPrefab, spawnPoint, Quaternion.Euler(spawnRotation));
    }

    // -- Coroutines -- //
    IEnumerator Restarting(float fadeTime)
    {
        UIManager.Instance.FadeToBlack(fadeTime);
        yield return new WaitForSecondsRealtime(fadeTime);        
        SetGameState(GameState.Restart);
    }
}
