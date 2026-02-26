using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class UpgradeData
{
    [field: SerializeField] public UpgradeType Type { get; set; }
    [field: SerializeField] public int Level { get; set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public string ModifiersDescription { get; private set; }
    [field: SerializeField] public int Cost { get; private set; }
    [field: SerializeField] public List<EmergencyUpgradeModifier> EmergencyModifiers { get; private set; }
    [field: SerializeField] public List<MinigameUpgradeModifier> MinigameModifiers { get; private set; }
}