using System;
using UnityEngine;
using System.Collections.Generic;

public class MinigamesManager : MonoBehaviour
{
    [SerializeField] private Vector2 spawnIntervalRange;
    [SerializeField] private List<GameObject> minigames;
    [SerializeField] private LocationName spawnLocation;
    [field: Header("Minigames Data")]
    [field: SerializeField] public MinigamesDatabase MinigamesDatabase { get; set; }

    public List<GameObject> ActiveMinigames { get; private set; } = new();
    public Dictionary<MinigameType, MinigameData> MinigamesData { get; private set; } = new();
    
    private float _currentSpawnTime;
    private float _spawnTimer;
    
    public MinigamePopupBehaviour MinigameInPlay { get; set; }
    
    public bool GamePlaying { get; set; }

    private void Start()
    {
        InitializeMinigamesData();
        ApplyUpgrades();
        if (ServiceLocator.Instance.GameManager.WaveIndex == 0)
        {
            _currentSpawnTime = 25f;
        }
        else
        {
            _currentSpawnTime = UnityEngine.Random.Range(spawnIntervalRange.x, spawnIntervalRange.y);
        }
    }

    private void Update()
    {
        if (ActiveMinigames.Count == 0 && MinigameInPlay == null)
        {
            _spawnTimer += Time.deltaTime;
            if (_spawnTimer >= _currentSpawnTime)
            {
                SpawnRandomMinigamePopup();
                _spawnTimer = 0f;
                _currentSpawnTime = UnityEngine.Random.Range(spawnIntervalRange.x, spawnIntervalRange.y);
            }
        }
    }

    private void SpawnRandomMinigamePopup()
    {
        GameObject emergenciesLayer = ServiceLocator.Instance.UIManager.EmergenciesLayer;
        List<MinigameData> availableMinigamesData = MinigamesDatabase.Minigames;
        if (availableMinigamesData.Count == 0) return;
        MinigameType randomType = availableMinigamesData[UnityEngine.Random.Range(0, availableMinigamesData.Count)].Type;
        GameObject minigame = availableMinigamesData.Find(mg => mg.Type == randomType).Prefab;
        Vector3 position = ServiceLocator.Instance.LocationsDatabase.Locations.Find(loc => loc.Name == spawnLocation).Coordinates;
        Instantiate(minigame, position, Quaternion.identity, emergenciesLayer.transform);
    }

    private void InitializeMinigamesData()
    {
        foreach (GameObject minigame in minigames)
        {
            MinigameType minigameType = minigame.GetComponent<MinigameController>().Type;
            MinigameData minigameData = new MinigameData(MinigamesDatabase.Minigames.Find(mg => mg.Type == minigameType));
            MinigamesData.Add(minigameType, minigameData);
        }
    }

    private void ApplyUpgrades()
    {
        foreach (KeyValuePair<UpgradeType, int> ownedUpgrade in ServiceLocator.Instance.PlayerManager.OwnedUpgrades)
        {
            List<UpgradeData> upgradesData;
            switch (ownedUpgrade.Key)
            {
                case UpgradeType.Civic:
                    upgradesData = new List<UpgradeData>(ServiceLocator.Instance.UpgradesDatabase.CivicUpgrades);
                    break;
                case UpgradeType.Democracy:
                    upgradesData = new List<UpgradeData>(ServiceLocator.Instance.UpgradesDatabase.DemocracyUpgrades);
                    break;
                case UpgradeType.Disinformation:
                    upgradesData = new List<UpgradeData>(ServiceLocator.Instance.UpgradesDatabase.DisinformationUpgrades);
                    break;
                default:
                    upgradesData = new List<UpgradeData>();
                    break;
            }

            for (int i = 0; i < ownedUpgrade.Value; i++)
            {
                UpgradeData upgradeData = upgradesData.Find(upgrade => upgrade.Level == i + 1);
                if (upgradeData != null)
                {
                    foreach (MinigameUpgradeModifier modifier in upgradeData.MinigameModifiers)
                    {
                        ModifyMinigamesStat(modifier);
                    }
                }
            }
        }
    }
    
    private void ModifyMinigamesStat(MinigameUpgradeModifier minigameUpgradeModifier)
    {
        foreach (MinigameType minigameType in minigameUpgradeModifier.AffectedTypes)
        {
            if (MinigamesData.ContainsKey(minigameType))
            {
                MinigameData data = MinigamesData[minigameType];
                data.ModifyStat(minigameUpgradeModifier.ModifiedStat, minigameUpgradeModifier.Value);
            }
        }
    }
    
    public void StartMinigame(MinigamePopupBehaviour minigamePopup)
    {
        GameObject minigame = minigames.Find(mg => mg.GetComponent<MinigameController>().Type == minigamePopup.Type);
        if (minigame != null)
        {
            MinigameInPlay = minigamePopup;
            ServiceLocator.Instance.UIManager.Inventory.SetActive(false);
            minigame.SetActive(true);
            minigame.GetComponent<MinigameController>().StartMinigame();
        }
    }
    
    public void CloseMinigame()
    {
        GameObject minigame = minigames.Find(mg => mg.GetComponent<MinigameController>().Type == MinigameInPlay.Type);
        minigame.SetActive(false);
        MinigameInPlay.DestroySelf();
        ServiceLocator.Instance.UIManager.Inventory.SetActive(true);
    }
    
    public void DestroyMinigames()
    {
        foreach (GameObject minigame in ActiveMinigames)
        {
            minigame.SetActive(false);
        }
        ActiveMinigames.Clear();
    }
}