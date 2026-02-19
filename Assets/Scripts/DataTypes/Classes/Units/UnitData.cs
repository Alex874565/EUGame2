using UnityEngine;

[System.Serializable]
public class UnitData
{
    [field: SerializeField] public UnitType Type { get; set; }
    [field: SerializeField] public UnitMobility Mobility { get; set; }
    [field: SerializeField] public int Speed { get; set; }
    [field: SerializeField] public int MovementCost { get; set; }
}