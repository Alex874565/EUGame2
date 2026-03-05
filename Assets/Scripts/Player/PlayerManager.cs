using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager _instance;
    
    [field: SerializeField] private int startingMoney = 100;
    
    public int Money { get; private set; }
    public Dictionary<UpgradeType, int> OwnedUpgrades { get; set; }
    public Dictionary<UnitType, int> StartingUnits { get; set; }
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;

        Money = startingMoney;
        OwnedUpgrades = new Dictionary<UpgradeType, int>
        {
            { UpgradeType.Civic, 0 },
            { UpgradeType.Disinformation, 0 },
            { UpgradeType.Democracy, 0 }
        };
        StartingUnits = new Dictionary<UnitType, int>
        {
            { UnitType.Airplane, 0 },
            { UnitType.DecontaminationUnit, 0 },
            { UnitType.FireUnit, 0 },
            { UnitType.Helicopter, 0 },
            { UnitType.MedicalUnit, 0 },
            { UnitType.MilitaryUnit, 0 },
            { UnitType.RescueUnit, 0 }
        };
        
        DontDestroyOnLoad(gameObject);
    }
    
    public void AddMoney(int amount)
    {
        Money += amount;
        ServiceLocator.Instance.SaveManager.SaveGame(new SaveData(
            ServiceLocator.Instance.GameManager.WaveIndex,
            ServiceLocator.Instance.GameManager.WonLastWave,
            Money,
            OwnedUpgrades,
            StartingUnits
        ));
    }
    
    public void SpendMoney(int amount)
    {
        if (Money >= amount)
        {
            Money -= amount;
        }
    }
    
    public bool CanAfford(int amount)
    {
        Debug.Log(Money >= amount);
        return Money >= amount;
    }
    
    public bool IsUpgradeOwned(UpgradeType upgradeType, int level)
    {
        return OwnedUpgrades.ContainsKey(upgradeType) && OwnedUpgrades[upgradeType] >= level;
    }
        
    public bool IsUpgradeAvailable(UpgradeType upgradeType, int level)
    {
        return level == OwnedUpgrades[upgradeType] + 1;
    }
    
    public void AddOwnedUpgrade(UpgradeType upgradeType, int level)
    {
        if (OwnedUpgrades.ContainsKey(upgradeType))
        {
            OwnedUpgrades[upgradeType] = level;
        }
    }
    
    public void AddStartingUnit(UnitType unitType, int count)
    {
        if (StartingUnits.ContainsKey(unitType))
        {
            StartingUnits[unitType] += count;
        }
        foreach(var unit in StartingUnits)
        {
            Debug.Log($"{unit.Key}: {unit.Value}");
        }
    }
}