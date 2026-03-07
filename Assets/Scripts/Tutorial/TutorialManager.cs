using UnityEngine;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour
{
    private TutorialStepData activeStep;
    private bool waiting;
    
    private List<GameObject> highlightTargets;

    public void StartStep(TutorialStepData step)
    {
        if (waiting) return;

        waiting = true;
        activeStep = new TutorialStepData(step);

        ServiceLocator.Instance.DialogueManager.ShowDialogue(activeStep.DialogueData);
        
        RegisterStepListeners(activeStep);

        if (activeStep.RequiredActionId == string.Empty)
            ServiceLocator.Instance.DialogueManager.DialogueUI.OnDialogueEnd += CompleteStep;

        if (step.PauseGame)
            ServiceLocator.Instance.GameManager.PauseGame();
    }

    public void NotifyAction(string actionId, GameObject target = null)
    {
        if (!waiting || activeStep == null) return;

        if (!activeStep.Accepts(actionId)) return;

        CompleteStep();
    }

    private void CompleteStep()
    {
        Debug.Log("CompleteStep");
        UnregisterStepListeners(activeStep);
        
        waiting = false;
        activeStep = null;

        if (ServiceLocator.Instance.GameManager.IsPaused)
        {
            ServiceLocator.Instance.GameManager.ResumeGame();
        }
        
        ServiceLocator.Instance.DialogueManager.DialogueUI.OnDialogueEnd -= CompleteStep;
    }

    private void RegisterStepListeners(TutorialStepData step)
    {
        // subscribe to your interaction system / input events
    }

    private void UnregisterStepListeners(TutorialStepData step)
    {
        // unsubscribe
    }
}