using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class ObstacleManager : MonoBehaviour, IManager
{
    private ObjectPlacementManager _objectPlacementManager => ManagerLocator.Get<ObjectPlacementManager>();
    
    private List<Transform> _obstacles = new List<Transform>();
    private int _currentLevel;
    
    [SerializeField] private Transform obstaclePrefab;
    [SerializeField] private int obstaclesPerLevel = 4;
    public static event Action OnObstaclesPlaced;
        

    private void OnEnable()
    {
        Snake.OnGameOver += OnGameOver;
        LevelManager.OnLevelChange += LevelChanged;
    }

    private void OnDisable()
    {
        Snake.OnGameOver -= OnGameOver;
        LevelManager.OnLevelChange -= LevelChanged;
    }

    private void LevelChanged(int level)
    {
        _currentLevel = level;
        if (_currentLevel <= 1)
            return;
        AddObstacles();
    }

    private void AddObstacles()
    {

        List<Vector2Int> obstacleVectors = _objectPlacementManager.GetRandomAvailableVectors(obstaclesPerLevel);

        foreach (Vector2Int obstacleVector in obstacleVectors)
        {
            Transform obstacle = Instantiate(obstaclePrefab);
            obstacle.transform.position = new Vector3(obstacleVector.x, obstacleVector.y, 0.0f);
            _obstacles.Add(obstacle);
        }
        OnObstaclesPlaced?.Invoke();
    }

    private void OnGameOver()
    {
        foreach (var obstacle in _obstacles)
        {
            Destroy(obstacle.gameObject);
        }
        _obstacles.Clear();
    }
}