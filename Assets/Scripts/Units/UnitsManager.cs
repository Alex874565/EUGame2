using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UnitFactory))]
public class UnitsManager : MonoBehaviour
{
    [field: SerializeField] public List<GameObject> InventoryUnits { get; private set; }
    
    private List<StartingUnitData> InventoryUnitsTracker { get; set; }
    public List<GameObject> ActiveUnits { get; private set; }
    
    public UnitFactory UnitFactory { get; private set; }
    
    private void Awake()
    {
        InventoryUnitsTracker = new List<StartingUnitData>();
        UnitFactory = GetComponent<UnitFactory>();
        ActiveUnits = new List<GameObject>();
    }
    
    public void InitializeUnits(int waveNumber)
    {
        List<StartingUnitData> unitsToInitialize = ServiceLocator.Instance.WavesDatabase.Waves[waveNumber].StartingUnits;
        foreach (StartingUnitData startingUnitData in unitsToInitialize)
        {
            InventoryUnitsTracker.Add(startingUnitData);
        }
        
        InitializeInventoryUnits();
    }
    
    public void InitializeInventoryUnits()
    {
        foreach (GameObject inventoryUnit in InventoryUnits)
        {
            UnitBehaviour unitBehaviour = inventoryUnit.GetComponent<UnitBehaviour>();
            StartingUnitData startingUnitData = InventoryUnitsTracker.Find(unit => unit.Type == unitBehaviour.Type);
            if (startingUnitData != null)
            {
                unitBehaviour.UpdateCount(startingUnitData.Count);
            }
            else
            {
                unitBehaviour.UpdateCount(0);
            }
        }
    }
    
    public void SetInventoryUnitCount(UnitType unitType, int count)
    {
        StartingUnitData startingUnitData = InventoryUnitsTracker.Find(unit => unit.Type == unitType);
        if (startingUnitData != null)
        {
            startingUnitData.Count = count;
        }
        
        GameObject inventoryUnit = InventoryUnits.Find(unit => unit.GetComponent<UnitBehaviour>().Type == unitType);
        if (inventoryUnit != null)
        {
            inventoryUnit.GetComponent<UnitBehaviour>().UpdateCount(count);
        }
    }
    
    public void DestroyUnits()
    {
        foreach (GameObject unit in ActiveUnits)
        {
            Destroy(unit);
        }
        ActiveUnits.Clear();
    }
}