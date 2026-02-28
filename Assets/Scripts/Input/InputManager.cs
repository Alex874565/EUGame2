using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public event EventHandler OnEscapeAction;
    public event EventHandler OnLeftClickActionFirst;
    public event EventHandler OnLeftClickActionSecond;

    public bool LeftClickPressed { get; private set; }
    public bool LeftClickHeld { get; private set; }
    public bool LeftClickReleased { get; private set; }
    public bool RightClickPressed { get; private set; }
    public bool RightClickHeld { get; private set; }
    public bool RightClickReleased { get; private set; }
    public float ScrollValue { get; private set; }

    private PlayerInput _playerInput;

    private InputAction _escapeAction;
    private InputAction _leftClickAction;
    private InputAction _rightClickAction;
    private InputAction _scrollAction;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

        // Assign actions here (Awake is fine)
        _escapeAction = _playerInput.actions["Escape"];
        _leftClickAction = _playerInput.actions["LeftClick"];
        _rightClickAction = _playerInput.actions["RightClick"];
        _scrollAction = _playerInput.actions["Scroll"];
    }

    private void OnEnable()
    {
        _escapeAction.performed += Escape_performed;
        _leftClickAction.performed += LeftClick_performed;
    }

    private void OnDisable()
    {
        _escapeAction.performed -= Escape_performed;
        _leftClickAction.performed -= LeftClick_performed;
    }

    private void Escape_performed(InputAction.CallbackContext context)
    {
        OnEscapeAction?.Invoke(this, EventArgs.Empty);
    }

    private void LeftClick_performed(InputAction.CallbackContext context)
    {
        OnLeftClickActionFirst?.Invoke(this, EventArgs.Empty);
        OnLeftClickActionSecond?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
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