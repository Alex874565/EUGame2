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
    [field: SerializeField] public int EmergencyFailLimit { get; private set; }
    [field: SerializeField] public float WaveDuration { get; private set; }
    
    public WaveData(WaveData wave)
    {
        WaveNumber = wave.WaveNumber;
        TimelineAsset = wave.TimelineAsset;
        StartingUnits = wave.StartingUnits;
        StartingMoney = wave.StartingMoney;
        EmergencyFailLimit = wave.EmergencyFailLimit;
        WaveDuration = wave.WaveDuration;
    }
}