using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;

[System.Serializable]
public class WaveData
{
    [field: SerializeField ] public int WaveNumber { get; set; }
    [field: SerializeField] public PlayableAsset TimelineAsset { get; private set; }
    [field: SerializeField] public List<StartingUnitData> StartingUnits { get; private set; }
    [field: SerializeField] public int StartingMoney { get; private set; }
}