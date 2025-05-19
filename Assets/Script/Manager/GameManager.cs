using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameState
{
    LevelCompleted,
    GameOver,
    Pause,
    GameWin,
    Playing,
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("Events")]
    public static Action<GameState> onGameStateChanged;
    [Header("Elements")]
    private GameState gameState;
    void Awake()
    {
        if (instance != null) Destroy(instance);
        else instance = this;
    }
    public void SetGameState(GameState gameState)
    {
        this.gameState = gameState;
        onGameStateChanged?.Invoke(gameState);
    }
}
