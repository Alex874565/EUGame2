using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlacementController : MonoBehaviour
{
    private UnitData _unitData;
    public Vector2 StartPosition { get; set; }
    
    private UnitBehaviour _unitBehaviour;
    
    private void Start()
    {
        StartPosition = ServiceLocator.Instance.LocationsDatabase.Locations.First(l => l.Name == LocationName.Test)
            .Coordinates;
        
        InitializeGhostUnit();
    }
    
    private void Update()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.position = Input.mousePosition;

        if (!ServiceLocator.Instance.CursorManager.IsHoveringUI())
        {
            DrawBezier(StartPosition, mouseWorldPos);
            if (ServiceLocator.Instance.InputManager.LeftClickReleased)
            {
                if (CanPlace())
                {
                
                }
            }
        }
        else
        {
            if (ServiceLocator.Instance.CursorManager.HoveredObject != null)
            {
                EmergencyBehaviour emergency = ServiceLocator.Instance.CursorManager.HoveredObject.GetComponent<EmergencyBehaviour>();
                if (emergency != null && !emergency.HasAllUnitsOfType(_unitData.Type) && StartPosition != (Vector2)emergency.transform.position)
                {
                    DrawBezier(StartPosition, emergency.transform.position);
                }
                else
                {
                    ServiceLocator.Instance.PlacementManager.CurveLineRenderer.positionCount = 0;
                }
            }
            else
            {
                ServiceLocator.Instance.PlacementManager.CurveLineRenderer.positionCount = 0;
            }
        }
        
        if (ServiceLocator.Instance.InputManager.RightClickPressed)
        {
            GameObject inventoryUnit = ServiceLocator.Instance.UnitsManager.InventoryUnits.First(unit => unit.GetComponent<UnitBehaviour>().Type == _unitData.Type);
            inventoryUnit.GetComponent<UnitBehaviour>().UpdateCount(inventoryUnit.GetComponent<UnitBehaviour>().Count + 1);
            if (_unitBehaviour.Count > 1)
            {
                _unitBehaviour.UpdateCount(_unitBehaviour.Count - 1);
            }
            else
            {
                ServiceLocator.Instance.PlacementManager.UnitInPlacing = null;
                ServiceLocator.Instance.PlacementManager.CurveLineRenderer.positionCount = 0;
                Destroy(gameObject);
            }
        }
    }

    private void InitializeGhostUnit()
    {
        
        _unitBehaviour = GetComponent<UnitBehaviour>();
        _unitData = _unitBehaviour.Data;
        _unitBehaviour.UpdateCount(1);
        transform.SetParent(ServiceLocator.Instance.UIManager.DragLayer.transform);
        GetComponent<Collider2D>().enabled = false; // Disable collider to prevent interaction during placement
        // Make the object semi-transparent to indicate it's being placed
        Image image = gameObject.GetComponentInChildren<Image>();
        if (image != null)
        {
            image.raycastTarget = false;
            Color color = image.color;
            color.a = 0.75f; // Set alpha to 50%
            image.color = color;
        }
    }

    public bool CanPlace()
    {
        GameObject hoveredInteractable = ServiceLocator.Instance.CursorManager.HoveredObject;
        if (hoveredInteractable != null)
        {
            EmergencyBehaviour emergency = hoveredInteractable.gameObject.GetComponentInChildren<EmergencyBehaviour>();
            if (emergency != null)
            {
                return true;
            }
        }
        
        return false;
    }

    private void StartMoving()
    {
        
    }
    
    #region Bezier Curve
    
    private void DrawBezier(Vector3 start, Vector3 end)
    {
        PlacementManager placementManager = ServiceLocator.Instance.PlacementManager;
        LineRenderer line = placementManager.CurveLineRenderer;
        Transform arrowHead = placementManager.CurveArrowHead;
        float curveHeight = placementManager.CurveHeightRatio * Camera.main.orthographicSize; // Scale height with camera size for consistent appearance
        int resolution = placementManager.CurveResolution;
        float zDepth = placementManager.CurveZDepth;
        
        line.startWidth = placementManager.CurveWidth;
        line.endWidth = placementManager.CurveWidth;
        
        start.z = zDepth;
        end.z = zDepth;

        // Control point: midpoint pushed upward in world Y
        Vector3 mid = (start + end) * 0.5f;
        Vector3 control = mid + Vector3.up * curveHeight;
        control.z = zDepth;

        line.positionCount = resolution;

        for (int i = 0; i < resolution; i++)
        {
            float t = i / (float)(resolution - 1);
            Vector3 p = BezierQuadratic(start, control, end, t);
            line.SetPosition(i, p);
        }

        // Optional arrow head facing tangent direction
        if (arrowHead)
        {
            arrowHead.position = end;

            Vector3 tangent = BezierQuadraticTangent(start, control, end, 1f);
            if (tangent.sqrMagnitude > 0.0001f)
                arrowHead.rotation = Quaternion.LookRotation(Vector3.forward, tangent); // 2D: Z-forward
        }
    }

    private Vector3 BezierQuadratic(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1f - t;
        return u * u * p0 + 2f * u * t * p1 + t * t * p2;
    }

    private Vector3 BezierQuadraticTangent(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        // derivative of quadratic bezier
        return 2f * (1f - t) * (p1 - p0) + 2f * t * (p2 - p1);
    }
    
    #endregion
}