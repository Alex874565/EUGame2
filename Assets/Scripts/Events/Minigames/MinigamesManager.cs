using System;
using UnityEngine;
using System.Collections.Generic;

public class MinigamesManager : MonoBehaviour
{
    public Dictionary<MinigameType, MinigameData> MinigamesStats { get; private set; }
    public List<GameObject> ActiveMinigames { get; private set; }

    private void Awake()
    {
        MinigamesStats = new Dictionary<MinigameType, MinigameData>();
        ActiveMinigames = new List<GameObject>();
    }
    
    private void Start()
    {
        InitializeMinigamesStats(ServiceLocator.Instance.MinigamesDatabase.Minigames);
    }

    public void SpawnMinigame(GameObject minigame, Vector3 position)
    {
        Instantiate(minigame, position, Quaternion.identity);
    }

    public void SetMinigameStats(MinigameType minigameType, MinigameData minigameData)
    {
        if (!MinigamesStats.ContainsKey(minigameType))
        {
            MinigamesStats.Add(minigameType, minigameData);
        }
        else
        {
            MinigamesStats[minigameType] = minigameData;
        }
    }

    public void InitializeMinigamesStats(List<MinigameData> minigames)
    {
        foreach (MinigameData minigameData in minigames)
        {
            SetMinigameStats(minigameData.MinigameType, minigameData);
        }
    }
}