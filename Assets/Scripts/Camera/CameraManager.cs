using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    [Header("Pan")]
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float panSpeed = 1f; // keep ~1, not 5
    [SerializeField] private BoxCollider2D cameraBounds;

    [Header("Zoom")]
    [SerializeField] private CinemachineCamera vcam;
    [SerializeField] private float zoomValue = 5f;
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 20f;

    private Vector2 _targetPosition;
    private float _targetZoom;

    private Vector2 _lastMousePos;
    private bool _panning;

    void Start()
    {
        _targetZoom = vcam.Lens.OrthographicSize;
        _targetPosition = cameraTarget.position;
    }

    void Update()
    {
        // Start pan
        if (ServiceLocator.Instance.InputManager.RightClickPressed)
        {
            _panning = true;
            _lastMousePos = Input.mousePosition;
            _targetPosition = cameraTarget.position;
        }

        // Pan while held (PIXEL delta -> WORLD delta)
        if (_panning && ServiceLocator.Instance.InputManager.RightClickHeld)
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 pixelDelta = mousePos - _lastMousePos;

            // world units per pixel (orthographic)
            float ortho = vcam.Lens.OrthographicSize;
            float worldPerPixel = (2f * ortho) / Screen.height;

            Vector2 worldDelta = new Vector2(-pixelDelta.x, -pixelDelta.y) * worldPerPixel * panSpeed;

            _targetPosition += worldDelta;
            _lastMousePos = mousePos;
        }

        // Stop pan
        if (_panning && !ServiceLocator.Instance.InputManager.RightClickHeld)
            _panning = false;

        // Zoom
        _targetZoom -= ServiceLocator.Instance.InputManager.ScrollValue * zoomValue;
        _targetZoom = Mathf.Clamp(_targetZoom, minZoom, maxZoom);
    }

    void LateUpdate()
    {
        // Now clamp using the *new* size
        ClampTargetToBounds();

        // Apply target
        cameraTarget.position = _targetPosition;
        
        
        // Apply zoom first
        vcam.Lens.OrthographicSize =
            Mathf.Lerp(vcam.Lens.OrthographicSize, _targetZoom, Time.deltaTime * zoomSpeed);
    }


    private void ClampTargetToBounds()
    {
        if (!cameraBounds) return;

        Bounds b = cameraBounds.bounds;

        float ortho = _targetZoom;

        // IMPORTANT: use the real output camera aspect (not vcam.Lens.Aspect)
        float aspect = Camera.main.aspect;

        float halfH = ortho;
        float halfW = ortho * aspect;

        float minX = b.min.x + halfW;
        float maxX = b.max.x - halfW;
        float minY = b.min.y + halfH;
        float maxY = b.max.y - halfH;

        // If zoomed out too far for bounds, lock to center per axis
        if (minX > maxX) _targetPosition.x = b.center.x;
        else _targetPosition.x = Mathf.Clamp(_targetPosition.x, minX, maxX);

        if (minY > maxY) _targetPosition.y = b.center.y;
        else _targetPosition.y = Mathf.Clamp(_targetPosition.y, minY, maxY);
    }
}
