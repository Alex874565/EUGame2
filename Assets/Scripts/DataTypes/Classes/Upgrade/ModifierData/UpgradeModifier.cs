using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeModifier
{
    [field: SerializeField] public ModifierType Type { get; set; }
    [field: SerializeField] public int Value { get; private set; }
}