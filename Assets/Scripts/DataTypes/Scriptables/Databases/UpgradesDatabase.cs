using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "UpgradesDatabase", menuName = "ScriptableObjects/Databases/UpgradesDatabase")]
public class UpgradesDatabase : ScriptableObject
{
    [field: SerializeField] public List<UpgradeData> CivicUpgrades { get; private set; }
    [field: SerializeField] public List<UpgradeData> DemocracyUpgrades { get; private set; }
    [field: SerializeField] public List<UpgradeData> DisinformationUpgrades { get; private set; }
    
    private void OnValidate()
    {
        foreach (UpgradeData upgrade in CivicUpgrades)
        {
            upgrade.Type = UpgradeType.Civic;
            upgrade.Level = CivicUpgrades.IndexOf(upgrade) + 1;
            
            SetModifierTypes(upgrade);
        }

        foreach (UpgradeData upgrade in DemocracyUpgrades)
        {
            upgrade.Type = UpgradeType.Democracy;
            upgrade.Level = DemocracyUpgrades.IndexOf(upgrade) + 1;

            SetModifierTypes(upgrade);
        }

        foreach (UpgradeData upgrade in DisinformationUpgrades)
        {
            upgrade.Type = UpgradeType.Disinformation;
            upgrade.Level = DisinformationUpgrades.IndexOf(upgrade) + 1;
            
            SetModifierTypes(upgrade);
        }
    }

    private void SetModifierTypes(UpgradeData upgrade)
    {
        foreach (EmergencyUpgradeModifier modifier in upgrade.EmergencyModifiers)
        {
            modifier.Type = ModifierType.Emergency;
        }

        foreach (MinigameUpgradeModifier modifier in upgrade.MinigameModifiers)
        {
            modifier.Type = ModifierType.Minigame;
        }
    }
}