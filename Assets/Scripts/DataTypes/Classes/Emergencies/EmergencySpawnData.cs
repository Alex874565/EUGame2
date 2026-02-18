using UnityEngine;

[System.Serializable]
public class EmergencySpawnData
{
    [field: SerializeField] public EmergencyType EmergencyType { get; private set; }
    [field: SerializeField] public LocationName LocationName { get; private set; }
}