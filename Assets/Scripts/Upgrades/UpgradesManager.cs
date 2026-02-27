using UnityEngine;
using System.Collections.Generic;

public class UpgradesManager : MonoBehaviour
{
    [field: SerializeField] public List<UpgradeBehaviour> UpgradeScripts { get; private set; }
    
    public Dictionary<UpgradeType, List<UpgradeData>> UpgradesData { get; private set; }

    private void Start()
    {
        InitializeUpgrades();
    }
    
    public void InitializeUpgrades()
    {
        UpgradesData = new Dictionary<UpgradeType, List<UpgradeData>>();
        UpgradesData.Add(UpgradeType.Civic, ServiceLocator.Instance.UpgradesDatabase.CivicUpgrades);
        UpgradesData.Add(UpgradeType.Disinformation, ServiceLocator.Instance.UpgradesDatabase.DisinformationUpgrades);
        UpgradesData.Add(UpgradeType.Democracy, ServiceLocator.Instance.UpgradesDatabase.DemocracyUpgrades);

        UpdateUIs();
    }
    
    public void ApplyUpgrade(UpgradeType upgradeType, int level, bool isFree)
    {
        bool upgradeApplied = UpgradeScripts.Find(upgrade => upgrade.Type == upgradeType && upgrade.Level == level).TryApplyUpgrade(isFree);
        if (upgradeApplied)
        {
            ServiceLocator.Instance.PlayerManager.OwnedUpgrades[upgradeType] += 1;
            UpdateUIs();
        }
    }
    
    private void UpdateUIs()
    {
        foreach (UpgradeBehaviour upgrade in UpgradeScripts)
        {
            upgrade.UpdateUI();
        }
    }
}