using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;
using System.Collections.Generic;

public class EmergencyClip : PlayableAsset, ITimelineClipAsset
{
    [SerializeField] private List<EmergencySpawnData> spawnData;

    public ClipCaps clipCaps => ClipCaps.None;
    
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<EmergencyPlayable>.Create(graph);
        var emergencyPlayable = playable.GetBehaviour();
        emergencyPlayable.SpawnData = spawnData;
        return playable;
    }
}