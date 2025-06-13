using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    private PlayerManager PlayerManager => ManagerLocator.Get<PlayerManager>();
    private LevelManager LevelManager => ManagerLocator.Get<LevelManager>();
    
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
        levelValue.text = LevelManager.GetCurrentLevel().ToString();
        playerText.text = $"PLAYER: {PlayerManager.PlayerInitials}";
    }
}
