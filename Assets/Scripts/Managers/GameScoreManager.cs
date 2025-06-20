using System;
using UnityEngine;

public class GameScoreManager : MonoBehaviour, IManager
{
    private LevelManager _levelManager => ManagerLocator.Get<LevelManager>();
    private PlayerManager _playerManager => ManagerLocator.Get<PlayerManager>();
    private HighScoreManager _highScoreManager => ManagerLocator.Get<HighScoreManager>();
    
    private int _gameScore;
    private int _applesConsumed;
    
    [SerializeField] private int pointsPerApple = 10;
    [SerializeField] private int levelMultiplier = 1;

    public static event Action<int> OnScoreChanged;
    public static event Action<int> OnAppleConsumed;

    private void OnEnable()
    {
        Snake.OnGameStart += OnGameStart;
        Snake.OnGameOver += OnGameOver;
    }
    
    private void OnDisable()
    {
        Snake.OnGameStart -= OnGameStart;
        Snake.OnGameOver -= OnGameOver;
    }

    public void ScoreApple()
    {
        _applesConsumed++;
        _gameScore += pointsPerApple + (levelMultiplier * _levelManager.GetCurrentLevel());
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
        OnScoreChanged?.Invoke(_gameScore);
        OnAppleConsumed?.Invoke(_applesConsumed);
    }

    public HighScoreManager.PlayerScore GetPlayerScore()
    {
        HighScoreManager.PlayerScore playerScore = new HighScoreManager.PlayerScore();
        
        playerScore.score = _gameScore;
        playerScore.player = _playerManager.PlayerInitials;
        playerScore.date = DateTime.UtcNow.ToString("o");

        return playerScore;
    }

    private void OnGameOver()
    {
        _highScoreManager.SetTopScore(GetPlayerScore(), () =>
        {
            GameManager.Instance.ChangeScenes(SceneName.GameOver);
        });
    }
}