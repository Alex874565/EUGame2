using System;
using UnityEngine;
using Object = UnityEngine.Object;

[RequireComponent(typeof(LineRenderer))]
public class PlacementManager : MonoBehaviour
{
    [field: Header("Curve Settings")]
    [field: SerializeField] public Transform CurveArrowHead { get; private set; }
    [field: SerializeField] public int CurveResolution { get; private set; } = 24;
    [field: SerializeField] public float CurveHeightRatio { get; private set; } = 2.5f;
    [field: SerializeField] public float CurveZDepth { get; private set; } = -10f;  
    [field: SerializeField] public float CurveWidth { get; private set; } = 0.5f;
    
    public LineRenderer CurveLineRenderer { get; private set; }
    
    public GameObject UnitInPlacing { get; set; }

    private void Start()
    {
        CurveLineRenderer = GetComponent<LineRenderer>();
    }

    public void ClearPlacement()
    {
        if (UnitInPlacing != null)
        {
            Destroy(UnitInPlacing);
            UnitInPlacing = null;
        }
    }
    
    public void StartPlacingUnit(UnitType unitType)
    {
        GameObject prefab = ServiceLocator.Instance.UnitsDatabase.Units.Find(u => u.Data.Type == unitType).Prefab;
        GameObject ghostUnit = Instantiate(prefab);
        UnitInPlacing = ghostUnit;
        ghostUnit.AddComponent<PlacementController>();
    }
}