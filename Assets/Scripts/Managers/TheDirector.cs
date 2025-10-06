using System;
using System.Collections;
using UnityEngine;

public class TheDirector : MonoBehaviour
{

    /// <summary>
    /// should probably get; private set to all instances?.
    /// </summary>
    public static TheDirector Instance { get; private set; }
    public GameState CurrentState { get; private set; }
    public event Action<GameState> OnGameStateChanged;

    void Awake()
    {
        if(Instance == null) { Instance = this; }      
        else { Debug.LogError("Multiple Directors"); Destroy(gameObject); }

        SetGameState(GameState.MainMenu);
    }

    public enum GameState
    {
        MainMenu,
        Wave,
        Shop,
        GameOver,
        Restart
    }
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
    IEnumerator Restarting(float fadeTime)
    {
        UIManager.Instance.FadeToBlack(fadeTime);
        yield return new WaitForSecondsRealtime(fadeTime);        
        SetGameState(GameState.Restart);
    }
}
