using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(EmergencyFactory))]
public class EmergenciesManager : MonoBehaviour
{
    public Dictionary<EmergencyType, EmergencyData> EmergenciesData { get; private set; }
    public List<GameObject> ActiveEmergencies { get; private set; }
    public EmergencyFactory EmergencyFactory { get; private set; }

    private void Awake()
    {
        EmergenciesData = new Dictionary<EmergencyType, EmergencyData>();
        ActiveEmergencies = new List<GameObject>();
        EmergencyFactory = GetComponent<EmergencyFactory>();
    }
    
    private void Start()
    {
        InitializeEmergenciesData(ServiceLocator.Instance.EmergenciesDatabase.Emergencies.Select(emergency => emergency.EmergencyData).ToList());
    }

    public void SetEmergencyData(EmergencyType emergencyType, EmergencyData emergencyData)
    {
        if (!EmergenciesData.ContainsKey(emergencyType))
        {
            EmergenciesData.Add(emergencyType, emergencyData);
        }
        else
        {
            EmergenciesData[emergencyType] = emergencyData;
        }
    }

    public void InitializeEmergenciesData(List<EmergencyData> emergencies)
    {
        foreach (EmergencyData emergencyData in emergencies)
        {
            SetEmergencyData(emergencyData.EmergencyType, emergencyData);
        }
    }
    
    public void ModifyEmergenciesStat(EmergencyUpgradeModifier modifier)
    {
        foreach (EmergencyType emergencyType in modifier.AffectedType)
        {
            if (EmergenciesData.ContainsKey(emergencyType))
            {
                EmergencyData data = EmergenciesData[emergencyType];
                data.ModifyStat(modifier.ModifiedStat, modifier.Value);
            }
        }
    }
    
    public void DestroyEmergencies()
    {
        foreach (GameObject emergency in ActiveEmergencies)
        {
            Destroy(emergency);
        }
        ActiveEmergencies.Clear();
    }

    public void EliminateEmergency(GameObject emergency)
    {
        ActiveEmergencies.Remove(emergency);
    }
}