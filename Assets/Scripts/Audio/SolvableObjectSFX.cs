using UnityEngine;

public class SolvableObjectSFX : SelectableObjectSFX
{
    [SerializeField] private AudioClip solveClip;
    [SerializeField] private AudioClip unsolveClip;
    
    public void PlaySolveSFX()
    {
        if(solveClip == null) return;
        
        ServiceLocator.Instance.AudioManager.PlayUI(solveClip);
    }
    
    public void PlayUnsolveSFX()
    {
        if(unsolveClip == null) return;
        
        ServiceLocator.Instance.AudioManager.PlayUI(unsolveClip);
    }
}