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
    public void Restart()
    {
        Debug.Log("The Director Restart");
        SetGameState(GameState.Restart);
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
