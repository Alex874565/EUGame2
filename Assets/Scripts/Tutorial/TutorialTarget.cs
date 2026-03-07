using UnityEngine;

public class TutorialTarget : MonoBehaviour
{
    public void NotifyAction(string actionId)
    {
        ServiceLocator.Instance.TutorialManager.NotifyAction(actionId, gameObject);
    }
}