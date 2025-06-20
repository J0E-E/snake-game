using System;
using TMPro;
using UnityEngine;

public class HighScoreDisplay : MonoBehaviour
{
    private HighScoreManager _highScoreManager => ManagerLocator.Get<HighScoreManager>();
    [SerializeField] private Canvas topScoredGrid;
    [SerializeField] private GameObject highScoreTextPrefab;

    private void Start()
    {
        GetTopScores();
    }

    private void GetTopScores()
    {
        _highScoreManager.GetTopScores((topScoresList) =>
        {   
            if (topScoresList == null) return;
            
            foreach (HighScoreManager.PlayerScore score in topScoresList.scores)
            {
                GameObject topScoreText = Instantiate(highScoreTextPrefab, topScoredGrid.transform);
                TextMeshProUGUI textComponent = topScoreText.GetComponent<TextMeshProUGUI>();
                if (textComponent)
                {
                    textComponent.text = $"{score.player.ToUpper()} {score.score} {score.date.Split('T')[0]}";
                }
            }
        });
    }
}
