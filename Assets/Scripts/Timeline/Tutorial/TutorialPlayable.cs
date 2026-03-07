using UnityEngine.Playables;

public class TutorialPlayable : PlayableBehaviour
{
    public TutorialStepData TutorialStepData { get; set; }
    
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        ServiceLocator.Instance.TutorialManager.StartStep(TutorialStepData);
    }
}