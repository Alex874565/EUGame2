using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class PlacementManager : MonoBehaviour
{
    [field: Header("Normal Line Settings")]
    [field: SerializeField] public LineRenderer MainLineRenderer { get; private set; }
    [field: SerializeField] public Transform MainLineArrowHead { get; private set; }
    [field: SerializeField] public LineRenderer GhostLineRenderer { get; private set; }
    [field: SerializeField] public Transform GhostLineArrowHead { get; private set; }
    
    [field: Header("Line Settings")]
    [field: SerializeField] public int LineResolution { get; private set; } = 24;
    [field: SerializeField] public float LineHeightRatio { get; private set; } = 2.5f;
    [field: SerializeField] public float LineZDepth { get; private set; } = -10f;  
    [field: SerializeField] public float LineWidth { get; private set; } = 0.5f;
    
    public GameObject UnitInPlacing { get; set; }

    private void Start()
    {
        UnitInPlacing = null;
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
        GameObject ghostUnit = ServiceLocator.Instance.UnitsManager.UnitFactory.SpawnPlacementUnit(unitType);
        UnitInPlacing = ghostUnit;
    }
}