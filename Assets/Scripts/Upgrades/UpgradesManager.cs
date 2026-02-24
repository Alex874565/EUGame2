using UnityEngine;
using System.Collections.Generic;

public class UpgradesManager : MonoBehaviour
{
    [field: SerializeField] public List<UpgradeBehaviour> UpgradeScripts { get; private set; }
    
    public Dictionary<UpgradeType, List<UpgradeData>> UpgradesData { get; private set; }

    public Dictionary<UpgradeType, int> OwnedUpgrades { get; private set; }

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
        
        OwnedUpgrades = new Dictionary<UpgradeType, int>();
        OwnedUpgrades.Add(UpgradeType.Civic, 0);
        OwnedUpgrades.Add(UpgradeType.Disinformation, 0);
        OwnedUpgrades.Add(UpgradeType.Democracy, 0);
        
        UpdateUIs();
    }
    
    public void ApplyUpgrade(UpgradeType upgradeType, int level, bool isFree)
    {
        bool upgradeApplied = UpgradeScripts.Find(upgrade => upgrade.Type == upgradeType && upgrade.Level == level).TryApplyUpgrade(isFree);
        if (upgradeApplied)
        {
            OwnedUpgrades[upgradeType] += 1;
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

    #region Checks
    
        public bool IsUpgradeOwned(UpgradeType upgradeType, int level)
        {
            return OwnedUpgrades.ContainsKey(upgradeType) && OwnedUpgrades[upgradeType] >= level;
        }
        
        public bool IsUpgradeAvailable(UpgradeType upgradeType, int level)
        {
            return level == OwnedUpgrades[upgradeType] + 1;
        }
    
    #endregion
}