using UnityEngine;

[System.Serializable]
public class MinigameData
{
    [field: SerializeField] public MinigameType Type { get; private set; }
     [field: SerializeField] public string Name { get; private set; }
     [field: SerializeField] public string Description { get; private set; }
     [field: SerializeField] public int ScoreToWin { get; private set; }
     [field: SerializeField] public int TimeLimit { get; private set; }
     [field: SerializeField] public int Reward { get; private set; }
     [field: SerializeField] public GameObject Prefab { get; private set; }
     
     public void ModifyStat(MinigameStat stat, int value)
     {
         switch (stat)
         {
             case MinigameStat.ScoreToWin:
                 ScoreToWin += value;
                 break;
             case MinigameStat.TimeLimit:
                 TimeLimit += value;
                 break;
             case MinigameStat.Reward:
                 Reward += value;
                 break;
         }
     }
}