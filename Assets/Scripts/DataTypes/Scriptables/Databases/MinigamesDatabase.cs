using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MinigamesDatabase", menuName = "ScriptableObjects/Databases/MinigamesDatabase", order = 1)]
public class MinigamesDatabase : ScriptableObject
{
    [field: SerializeField] public List<MinigameData> Minigames { get; private set; }
}