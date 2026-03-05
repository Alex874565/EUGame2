using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EmergencyData
{
    [field: SerializeField] public EmergencyType EmergencyType { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public List<RequiredUnitData> RequiredResources { get; private set; }
    [field: SerializeField] public int Severity { get; private set; }
    [field: SerializeField] public int TimeUntilExpiry { get; private set; }
    [field: SerializeField] public int TimeToSolve { get; private set; }

    public void ModifyStat(EmergencyStat stat, int value)
    {
        switch (stat)
        {
            case EmergencyStat.TimeToSolve:
                TimeToSolve += value;
                break;
            case EmergencyStat.TimeUntilExpiry:
                TimeUntilExpiry += value;
                break;
        }
    }
    
    public EmergencyData(EmergencyData emergencyData)
    {
        EmergencyType = emergencyData.EmergencyType;
        Name = emergencyData.Name;
        Description = emergencyData.Description;
        RequiredResources = new List<RequiredUnitData>(emergencyData.RequiredResources);
        Severity = emergencyData.Severity;
        TimeUntilExpiry = emergencyData.TimeUntilExpiry;
        TimeToSolve = emergencyData.TimeToSolve;
    }
}