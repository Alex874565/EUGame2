using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(EmergencyFactory))]
public class EmergenciesManager : MonoBehaviour
{
    public Dictionary<EmergencyType, EmergencyData> EmergenciesData { get; private set; }
    public List<GameObject> ActiveEmergencies { get; private set; }
    public EmergencyFactory EmergencyFactory { get; private set; }

    private void Awake()
    {
        EmergenciesData = new Dictionary<EmergencyType, EmergencyData>();
        ActiveEmergencies = new List<GameObject>();
        EmergencyFactory = GetComponent<EmergencyFactory>();
    }
    
    private void Start()
    {
        InitializeEmergenciesData(ServiceLocator.Instance.EmergenciesDatabase.Emergencies.Select(emergency => emergency.EmergencyData).ToList());
        ApplyUpgrades();
    }

    private void SetEmergencyData(EmergencyType emergencyType, EmergencyData emergencyData)
    {
        if (!EmergenciesData.ContainsKey(emergencyType))
        {
            EmergenciesData.Add(emergencyType, new EmergencyData(emergencyData));
        }
        else
        {
            EmergenciesData[emergencyType] = new EmergencyData(emergencyData);
        }
    }

    private void InitializeEmergenciesData(List<EmergencyData> emergencies)
    {
        foreach (EmergencyData emergencyData in emergencies)
        {
            SetEmergencyData(emergencyData.EmergencyType, emergencyData);
        }
    }

    private void ApplyUpgrades()
    {
        foreach (KeyValuePair<UpgradeType, int> ownedUpgrade in ServiceLocator.Instance.PlayerManager.OwnedUpgrades)
        {
            List<UpgradeData> upgradeDatas;
            switch (ownedUpgrade.Key)
            {
                case UpgradeType.Civic:
                    upgradeDatas = new  List<UpgradeData>(ServiceLocator.Instance.UpgradesDatabase.CivicUpgrades);
                    break;
                case UpgradeType.Disinformation:
                    upgradeDatas = new List<UpgradeData>(ServiceLocator.Instance.UpgradesDatabase.DisinformationUpgrades);
                    break;
                case UpgradeType.Democracy:
                    upgradeDatas = new List<UpgradeData>(ServiceLocator.Instance.UpgradesDatabase.DemocracyUpgrades);
                    break;
                default:
                    upgradeDatas = new List<UpgradeData>();
                    break;
            }

            for (int i = 0; i < ownedUpgrade.Value; i++)
            {
                UpgradeData upgradeData = upgradeDatas.Find(upgrade => upgrade.Level == i + 1);
                if (upgradeData != null)
                {
                    foreach (EmergencyUpgradeModifier modifier in upgradeData.EmergencyModifiers)
                    {
                        ModifyEmergenciesStat(modifier);
                    }
                }
            }
        }
    } 
    
    private void ModifyEmergenciesStat(EmergencyUpgradeModifier modifier)
    {
        foreach (EmergencyType emergencyType in modifier.AffectedType)
        {
            if (EmergenciesData.ContainsKey(emergencyType))
            {
                EmergencyData data = EmergenciesData[emergencyType];
                data.ModifyStat(modifier.ModifiedStat, modifier.Value);
            }
        }
    }
    
    public void DestroyEmergencies()
    {
        foreach (GameObject emergency in ActiveEmergencies)
        {
            Destroy(emergency);
        }
        ActiveEmergencies.Clear();
    }

    public void EliminateEmergency(GameObject emergency)
    {
        ActiveEmergencies.Remove(emergency);
    }
}