using System;
using UnityEngine;

public class GameScoreManager : MonoBehaviour, IManager
{
    private GameManager GameManager => ManagerLocator.Get<GameManager>();
    
    private int _gameScore = 0;
    private int _applesConsumed = 0;
    
    [SerializeField] private int pointsPerApple = 10;
    [SerializeField] private int levelMultiplier = 1;

    public static event Action<int> OnScoreChanged;
    public static event Action<int> OnAppleConsumed;
    
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
}