using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class MinigameUpgradeModifier : UpgradeModifier
{
    [field: SerializeField] public MinigameStat ModifiedStat { get; private set; }
    [field: SerializeField] public List<MinigameType> AffectedTypes { get; private set; }
}