using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EmergencyDetailsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text emergencyLocationText;
    [SerializeField] private TMP_Text emergencyNameText;
    [SerializeField] private GameObject activeUnits;
    [SerializeField] private GameObject incomingUnits;
    
    public EmergencyBehaviour Emergency { get; private set; }

    public void Initialize(EmergencyBehaviour emergency)
    {
        Emergency = emergency;
        
        emergencyLocationText.text = Emergency.LocationData.Name.ToString();
        emergencyNameText.text = Emergency.EmergencyData.Name;

        foreach (var child in activeUnits.transform)
        {
            Destroy(((Transform)child).gameObject);
        }

        foreach (var child in incomingUnits.transform)
        {
            Destroy(((Transform)child).gameObject);
        }
        
        UpdateAvailableUnits(Emergency.AvailableUnits);
        UpdateIncomingUnits(Emergency.IncomingUnits);
    }
    
    public void UpdateAvailableUnits(List<RequiredUnitData> units)
    {
        foreach (var unit in units)
        {
            GameObject activeUnitGO = ServiceLocator.Instance.UnitsManager.UnitFactory.SpawnUnit(unit.Type);
            activeUnitGO.transform.SetParent(activeUnits.transform, false);
            UnitBehaviour activeUnit = activeUnitGO.GetComponent<UnitBehaviour>();
            activeUnit.OwningEmergency = Emergency;
            activeUnit.UpdateCount(unit.Amount);
        }
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(
            activeUnits.GetComponent<RectTransform>()
        );
    }
    
    public void UpdateIncomingUnits(List<RequiredUnitData> units)
    {
        foreach (var unit in units)
        {
            GameObject activeUnitGO = ServiceLocator.Instance.UnitsManager.UnitFactory.SpawnUnit(unit.Type);
            activeUnitGO.transform.SetParent(incomingUnits.transform, false);
            UnitBehaviour activeUnit = activeUnitGO.GetComponent<UnitBehaviour>();
            activeUnit.OwningEmergency = Emergency;
            activeUnit.IsIncoming = true;
            activeUnit.IsInteractable = false;
            activeUnit.UpdateCount(unit.Amount);
        }
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(
            activeUnits.GetComponent<RectTransform>()
        );
    }
}