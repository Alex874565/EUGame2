using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TutorialClip : PlayableAsset, ITimelineClipAsset
{
    [SerializeField] private TutorialStepData tutorialStepData;

    public ClipCaps clipCaps => ClipCaps.None;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<TutorialPlayable>.Create(graph);
        var tutorialPlayable = playable.GetBehaviour();
        tutorialPlayable.TutorialStepData = tutorialStepData;
        return playable;
    }
}