using System;
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
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Debug.Log($"We are here");
        SetGameState(GameState.MainMenu);
    }

    public enum GameState
    {
        MainMenu,
        Wave,
        Shop,
        GameOver
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
}
