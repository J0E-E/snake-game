using System;
using UnityEngine;

public class GameScoreManager : MonoBehaviour, IManager
{
    private GameManager GameManager => ManagerLocator.Get<GameManager>();
    
    private int _gameScore;
    private int _applesConsumed;
    
    [SerializeField] private int pointsPerApple = 10;
    [SerializeField] private int levelMultiplier = 1;

    public static event Action<int> OnScoreChanged;
    public static event Action<int> OnAppleConsumed;

    private void OnEnable()
    {
        Snake.OnGameStart += OnGameStart;
    }
    
    private void OnDisable()
    {
        Snake.OnGameStart -= OnGameStart;
    }

    public void ScoreApple()
    {
        _applesConsumed++;
        _gameScore += pointsPerApple + (levelMultiplier * GameManager.GetCurrentLevel());
        OnScoreChanged?.Invoke(_gameScore);
        OnAppleConsumed?.Invoke(_applesConsumed);
    }

    public int GetGameScore()
    {
        return _gameScore;
    }

    private void OnGameStart()
    {
        _gameScore = 0;
        _applesConsumed = 0;
    }
}