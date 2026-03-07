using UnityEngine;

[CreateAssetMenu(fileName = "TutorialStepData", menuName = "ScriptableObjects/TutorialStepData")]
public class TutorialStepData : ScriptableObject
{
    [field: SerializeField] public int StepIndex { get; private set; }
    [field: SerializeField] public DialogueData DialogueData { get; private set; }
    [field: SerializeField] public string RequiredActionId { get; private set; }
    [field: SerializeField] public string[] HighlightTargetIds { get; private set; }
    [field: SerializeField] public bool PauseGame { get; private set; } = true;

    private bool _completed = false;
    
    public TutorialStepData(TutorialStepData step)
    {
        StepIndex = step.StepIndex;
        DialogueData = step.DialogueData;
        RequiredActionId = step.RequiredActionId;
        HighlightTargetIds = step.HighlightTargetIds;
        PauseGame = step.PauseGame;
    }
    
    public bool Accepts(string actionId)
    {
        return actionId == RequiredActionId;
    }

    public bool IsCompleteAfter(string actionId)
    {
        if (!Accepts(actionId)) return false;

        _completed = true;
        return true;
    }
}