using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EmergenciesDatabase", menuName = "ScriptableObjects/Databases/EmergenciesDatabase", order = 1)]
public class EmergenciesDatabase : ScriptableObject
{
    [field: SerializeField] public List<EmergencyDatabaseObject> Emergencies {get; private set;}
    
    [System.Serializable]
    public struct EmergencyDatabaseObject
    {
        [field: SerializeField] public GameObject EmergencyPrefab { get; private set; }
        [field: SerializeField] public EmergencyData EmergencyData { get; private set; }
    }
}