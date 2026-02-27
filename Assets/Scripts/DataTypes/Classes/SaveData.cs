using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public int WaveIndex;
    public bool WonLastWave;
    public int PlayerMoney;
    public int CivicUpgrades;
    public int DisinformationUpgrades;
    public int DemocracyUpgrades;
    
    public SaveData(int waveIndex, bool wonLastWave, int playerMoney, Dictionary<UpgradeType, int> ownedUpgrades)
    {
        WaveIndex = waveIndex;
        WonLastWave = wonLastWave;
        PlayerMoney = playerMoney;
        CivicUpgrades = ownedUpgrades[UpgradeType.Civic];
        DisinformationUpgrades = ownedUpgrades[UpgradeType.Disinformation];
        DemocracyUpgrades = ownedUpgrades[UpgradeType.Democracy];
    }

    public Dictionary<UpgradeType, int> GetOwnedUpgradesDictionary()
    {
        Dictionary<UpgradeType, int> ownedUpgradesDict = new Dictionary<UpgradeType, int>
        {
            { UpgradeType.Civic, CivicUpgrades },
            { UpgradeType.Disinformation, DisinformationUpgrades },
            { UpgradeType.Democracy, DemocracyUpgrades }
        };
        return ownedUpgradesDict;
    }
    
    private List<List<int>> GetOwnedUpgradesList(Dictionary<UpgradeType, int> ownedUpgradesDict)
    {
        List<List<int>> ownedUpgradesList = new List<List<int>>();
        ownedUpgradesList.Add(new List<int> { ownedUpgradesDict[UpgradeType.Civic] });
        ownedUpgradesList.Add(new List<int> { ownedUpgradesDict[UpgradeType.Disinformation] });
        ownedUpgradesList.Add(new List<int> { ownedUpgradesDict[UpgradeType.Democracy] });
        return ownedUpgradesList;
    }
}