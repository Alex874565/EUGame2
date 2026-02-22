using System;
using UnityEngine;
using System.Collections.Generic;

public class MinigamesManager : MonoBehaviour
{
    [SerializeField] private Vector2 spawnIntervalRange;
    [SerializeField] private List<GameObject> minigames;
    [SerializeField] private LocationName spawnLocation;
    
    public List<GameObject> ActiveMinigames { get; private set; }
    
    private float _currentSpawnTime;
    private float _spawnTimer;
    
    public MinigamePopupBehaviour MinigameInPlay { get; set; }

    private void Awake()
    {
        ActiveMinigames = new List<GameObject>();
    }
    
    private void Start()
    {
        InitializeMinigamesStats();
        SpawnRandomMinigamePopup();
        _currentSpawnTime = UnityEngine.Random.Range(spawnIntervalRange.x, spawnIntervalRange.y);
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

    public void SpawnRandomMinigamePopup()
    {
        GameObject emergenciesLayer = ServiceLocator.Instance.UIManager.EmergenciesLayer;
        List<MinigameData> availableMinigamesData = ServiceLocator.Instance.MinigamesDatabase.Minigames;
        if (availableMinigamesData.Count == 0) return;
        MinigameType randomType = availableMinigamesData[UnityEngine.Random.Range(0, availableMinigamesData.Count)].Type;
        GameObject minigame = availableMinigamesData.Find(mg => mg.Type == randomType).Prefab;
        Vector3 position = ServiceLocator.Instance.LocationsDatabase.Locations.Find(loc => loc.Name == spawnLocation).Coordinates;
        Instantiate(minigame, position, Quaternion.identity, emergenciesLayer.transform);
    }

    public void InitializeMinigamesStats()
    {
        foreach (GameObject minigame in minigames)
        {
            MinigameType minigameType = minigame.GetComponent<MinigameController>().Type;
            MinigameData minigameData = ServiceLocator.Instance.MinigamesDatabase.Minigames.Find(mg => mg.Type == minigameType);
            minigame.GetComponent<MinigameController>().Data = minigameData;
        }
    }
    
    public void StartMinigame(MinigamePopupBehaviour minigamePopup)
    {
        GameObject minigame = minigames.Find(mg => mg.GetComponent<MinigameController>().Type == minigamePopup.Type);
        if (minigame != null)
        {
            MinigameInPlay = minigamePopup;
            minigame.SetActive(true);
            minigame.GetComponent<MinigameController>().StartMinigame();
        }
    }
    
    public void CloseMinigame()
    {
        GameObject minigame = minigames.Find(mg => mg.GetComponent<MinigameController>().Type == MinigameInPlay.Type);
        minigame.SetActive(false);
        Destroy(MinigameInPlay.gameObject);
        MinigameInPlay = null;
    }
    
    public void DestroyMinigames()
    {
        foreach (GameObject minigame in ActiveMinigames)
        {
            Destroy(minigame);
        }
        ActiveMinigames.Clear();
    }
}