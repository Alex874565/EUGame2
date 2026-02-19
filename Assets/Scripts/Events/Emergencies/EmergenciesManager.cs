using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(EmergencyFactory))]
public class EmergenciesManager : MonoBehaviour
{
    public Dictionary<EmergencyType, EmergencyData> EmergenciesStats { get; private set; }
    public List<GameObject> ActiveEmergencies { get; private set; }
    
    public EmergencyFactory EmergencyFactory { get; private set; }

    private void Awake()
    {
        EmergenciesStats = new Dictionary<EmergencyType, EmergencyData>();
        ActiveEmergencies = new List<GameObject>();
        EmergencyFactory = GetComponent<EmergencyFactory>();
    }
    
    private void Start()
    {
        InitializeEmergenciesStats(ServiceLocator.Instance.EmergenciesDatabase.Emergencies.Select(emergency => emergency.EmergencyData).ToList());
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