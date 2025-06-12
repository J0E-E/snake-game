using System;
using TMPro;
using UnityEngine;

public class HighScoreDisplay : MonoBehaviour
{
    private HighScoreManager HighScoreManager => ManagerLocator.Get<HighScoreManager>();
    [SerializeField] private Canvas topScoredGrid;
    [SerializeField] private GameObject highScoreTextPrefab;

    private void Start()
    {
        GetTopScores();
    }

    private void GetTopScores()
    {
        HighScoreManager.GetTopScores((topScoresList) =>
        {   
            foreach (HighScoreManager.TopScore score in topScoresList.scores)
            {
                GameObject topScoreText = Instantiate(highScoreTextPrefab, topScoredGrid.transform);
                TextMeshProUGUI textComponent = topScoreText.GetComponent<TextMeshProUGUI>();
                textComponent.text = $"{score.player.ToUpper()} {score.score} {score.date.Split('T')[0]}";
            }
        });
    }
}
