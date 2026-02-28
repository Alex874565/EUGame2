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
    public List<int> StartingUnits;
    
    public SaveData(int waveIndex, bool wonLastWave, int playerMoney, Dictionary<UpgradeType, int> ownedUpgrades, Dictionary<UnitType, int> startingUnits)
    {
        WaveIndex = waveIndex;
        WonLastWave = wonLastWave;
        PlayerMoney = playerMoney;
        CivicUpgrades = ownedUpgrades[UpgradeType.Civic];
        DisinformationUpgrades = ownedUpgrades[UpgradeType.Disinformation];
        DemocracyUpgrades = ownedUpgrades[UpgradeType.Democracy];
        StartingUnits = GetStartingUnitsList(startingUnits);
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
    
    private List<int> GetStartingUnitsList(Dictionary<UnitType, int> startingUnitsDict)
    {
        StartingUnits = new List<int>
        {
            startingUnitsDict[UnitType.Airplane],
            startingUnitsDict[UnitType.DecontaminationUnit],
            startingUnitsDict[UnitType.FireUnit],
            startingUnitsDict[UnitType.Helicopter],
            startingUnitsDict[UnitType.MedicalUnit],
            startingUnitsDict[UnitType.MilitaryUnit],
            startingUnitsDict[UnitType.RescueUnit]
        };
        return StartingUnits;
    }
    
    public Dictionary<UnitType, int> GetStartingUnitsDictionary()
    {
        Dictionary<UnitType, int> startingUnitsDict = new Dictionary<UnitType, int>
        {
            { UnitType.Airplane, StartingUnits[0] },
            { UnitType.DecontaminationUnit, StartingUnits[1] },
            { UnitType.FireUnit, StartingUnits[2] },
            { UnitType.Helicopter, StartingUnits[3] },
            { UnitType.MedicalUnit, StartingUnits[4] },
            { UnitType.MilitaryUnit, StartingUnits[5] },
            { UnitType.RescueUnit, StartingUnits[6] }
        };
        return startingUnitsDict;
    }
}