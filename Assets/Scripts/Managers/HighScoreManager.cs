using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;

public class HighScoreManager : MonoBehaviour, IManager
{
    private const string URI = "https://api.joeyshub.com/api/games/scores";
    
    [Serializable]
    public class GetScoresBody
    {
        public string game = "snake";
        public string action = "get";
    }
    
    [Serializable]
    public class TopScore
    {
        public string player;
        public int score;
        public string date;

    }

    [Serializable]
    public class TopScoresList
    {
        public List<TopScore> scores;
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
    
    public void GetTopScores(Action<TopScoresList> onComplete)
    {
        StartCoroutine(GetScores(onComplete));
    }
}
