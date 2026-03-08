using UnityEngine;

[System.Serializable]
public class StartingUnitData
{
    [field: SerializeField] public UnitType Type { get; private set; }
    [field: SerializeField] public int Count { get; set; }

    public StartingUnitData Clone()
    {
        return new StartingUnitData
        {
            Type = this.Type,
            Count = this.Count
        };
    }
}