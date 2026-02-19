using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UnitFactory))]
public class UnitsManager : MonoBehaviour
{
    [field: SerializeField] public List<GameObject> InventoryUnits { get; private set; }
    
    public List<StartingUnitData> ActiveUnits { get; set; }
    public UnitFactory UnitFactory { get; private set; }
    
    private void Awake()
    {
        ActiveUnits = new List<StartingUnitData>();
        UnitFactory = GetComponent<UnitFactory>();
    }
    
    public void InitializeUnits(int waveNumber)
    {
        List<StartingUnitData> unitsToInitialize = ServiceLocator.Instance.WavesDatabase.Waves[waveNumber].StartingUnits;
        foreach (StartingUnitData startingUnitData in unitsToInitialize)
        {
            ActiveUnits.Add(startingUnitData);
        }
        
        InitializeInventoryUnits();
    }
    
    public void InitializeInventoryUnits()
    {
        foreach (GameObject inventoryUnit in InventoryUnits)
        {
            UnitBehaviour unitBehaviour = inventoryUnit.GetComponent<UnitBehaviour>();
            StartingUnitData startingUnitData = ActiveUnits.Find(unit => unit.Type == unitBehaviour.Type);
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
}