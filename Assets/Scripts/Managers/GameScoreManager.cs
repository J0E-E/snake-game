using UnityEngine;

public class GameScoreManager : MonoBehaviour
{
    private GameManager GameManager => ManagerLocator.Get<GameManager>();
    
    private int _gameScore = 0;
    
    [SerializeField] private int pointsPerApple;
    [SerializeField] private int levelMultiplier = 1;

    public void ScoreApple()
    {
        _gameScore += pointsPerApple + (levelMultiplier * GameManager.GetCurrentLevel());
    }

    public int GetGameScore()
    {
        return _gameScore;
    }
}