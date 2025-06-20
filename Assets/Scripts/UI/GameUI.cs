using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    private PlayerManager _playerManager => ManagerLocator.Get<PlayerManager>();
    private LevelManager _levelManager => ManagerLocator.Get<LevelManager>();

    private GameScoreManager _gameScoreManager => ManagerLocator.Get<GameScoreManager>();
    
    [SerializeField] private TextMeshProUGUI playerText;
    [SerializeField] private TextMeshProUGUI scoreValue;
    [SerializeField] private TextMeshProUGUI levelValue;

    private void OnEnable()
    {
        GameScoreManager.OnScoreChanged += UpdateScore;
        LevelManager.OnLevelChange += UpdateLevel;
    }

    private void OnDisable()
    {
        GameScoreManager.OnScoreChanged -= UpdateScore;
        LevelManager.OnLevelChange -= UpdateLevel;
    }

    private void UpdateScore(int newScore)
    {
        scoreValue.text = newScore.ToString();
    }

    private void UpdateLevel(int newLevel)
    {
        levelValue.text = newLevel.ToString();
    }

    private void Start()
    {
        scoreValue.text = _gameScoreManager.GetGameScore().ToString();
        levelValue.text = _levelManager.GetCurrentLevel().ToString();
        playerText.text = $"PLAYER: {_playerManager.PlayerInitials}";
    }
}
