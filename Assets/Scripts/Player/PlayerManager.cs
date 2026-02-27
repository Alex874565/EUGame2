using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager _instance;
    
    [field: SerializeField] private int startingMoney = 100;
    
    public int Money { get; private set; }
    public Dictionary<UpgradeType, int> OwnedUpgrades { get; set; }
    
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
        
        DontDestroyOnLoad(gameObject);
    }
    
    public void AddMoney(int amount)
    {
        Money += amount;
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
}