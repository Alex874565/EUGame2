using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "UnitsDatabase", menuName = "ScriptableObjects/Databases/UnitsDatabase")]
public class UnitsDatabase : ScriptableObject
{
    [field: SerializeField] public List<UnitsDatabaseObject> Units { get; private set; }

    [System.Serializable]
    public class UnitsDatabaseObject
    {
        [field: SerializeField] public GameObject Prefab { get; private set; }
        [field: SerializeField] public UnitData Data { get; private set; }
    }
}