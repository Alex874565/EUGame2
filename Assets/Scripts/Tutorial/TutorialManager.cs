using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private TutorialStepData activeStep;
    private bool waiting;
    
    

    public void StartStep(TutorialStepData step)
    {
        if (waiting) return;

        waiting = true;
        activeStep = new TutorialStepData(step);

        PauseGameplay();
        ServiceLocator.Instance.DialogueManager.ShowDialogue(activeStep.DialogueData);
        
        RegisterStepListeners(activeStep);

        if (step.PauseGame)
            ServiceLocator.Instance.GameManager.PauseArtificially();
    }

    public void NotifyAction(string actionId, GameObject target = null)
    {
        if (!waiting || activeStep == null) return;

        if (!activeStep.Accepts(actionId)) return;

        CompleteStep();
    }

    private void CompleteStep()
    {
        UnregisterStepListeners(activeStep);

        ResumeGameplay();

        waiting = false;
        activeStep = null;

        if (ServiceLocator.Instance.GameManager.IsPaused)
        {
            ServiceLocator.Instance.GameManager.ResumeArtificially();
        }
    }

    private void PauseGameplay()
    {
        Time.timeScale = 0f;
    }

    private void ResumeGameplay()
    {
        Time.timeScale = 1f;
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