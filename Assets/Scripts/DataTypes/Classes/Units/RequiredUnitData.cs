using UnityEngine;

[System.Serializable]
public class RequiredUnitData
{
    [field: SerializeField] public UnitType Type { get; set; }
    [field: SerializeField] public int Amount { get; set; }
}