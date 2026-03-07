using UnityEngine;

public class TutorialTarget : MonoBehaviour
{
    [field: SerializeField] public string TargetId { get; private set; }
    
    
    
    public void NotifyAction()
    {
        ServiceLocator.Instance.TutorialManager.NotifyAction(TargetId, gameObject);
    }
}