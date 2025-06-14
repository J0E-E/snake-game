using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleManager : MonoBehaviour, IManager
{
    private List<Transform> _segments;
    private Vector2 _snakeDirection;
    private Transform _food;
    private List<Transform> _obstacles = new List<Transform>();
    private List<Vector2Int> _offLimitsVectors = new List<Vector2Int>();
    private List<Vector2Int> _allPossibleVectors = new List<Vector2Int>();

    private int _currentLevel;
    [SerializeField] private Transform obstaclePrefab;
    [SerializeField] private int obstaclesPerLevel = 4;

    private void OnEnable()
    {
        GridAreaInitializer.OnGridAreaLoad += UpdateAllPossibleVectors;
        Snake.OnSnakeGrow += UpdateSnakeLocation;
        Snake.OnDirectionChange += UpdateSnakeDirection;
        LevelManager.OnLevelChange += LevelChanged;
        Food.OnPlaceFood += FoodPlaced;
    }

    private void OnDisable()
    {
        GridAreaInitializer.OnGridAreaLoad -= UpdateAllPossibleVectors;
        Snake.OnSnakeGrow -= UpdateSnakeLocation;
        Snake.OnDirectionChange += UpdateSnakeDirection;
        LevelManager.OnLevelChange -= LevelChanged;
        Food.OnPlaceFood -= FoodPlaced;
    }

    private void UpdateSnakeLocation(List<Transform> segments)
    {
        _segments = segments;
    }

    private void UpdateSnakeDirection(Vector2 direction)
    {
        _snakeDirection = direction;
    }

    private void UpdateAllPossibleVectors(BoxCollider2D boxCollider2D)
    {
        Bounds bounds = boxCollider2D.bounds;

        int minX = Mathf.FloorToInt(bounds.min.x);
        int maxX = Mathf.CeilToInt(bounds.max.x);
        int minY = Mathf.FloorToInt(bounds.min.y);
        int maxY = Mathf.CeilToInt(bounds.max.y);

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                Vector2Int vector2 = new Vector2Int(x, y);
                _allPossibleVectors.Add(vector2);
            }
        }
    }

    private void SetObstacleBuffer()
    {
        // Create a buffer here.
    }

    private void LevelChanged(int level)
    {
        _currentLevel = level;
        if (_currentLevel <= 1)
            return;
        SetObstacleBuffer();
        SetOffLimitsLocations();
        AddObstacles();
    }

    private void FoodPlaced(Transform food)
    {
        _food = food;
    }

    private void SetOffLimitsLocations()
    {
        _offLimitsVectors.Clear();
        
        foreach (var segment in _segments)
        {
            if (segment == null) continue;
            
            Vector2Int segmentVector = new Vector2Int(
                Mathf.RoundToInt(segment.position.x),
                Mathf.RoundToInt(segment.position.y)
            );
            _offLimitsVectors.Add(segmentVector);
        }

        foreach (var obstacle in _obstacles)
        {
            if (obstacle == null) continue;
            
            Vector2Int obstacleVector = new Vector2Int(
                Mathf.RoundToInt(obstacle.position.x),
                Mathf.RoundToInt(obstacle.position.y)
            );
            _offLimitsVectors.Add(obstacleVector);
        }

        Vector2Int foodVector = new Vector2Int(
            Mathf.RoundToInt(_food.position.x),
            Mathf.RoundToInt(_food.position.y)
        );
        
        _offLimitsVectors.Add(foodVector);
    }

    private Vector2Int GetAvailableVector()
    {
        if (_allPossibleVectors.Count == 0)
        {
            Debug.LogWarning("No available positions found for obstacles.");
            return Vector2Int.zero; // TODO: Handle no obstacle positions better.
        }

        List<Vector2Int> availableVectors = _allPossibleVectors.Where(pos => !_offLimitsVectors.Contains(pos)).ToList();
        Vector2Int randomAvailableVector = availableVectors[Random.Range(0, availableVectors.Count)];
        return randomAvailableVector;
    }

    private void AddObstacles()
    {
        foreach (var offLimitsTransform in _offLimitsVectors)
        {
            Debug.Log(offLimitsTransform);
        }


        for (int i = 0; i < obstaclesPerLevel; i++)
        {
            Vector2Int obstacleVector = GetAvailableVector();
            Transform obstacle = Instantiate(obstaclePrefab);
            obstacle.transform.position = new Vector3(obstacleVector.x, obstacleVector.y, 0.0f);
            _obstacles.Add(obstacle);
        }

        Debug.Log($"{obstaclesPerLevel} obstacles added.");
    }
}