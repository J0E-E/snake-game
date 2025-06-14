using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPlacementManager : MonoBehaviour, IManager
{
    private List<Vector2Int> _allPossibleVectors = new List<Vector2Int>();
    private List<Vector2Int> _offLimitsVectors = new List<Vector2Int>();
    private List<Vector2Int> _availableVectors = new List<Vector2Int>();
    private void OnEnable()
    {
        GridAreaInitializer.OnGridAreaLoad += UpdateAllPossibleVectors;
    }

    private void OnDisable()
    {
        GridAreaInitializer.OnGridAreaLoad -= UpdateAllPossibleVectors;
    }

    private void UpdateAllPossibleVectors(BoxCollider2D gridArea)
    {
        Bounds bounds = gridArea.bounds;

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

    public List<Vector2Int> GetRandomAvailableVectors(int numberOfVectors = 1)
    {
        
        SetOffLimitsVectors();
        SetAvailableVectors();
        List<Vector2Int> availableVectors = new List<Vector2Int>();
        for (int i = 0; i < numberOfVectors; i++)
        {
            Vector2Int randomVector;
            do
            {
                randomVector = _availableVectors[Random.Range(0, _availableVectors.Count)];
            } while (availableVectors.Contains(randomVector));
            
            availableVectors.Add(randomVector);
        }

        return availableVectors;
    }
    
    private void SetOffLimitsVectors()
    {
        _offLimitsVectors.Clear();
        BoxCollider2D[] allBoxColliders = Object.FindObjectsByType<BoxCollider2D>(FindObjectsSortMode.None);

        BoxCollider2D[] filteredColliders = allBoxColliders
            .Where(boxCollider => !boxCollider.CompareTag("Grid Area") && boxCollider.gameObject.layer != LayerMask.NameToLayer("Walls"))
            .ToArray();
        foreach (var filteredCollider in filteredColliders)
        {
            Vector2Int colliderPosition = new Vector2Int(Mathf.RoundToInt(filteredCollider.transform.position.x),
                Mathf.RoundToInt(filteredCollider.transform.position.y));
            _offLimitsVectors.Add(colliderPosition);
        }
    }

    private void SetAvailableVectors()
    {
        _availableVectors = _allPossibleVectors.Where(pos => !_offLimitsVectors.Contains(pos)).ToList();
    }
}
