using UnityEngine;

[System.Serializable]
public class MinigameData
{
    [field: SerializeField] public MinigameType MinigameType { get; private set; }
     [field: SerializeField] public string Name { get; private set; }
     [field: SerializeField] public string Description { get; private set; }
     [field: SerializeField] public int Difficulty { get; private set; }
     [field: SerializeField] public int TimeLimit { get; private set; }
     [field: SerializeField] public int Reward { get; private set; }
     [field: SerializeField] public int TimeOnMap { get; private set; }
}