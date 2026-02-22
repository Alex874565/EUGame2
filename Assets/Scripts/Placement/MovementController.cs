using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class MovementController : MonoBehaviour
{
    
    private UnitData _unitData;

    private Vector2 _startPosition;
    private GameObject _targetObject;
    
    private UnitBehaviour _unitBehaviour;
    private Image _unitImage;
    private float _moveSpeed;
    
    private LineRenderer _ghostLine;
    private LineRenderer _mainLine;

    private List<Vector3> _bezierPoints; // world units per second
    private int _moveIndex;
    private float _moveSegmentT;
    private float _currentSegmentLength;

    private bool _canMove;
    
    private void Start()
    {
        _canMove = false;
    }
    
    private void Update()
    {
        if (_canMove)
        {
            Move();
        }
    }

    private void InitializeMovingUnit()
    {        
        _ghostLine = Instantiate(ServiceLocator.Instance.PlacementManager.GhostLineRenderer);
        _mainLine = Instantiate(ServiceLocator.Instance.PlacementManager.MainLineRenderer);

        // Behaviour
        _unitBehaviour = GetComponent<UnitBehaviour>();
        _unitData = _unitBehaviour.Data;
        _moveSpeed = _unitData.Speed;

        // Image
        _unitImage = GetComponentInChildren<Image>();
        
        // Canvas
        transform.SetParent(ServiceLocator.Instance.UIManager.MovementLayer.transform);
    }

    public void StartMovement(Vector2 start, GameObject target)
    {
        InitializeMovingUnit();

        _startPosition = start;
        _targetObject = target;
        
        Vector2 targetPos = target.transform.position;
        
        // Use world positions for the path
        _bezierPoints = BuildBezierPoints(_startPosition, targetPos);

        // Draw the final/main line if you want (or ghost line)
        DrawBezier(_ghostLine, _startPosition, targetPos);
        DrawBezier(_mainLine, _startPosition, targetPos);

        // Start at beginning of path
        _moveIndex = 0;
        _moveSegmentT = 0f;

        // Ensure the moving unit starts at the first point (world)
        // (If your unit is UI, see note below)
        transform.position = _bezierPoints[0];

        _canMove = true;
    }

    private void Move()
    {
        if (_bezierPoints == null || _bezierPoints.Count < 2)
            return;

        // If reached the end
        if (_moveIndex >= _bezierPoints.Count - 2)
        {
            ReachTarget();
            return;
        }

        Vector3 a = _bezierPoints[_moveIndex];
        Vector3 b = _bezierPoints[_moveIndex + 1];

        float segmentLen = Vector3.Distance(a, b);
        if (segmentLen < 0.0001f)
        {
            _moveIndex++;
            _moveSegmentT = 0f;
            return;
        }

        // advance along this segment by speed
        float dt = Time.deltaTime;
        float moveDist = _moveSpeed * dt;

        // convert distance to t step on this segment
        float tStep = moveDist / segmentLen;
        _moveSegmentT += tStep;

        while (_moveSegmentT >= 1f && _moveIndex < _bezierPoints.Count - 2)
        {
            _moveSegmentT -= 1f;
            _moveIndex++;

            a = _bezierPoints[_moveIndex];
            b = _bezierPoints[_moveIndex + 1];
            segmentLen = Vector3.Distance(a, b);
            if (segmentLen < 0.0001f) break;

            // If we still have leftover distance this frame, loop continues naturally
        }

        Vector3 pos = Vector3.Lerp(a, b, Mathf.Clamp01(_moveSegmentT));

        // If this unit is a WORLD object:
        transform.position = pos;
        
        // If you want rotation along path (2D):
        Vector2 tangent = (b - a);
        if (tangent.sqrMagnitude > 0.000001f)
        {
            float angle = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg;

            // sprite points LEFT by default, so add 180
            float offset = 180f;

            _unitImage.rectTransform.localRotation = Quaternion.Euler(0f, 0f, angle + offset);
        }
        
        _mainLine.positionCount = Mathf.Clamp(_moveIndex + 2, 0, _bezierPoints.Count);
        for (int i = 1; i < _mainLine.positionCount; i++)
        {
            _mainLine.SetPosition(i, _bezierPoints[i]);
        }
    }
    
    private void ReachTarget()
    {
        int unitsCount = _unitBehaviour.Count;
        if (_targetObject != null)
        {
            EmergencyBehaviour emergency = _targetObject.GetComponent<EmergencyBehaviour>();
            emergency.SetActiveUnits(_unitData.Type, emergency.GetActiveUnitsOfType(_unitData.Type) + unitsCount);
            emergency.SetIncomingUnits(_unitData.Type, emergency.GetIncomingUnitsOfType(_unitData.Type) - unitsCount);
        }
        else
        {
            int currentCount = ServiceLocator.Instance.UnitsManager.InventoryUnits.Find(unit => unit.GetComponent<UnitBehaviour>().Type == _unitData.Type).GetComponent<UnitBehaviour>().Count;
            ServiceLocator.Instance.UnitsManager.SetInventoryUnitCount(_unitData.Type, currentCount + unitsCount);
        }

        DeleteLines();
        Destroy(gameObject);
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
}