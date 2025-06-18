using System;
using UnityEngine;

public class GameScoreManager : MonoBehaviour, IManager
{
    private LevelManager _levelManager => ManagerLocator.Get<LevelManager>();
    
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
        _gameScore += pointsPerApple + (levelMultiplier * _levelManager.GetCurrentLevel());
        OnScoreChanged?.Invoke(_gameScore);
        OnAppleConsumed?.Invoke(_applesConsumed);
    }

    private void OnGameStart()
    {
        _gameScore = 0;
        _applesConsumed = 0;
    }
}