using UnityEngine.Playables;

public class TutorialPlayable : PlayableBehaviour
{
    public DialogueData Dialogue { get; set; }
    
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        ServiceLocator.Instance.DialogueManager.ShowDialogue(Dialogue);
    }
}