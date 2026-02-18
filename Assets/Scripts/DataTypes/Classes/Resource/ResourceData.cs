using UnityEngine;

[System.Serializable]
public class ResourceData
{
    [field: SerializeField] public ResourceType Type { get; set; }
    [field: SerializeField] public ResourceMobility Mobility { get; set; }
    [field: SerializeField] public int Speed { get; set; }
    [field: SerializeField] public int MovementCost { get; set; }
}