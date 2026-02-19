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
}