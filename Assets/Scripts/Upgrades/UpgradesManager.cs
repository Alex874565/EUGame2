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
        UpgradesData = new Dictionary<UpgradeType, List<UpgradeData>>
        {
            { UpgradeType.Civic, ServiceLocator.Instance.UpgradesDatabase.CivicUpgrades },
            { UpgradeType.Disinformation, ServiceLocator.Instance.UpgradesDatabase.DisinformationUpgrades },
            { UpgradeType.Democracy, ServiceLocator.Instance.UpgradesDatabase.DemocracyUpgrades }
        };

        foreach (UpgradeBehaviour upgrade in UpgradeScripts)
        {
            upgrade.InitializeUpgrade();
        }
    }
    
    public void ApplyUpgrade(UpgradeType upgradeType, int level, bool isFree)
    {
        bool upgradeApplied = UpgradeScripts.Find(upgrade => upgrade.Type == upgradeType && upgrade.Level == level).TryApplyUpgrade(isFree);
        if (upgradeApplied)
        {
            ServiceLocator.Instance.PlayerManager.AddOwnedUpgrade(upgradeType, level);
            ServiceLocator.Instance.SaveManager.SaveGame(new SaveData(
                ServiceLocator.Instance.GameManager.WaveIndex,
                ServiceLocator.Instance.GameManager.WonLastWave,
                ServiceLocator.Instance.PlayerManager.Money,
                ServiceLocator.Instance.PlayerManager.OwnedUpgrades,
                ServiceLocator.Instance.PlayerManager.StartingUnits
            ));
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