using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class PlacementController : MonoBehaviour
{
    private UnitData _unitData;
    public Vector2 StartPosition { get; set; }
    
    private UnitBehaviour _unitBehaviour;
    private Image _unitImage;
    private float _moveSpeed;
    
    private LineRenderer _ghostLine;
    private LineRenderer _mainLine;
    
    private List<Vector3> _bezierPoints; // world units per second
    private int _moveIndex;
    private float _moveSegmentT;
    private float _currentSegmentLength;
    
    private void Start()
    {
        InitializeGhostUnit();
    }
    
    private void Update()
    {
        HandlePlacement();
    }
    
    
    private void HandlePlacement()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if (CanPlace() && !ServiceLocator.Instance.CursorManager.IsHoveringMenu())
        {
            GameObject targetObject = ServiceLocator.Instance.CursorManager.HoveredObject;
            LockOnEmergency(targetObject);
            if (ServiceLocator.Instance.InputManager.LeftClickPressed)
            {
                ServiceLocator.Instance.CursorManager.SelectObject(targetObject);
                int requiredUnits = targetObject.GetComponentInChildren<EmergencyBehaviour>().RequiredUnitsOfType(_unitData.Type);
                int unitsToSend = Mathf.Min(requiredUnits, _unitBehaviour.Count);
                StartCoroutine(SendUnits(targetObject, unitsToSend));
            }
        }
        else
        {
            if (!ServiceLocator.Instance.CursorManager.IsHoveringMenu())
            {
                PlaceGhost(mousePos);
            }
            else
            {
                DeleteLines();
                transform.position = mousePos;
            }
        }
        
        if (ServiceLocator.Instance.InputManager.RightClickPressed && ServiceLocator.Instance.CursorManager.IsHoveringMenu())
        {
            GiveUpUnit();
        }
    }

    private void InitializeGhostUnit()
    {
        _ghostLine = Instantiate(ServiceLocator.Instance.PlacementManager.GhostLineRenderer);
        _mainLine = Instantiate(ServiceLocator.Instance.PlacementManager.MainLineRenderer);
        
        // Behaviour
        _unitBehaviour = GetComponent<UnitBehaviour>();
        _unitData = _unitBehaviour.Data;
        _unitBehaviour.UpdateCount(1);
        
        // Canvas
        transform.SetParent(ServiceLocator.Instance.UIManager.PlacementLayer.transform);
        
        // Image
        _unitImage = GetComponentInChildren<Image>();
        _unitImage.raycastTarget = false;
        SetTransparency(.75f);
        _moveSpeed = _unitData.Speed;
    }

    private GameObject SpawnMovingUnit(int unitsToSend)
    {
        GameObject unitInMovement = ServiceLocator.Instance.UnitsManager.UnitFactory.SpawnMovingUnit(_unitData.Type);
        unitInMovement.GetComponent<UnitBehaviour>().UpdateCount(unitsToSend);
        return unitInMovement;
    }
    
    private void GiveUpUnit()
    {
        if(_unitBehaviour.OwningEmergency != null)
        {
            _unitBehaviour.OwningEmergency.SetActiveUnits(_unitData.Type,
                _unitBehaviour.OwningEmergency.GetActiveUnitsOfType(_unitData.Type));
        }
        else
        {
            GameObject inventoryUnit =
                ServiceLocator.Instance.UnitsManager.InventoryUnits.First(unit =>
                    unit.GetComponent<UnitBehaviour>().Type == _unitData.Type);
            inventoryUnit.GetComponent<UnitBehaviour>()
                .UpdateCount(inventoryUnit.GetComponent<UnitBehaviour>().Count + 1);
        }

        if (_unitBehaviour.Count > 1)
        {
            _unitBehaviour.UpdateCount(_unitBehaviour.Count - 1);
        }
        else
        {
            DeleteLines();
            ServiceLocator.Instance.PlacementManager.ClearPlacement();
        }
    }

    private IEnumerator SendUnits(GameObject target, int unitsToSend)
    {
        int currentMoney = ServiceLocator.Instance.WavesManager.CurrentMoney;
        ServiceLocator.Instance.WavesManager.UpdateMoney(currentMoney - _unitData.MovementCost * unitsToSend);
        if (_unitBehaviour.OwningEmergency)
        {
            _unitBehaviour.OwningEmergency.SetActiveUnits(_unitData.Type, _unitBehaviour.OwningEmergency.GetActiveUnitsOfType(_unitData.Type) - unitsToSend);
        }
        
        EmergencyBehaviour emergency = target.GetComponent<EmergencyBehaviour>();
        emergency.SetIncomingUnits(_unitData.Type, emergency.GetIncomingUnitsOfType(_unitData.Type) + unitsToSend);
        emergency.ReactToUnitHover(gameObject);
        GameObject unit = SpawnMovingUnit(unitsToSend);
        yield return new WaitForNextFrameUnit();
        unit.GetComponent<MovementController>().StartMovement(StartPosition, target);
        if (_unitBehaviour.Count > unitsToSend)
        {
            _unitBehaviour.UpdateCount(_unitBehaviour.Count - unitsToSend);
        }
        else
        {
            DeleteLines();
            ServiceLocator.Instance.PlacementManager.ClearPlacement();
        }
    }

    private void PlaceGhost(Vector2 mouseWorldPos)
    {
        transform.position = mouseWorldPos;
        
        SetTransparency(.75f);
        DeleteLines();
            
        DrawBezier(_ghostLine, StartPosition, mouseWorldPos);
    }
    
    private void LockOnEmergency(GameObject emergency)
    {
        Vector3 target = emergency.transform.position;
        DeleteLines();
        DrawBezier(_mainLine, StartPosition, target);
        transform.position = target;
        SetTransparency(1f);
    }
    
    #region Bezier Curve
    
    private void DrawBezier(LineRenderer line, Vector3 start, Vector3 end)
    {
        PlacementManager pm = ServiceLocator.Instance.PlacementManager;

        line.startWidth = pm.LineWidth;
        line.endWidth = pm.LineWidth;

        var pts = BuildBezierPoints(start, end);

        line.positionCount = pts.Count;
        for (int i = 0; i < pts.Count; i++)
            line.SetPosition(i, pts[i]);

        // Arrow head (if you want it to match the line end)
        Transform arrowHead = pm.MainLineArrowHead;
        if (arrowHead)
        {
            arrowHead.position = pts[^1];

            // Use last segment as direction
            Vector3 dir = pts[^1] - pts[^2];
            if (dir.sqrMagnitude > 0.0001f)
                arrowHead.rotation = Quaternion.LookRotation(Vector3.forward, dir);
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
    
    private List<Vector3> BuildBezierPoints(Vector3 start, Vector3 end)
    {
        PlacementManager pm = ServiceLocator.Instance.PlacementManager;

        float curveHeight = pm.LineHeightRatio * Camera.main.orthographicSize;
        int resolution = pm.LineResolution;
        float zDepth = pm.LineZDepth;

        start.z = zDepth;
        end.z = zDepth;

        Vector3 mid = (start + end) * 0.5f;
        Vector3 control = mid + Vector3.up * curveHeight;
        control.z = zDepth;

        var pts = new List<Vector3>(resolution);
        for (int i = 0; i < resolution; i++)
        {
            float t = i / (float)(resolution - 1);
            pts.Add(BezierQuadratic(start, control, end, t));
        }

        return pts;
    }
    
    
    public void DeleteLines()
    {
        _ghostLine.positionCount = 0;
        _mainLine.positionCount = 0;
    }
    
    #endregion
    
    #region Helpers
    
    private void SetTransparency(float alpha)
    {
        if (_unitImage != null)
        {
            Color color = _unitImage.color;
            color.a = alpha; // Set alpha to 50%
            _unitImage.color = color;
        }
    }
    
    public bool CanPlace()
    {
        GameObject hoveredInteractable = ServiceLocator.Instance.CursorManager.HoveredObject;
        if (hoveredInteractable != null)
        {
            EmergencyBehaviour emergency = hoveredInteractable.gameObject.GetComponentInChildren<EmergencyBehaviour>();
            if (emergency != null && emergency.AcceptsUnitsOfType(_unitData.Type) && StartPosition != (Vector2)emergency.transform.position)
            {
                return true;
            }
        }
        
        return false;
    }
    
    #endregion
}