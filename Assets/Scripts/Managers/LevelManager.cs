using System;
using UnityEngine;

public class LevelManager : MonoBehaviour, IManager
{
   [SerializeField] private int applesPerLevel = 5;
    private int _currentLevel = 1;
    
    public static event Action<int> OnLevelChange;
    private void OnEnable()
    {
        GameScoreManager.OnAppleConsumed += AppleConsumed;
        Snake.OnGameStart += OnGameStart;
    }

    private void OnDisable()
    {
        GameScoreManager.OnAppleConsumed -= AppleConsumed;
        Snake.OnGameStart -= OnGameStart;
    }

    private void Start()
    {
        OnLevelChange?.Invoke(_currentLevel);
    }

    private void AppleConsumed(int numberOfApples)
    {
        if (!IsLevelUp(numberOfApples) || numberOfApples == 0) return;
        
        _currentLevel++;
        OnLevelChange?.Invoke(_currentLevel);
        
    }

    public int GetCurrentLevel()
    {
        return _currentLevel;
    }

    private void OnGameStart()
    {
        _currentLevel = 1;
        OnLevelChange?.Invoke(_currentLevel);
    }

    public bool IsLevelUp(int numberOfApples)
    {
        return numberOfApples % applesPerLevel == 0;
    }
}
