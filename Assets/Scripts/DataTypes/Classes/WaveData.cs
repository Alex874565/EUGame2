using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class WaveData
{
    [field: SerializeField] public PlayableAsset TimelineAsset { get; private set; }
    [field: SerializeField] public int StartingMoney { get; private set; }
}