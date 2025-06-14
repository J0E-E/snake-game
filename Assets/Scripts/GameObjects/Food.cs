using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Food : MonoBehaviour
{
    public BoxCollider2D gridArea;
    
    private LevelManager _levelManager => ManagerLocator.Get<LevelManager>();
    private ObjectPlacementManager _objectPlacementManager => ManagerLocator.Get<ObjectPlacementManager>();

    private void OnEnable()
    {
        Snake.OnGameOver += OnGameOver;
        Snake.OnGameStart += OnGameStart;
        GameScoreManager.OnAppleConsumed += AppleConsumed;
        ObstacleManager.OnObstaclesPlaced += ObstaclesPlaced;
    }

    private void OnDisable()
    {
        Snake.OnGameOver -= OnGameOver;
        Snake.OnGameStart -= OnGameStart;
        GameScoreManager.OnAppleConsumed -= AppleConsumed;
        ObstacleManager.OnObstaclesPlaced -= ObstaclesPlaced;
    }

    private void AppleConsumed(int totalApples)
    {
        if (_levelManager.IsLevelUp(totalApples)) return;
        
        RandomizePosition();
    }

    private void ObstaclesPlaced()
    {
        RandomizePosition();
    }

    public void RandomizePosition()
    {
        Bounds bounds = gridArea.bounds;

        Vector2Int newPosition = _objectPlacementManager.GetRandomAvailableVectors()[0];

        transform.position = new Vector3(newPosition.x, newPosition.y, 0.0f);
    }

    private void OnGameOver()
    {
        Destroy(this.gameObject);
    }

    private void OnGameStart()
    {
        RandomizePosition();
    }
}