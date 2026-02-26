using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EmergencyUpgradeModifier : UpgradeModifier
{
    [field: SerializeField] public EmergencyStat ModifiedStat { get; private set; }
    [field: SerializeField] public List<EmergencyType> AffectedType { get; private set; }
}