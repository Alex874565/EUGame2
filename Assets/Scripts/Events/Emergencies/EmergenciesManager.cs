using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EmergenciesManager : MonoBehaviour
{
    public Dictionary<EmergencyType, EmergencyData> EmergenciesStats { get; private set; }
    public List<GameObject> ActiveEmergencies { get; private set; }

    private void Awake()
    {
        EmergenciesStats = new Dictionary<EmergencyType, EmergencyData>();
        ActiveEmergencies = new List<GameObject>();
    }
    
    private void Start()
    {
        InitializeEmergenciesStats(ServiceLocator.Instance.EmergenciesDatabase.Emergencies.Select(emergency => emergency.EmergencyData).ToList());
    }
    
    public void SpawnEmergency(EmergencyType emergencyType, Vector3 position)
    {
        GameObject emergencyPrefab = ServiceLocator.Instance.EmergenciesDatabase.Emergencies.Find(emergency => emergency.EmergencyData.EmergencyType == emergencyType).EmergencyPrefab;
        Instantiate(emergencyPrefab, position, Quaternion.identity);
    }

    public void SetEmergencyStats(EmergencyType emergencyType, EmergencyData emergencyData)
    {
        if (!EmergenciesStats.ContainsKey(emergencyType))
        {
            EmergenciesStats.Add(emergencyType, emergencyData);
        }
        else
        {
            EmergenciesStats[emergencyType] = emergencyData;
        }
    }

    public void InitializeEmergenciesStats(List<EmergencyData> emergencies)
    {
        foreach (EmergencyData emergencyData in emergencies)
        {
            SetEmergencyStats(emergencyData.EmergencyType, emergencyData);
        }
    }
}