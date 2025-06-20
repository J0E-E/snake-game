using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Snake : MonoBehaviour
{
    private GameScoreManager _gameScoreManager => ManagerLocator.Get<GameScoreManager>();
    [SerializeField] private TouchManager touchManager;
    
    public static event Action<List<Transform>> OnSnakeGrow;
    public static event Action<Vector2> OnDirectionChange;
    public static event Action OnGameOver;
    public static event Action OnGameStart;

    private bool _canMove;
    private Vector2 _direction = Vector2.right;
    private Vector2 _newDirection = Vector2.right;
    private List<Transform> _segments = new List<Transform>();
    
    [SerializeField] private Transform segmentPrefab;
    [SerializeField] private Transform tailPrefab;
    [SerializeField] private int initialSize = 4;

    private void Awake()
    {
        touchManager.swipePerformed += SetDirection;
    }

    private void Start()
    {
        ResetState();
        _canMove = true;
        OnGameStart?.Invoke();
    }

    private void SetDirection(Vector2 vector)
    {
        if (vector == Vector2.up & !(_direction == Vector2.down))
        {
            _newDirection = Vector2.up;
        }
        else if (vector == Vector2.down & !(_direction == Vector2.up))
        {
            _newDirection = Vector2.down;
        }
        else if (vector == Vector2.left & !(_direction == Vector2.right))
        {
            _newDirection = Vector2.left;
        }
        else if (vector == Vector2.right & !(_direction == Vector2.left))
        {
            _newDirection = Vector2.right;
        }
    }

    private void OnMove(InputValue value)
    {
        Vector2 vector = value.Get<Vector2>();
        SetDirection(vector);
        
    }

    private void FixedUpdate()
    {
        if (!_canMove) return;
        
        _direction = _newDirection;
        OnDirectionChange?.Invoke(_direction);
        for (var i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }
        
        this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x) + _direction.x,
            Mathf.Round(this.transform.position.y) + _direction.y,
            0.0f
        );
        RotateHead();
        RotateTail(_segments[^1], _segments[^2]);
    }

    private void RotateTail(Transform tailSegment, Transform nextSegment)
    {
        Vector2 bodyDirection = nextSegment.position - tailSegment.position;
        
        var rotationAngle = bodyDirection switch
        {
            var dir when dir == Vector2.up => 0f,
            var dir when dir == Vector2.down => 180f,
            var dir when dir == Vector2.right => -90f,
            var dir when dir == Vector2.left => 90f,
            _ => 0f
        };
        
        tailSegment.transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);
    }

    private void RotateHead()
    {
        if (_direction == Vector2.up)
        {
            this.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
        else if (_direction == Vector2.down)
        {
            this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (_direction == Vector2.right)
        {
            this.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
        else if (_direction == Vector2.left)
        {
            this.transform.rotation = Quaternion.Euler(0f, 0f, 270f);
        }
    }

    private void Grow()
    {
        var segment = Instantiate(this.segmentPrefab);
        segment.position = _segments[^1].position;
        
        _segments.Insert(_segments.Count -1, segment);
        RotateTail(_segments[^1], _segments[^3]);
        OnSnakeGrow?.Invoke(_segments);
    }

    private void ResetState()
    {
        for (var i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }
        
        _segments.Clear();
        this.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        _segments.Add(this.transform);
        
        for (var i = 1; i < initialSize - 1; i++)
        {
            var newSegment = Instantiate(this.segmentPrefab);
            newSegment.position = _segments[^1].position + Vector3.left;
            _segments.Add(newSegment);
        }
        
        var tailSegment = Instantiate(this.tailPrefab);
        tailSegment.position = _segments[^1].position + Vector3.left;
        _segments.Add(tailSegment);
        RotateTail(_segments[^1], _segments[^2]);
        OnSnakeGrow?.Invoke(_segments);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            _gameScoreManager.ScoreApple();
            Grow();
        }

        if (other.CompareTag("Obstacle"))
        {
            _canMove = false;
            OnGameOver?.Invoke();
        }
    }

    public Vector2 Direction => _direction;
}