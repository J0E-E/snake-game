using System;
using UnityEngine;

public class LevelManager : MonoBehaviour, IManager
{
    private int levelTriggerCount = 5;
    private int currentLevel = 1;
    
    public static event Action<int> OnLevelChange;
    private void OnEnable()
    {
        GameScoreManager.OnAppleConsumed += AppleConsumed;
    }

    private void OnDisable()
    {
        GameScoreManager.OnAppleConsumed -= AppleConsumed;
    }

    private void Start()
    {
        OnLevelChange?.Invoke(currentLevel);
    }

    private void AppleConsumed(int numberOfApples)
    {
        if (numberOfApples % levelTriggerCount == 0)
        {
            currentLevel++;
            OnLevelChange?.Invoke(currentLevel);
        }
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }
}
