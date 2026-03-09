using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UnitFactory))]
public class UnitsManager : MonoBehaviour
{
    [field: SerializeField] public List<GameObject> InventoryUnits { get; private set; }

    private List<StartingUnitData> InventoryUnitsTracker { get; set; } = new();
    public List<GameObject> ActiveUnits { get; private set; } = new();
    
    public UnitFactory UnitFactory { get; private set; }
    
    private void Awake()
    {
        UnitFactory = GetComponent<UnitFactory>();
    }
    
    public void InitializeUnits(int waveNumber)
    {
        List<StartingUnitData> unitsToInitialize = new List<StartingUnitData>();

        foreach (StartingUnitData originalData in ServiceLocator.Instance.WavesDatabase.Waves[waveNumber].StartingUnits)
        {
            StartingUnitData clonedData = originalData.Clone();
            clonedData.Count += ServiceLocator.Instance.PlayerManager.StartingUnits[clonedData.Type];
            unitsToInitialize.Add(clonedData);
            InventoryUnitsTracker.Add(clonedData);
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