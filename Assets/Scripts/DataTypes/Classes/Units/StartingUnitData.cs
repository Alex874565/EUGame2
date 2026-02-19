using UnityEngine;

[System.Serializable]
public class StartingUnitData
{
    [field: SerializeField] public UnitType Type { get; private set; }
    [field: SerializeField] public int Count { get; private set; }
}