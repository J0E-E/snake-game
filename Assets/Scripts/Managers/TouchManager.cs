using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class TouchManager : MonoBehaviour
{
    public static TouchManager Instance;
    
    private PlayerInput playerInput;
    private InputAction swipe;

    [SerializeField] private float swipeResistance = .2f;

    public delegate void Swipe(Vector2 direction);

    public event Swipe swipePerformed;
    
    private void Awake()
    {
        Instance = this;
        playerInput = GetComponent<PlayerInput>();
        swipe = playerInput.actions["TouchSwipe"];
    }

    private void OnTouchSwipe()
    {
        Debug.Log("HERE!");
    }

    private void OnEnable()
    {
        swipe.performed += OnSwipePerformed;
    }

    private void OnDisable()
    {
        swipe.performed -= OnSwipePerformed;
    }

    private void OnSwipePerformed(InputAction.CallbackContext context)
    {
        TouchState touchState = context.ReadValue<TouchState>();
        Vector2 delta = touchState.delta;

        if (delta.magnitude < swipeResistance) return;

        Vector2 direction = Vector2.zero;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            direction = delta.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            direction = delta.y > 0 ? Vector2.up : Vector2.down;
        }

        if (direction != Vector2.zero && swipePerformed != null)
        {
            swipePerformed(direction);
        }
    }
}
