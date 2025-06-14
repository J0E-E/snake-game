using System;
using UnityEngine;

public class GridAreaInitializer : MonoBehaviour
{
    private BoxCollider2D _boxCollider2D;
    public static event Action<BoxCollider2D> OnGridAreaLoad;

    private void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        OnGridAreaLoad?.Invoke(_boxCollider2D);
    }
}
