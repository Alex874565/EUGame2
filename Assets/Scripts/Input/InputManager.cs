using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public event EventHandler OnEscapeAction;

    public bool LeftClickPressed { get; private set; }
    public bool LeftClickHeld { get; private set; }
    public bool LeftClickReleased { get; private set; }
    public bool RightClickPressed { get; private set; }
    public bool RightClickHeld { get; private set; }
    public bool RightClickReleased { get; private set; }

    public float ScrollValue { get; private set; }
    
    private PlayerInput _playerInput;
    
    private InputAction _leftClickAction;
    private InputAction _rightClickAction;
    private InputAction _scrollAction;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

        _playerInput.actions["Escape"].performed += Escape_perfomed;
    }

    private void Escape_perfomed(InputAction.CallbackContext context)
    {
        OnEscapeAction?.Invoke(this, EventArgs.Empty);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _leftClickAction = _playerInput.actions["LeftClick"];
        _rightClickAction = _playerInput.actions["RightClick"];
        _scrollAction = _playerInput.actions["Scroll"];
    }

    // Update is called once per frame
    void Update()
    {
        LeftClickPressed = _leftClickAction.WasPressedThisFrame();
        LeftClickHeld = _leftClickAction.IsPressed();
        LeftClickReleased = _leftClickAction.WasReleasedThisFrame();
        
        RightClickPressed = _rightClickAction.WasPressedThisFrame();
        RightClickHeld = _rightClickAction.IsPressed();
        RightClickReleased = _rightClickAction.WasReleasedThisFrame();
        
        ScrollValue = _scrollAction.ReadValue<Vector2>().y;
    }
}
