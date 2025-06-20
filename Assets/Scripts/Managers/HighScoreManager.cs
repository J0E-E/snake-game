using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;

public class HighScoreManager : MonoBehaviour, IManager
{
    private const string URI = "https://api.joeyshub.com/api/games/scores";
    private int _lowestTopScore = 999999999;
    private int _totalTopScores = 0;
    
    [Serializable]
    public class GetScoresBody
    {
        public string game = "snake";
        public string action = "get";
    }
    
    [Serializable]
    public class SetScoreBody
    {
        public string game = "snake";
        public string action = "set";
        public string player;
        public string date;
        public int score;

        public SetScoreBody(PlayerScore playerScore)
        {
            player = playerScore.player;
            date = playerScore.date;
            score = playerScore.score;
        }
    }
    
    [Serializable]
    public class PlayerScore
    {
        public string player;
        public int score;
        public string date;

    }

    [Serializable]
    public class TopScoresList
    {
        public List<PlayerScore> scores;
    }
    
    private string ComputeHMACSHA256(string message, string secretKey)
    {
        // Ensure consistent encoding (UTF-8)
        var keyBytes = System.Text.Encoding.UTF8.GetBytes(secretKey);
        var messageBytes = System.Text.Encoding.UTF8.GetBytes(message);

        using (var hmac = new HMACSHA256(keyBytes))
        {
            var hashBytes = hmac.ComputeHash(messageBytes);
            return Convert.ToBase64String(hashBytes); // Use Base64 encoding
        }
    }
    
    IEnumerator GetScores(Action<TopScoresList> onComplete)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(URI, JsonUtility.ToJson(new GetScoresBody()), "application/json"))
        {
            yield return webRequest.SendWebRequest();
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(String.Format("Something went wrong: {0}", webRequest.error));
                    onComplete.Invoke(null);
                    break;
                case UnityWebRequest.Result.Success:
                    try
                    {
                        // deserialize JSON response
                        string jsonResponse = webRequest.downloadHandler.text;
                        TopScoresList topScoresList = JsonUtility.FromJson<TopScoresList>(jsonResponse);
                        foreach (var scoreModel in topScoresList.scores)
                        {
                            _totalTopScores++;
                            if (scoreModel.score < _lowestTopScore)
                            {
                                _lowestTopScore = scoreModel.score;
                            }
                        }
                        onComplete.Invoke(topScoresList);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Failed to deserialize JSON: {ex.Message}");
                        onComplete.Invoke(null);
                    }
                    break;
            }
        }
    }

    IEnumerator SetScore(PlayerScore playerScore, Action onComplete)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(URI, JsonUtility.ToJson(new SetScoreBody(playerScore)), "application/json"))
        {
            
            string timestamp = DateTime.UtcNow.ToString("o");
            webRequest.SetRequestHeader("x-timestamp", timestamp);
            
            yield return webRequest.SendWebRequest();
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(String.Format("Something went wrong: {0}", webRequest.error));
                    onComplete.Invoke();
                    break;
                case UnityWebRequest.Result.Success:
                    onComplete.Invoke();
                    break;
            }
        }
    }
    
    public void GetTopScores(Action<TopScoresList> onComplete)
    {
        StartCoroutine(GetScores(onComplete));
    }

    public void SetTopScore(PlayerScore newPlayerScore, Action onComplete)
    {
        if (newPlayerScore.score <= _lowestTopScore && _totalTopScores >= 10)
        {
            onComplete.Invoke();
            return;
        }

        StartCoroutine(SetScore(newPlayerScore, onComplete));
    }
}
