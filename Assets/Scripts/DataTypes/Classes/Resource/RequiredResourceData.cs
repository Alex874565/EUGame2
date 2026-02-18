using UnityEngine;

[System.Serializable]
public class RequiredResourceData
{
    [field: SerializeField] public ResourceType Type { get; set; }
    [field: SerializeField] public int Amount { get; set; }
}