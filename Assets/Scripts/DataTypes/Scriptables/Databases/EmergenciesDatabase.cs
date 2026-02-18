using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EmergenciesDatabase", menuName = "ScriptableObjects/Databases/EmergenciesDatabase", order = 1)]
public class EmergenciesDatabase : ScriptableObject
{
    [field: SerializeField] public List<EmergencyDatabaseData> Emergencies {get; private set;}
    
    [System.Serializable]
    public struct EmergencyDatabaseData
    {
        [field: SerializeField] public GameObject EmergencyPrefab { get; private set; }
        [field: SerializeField] public EmergencyData EmergencyData { get; private set; }
    }
}