using UnityEngine;

public class SelectableObjectSFX : MonoBehaviour
{
    [SerializeField] private AudioClip selectClip;
    [SerializeField] private AudioClip deselectClip;
    [SerializeField] private AudioClip appearClip;
    
    public void PlaySelectSFX()
    {
        if(selectClip == null) return;
        
        ServiceLocator.Instance.AudioManager.PlayUIRandomPitch(selectClip);
    }
    
    public void PlayDeselectSFX()
    {
        if(deselectClip == null) return;
        
        ServiceLocator.Instance.AudioManager.PlayUIRandomPitch(deselectClip);
    }
    
    public void PlayAppearSFX()
    {
        if(appearClip == null) return;
        
        ServiceLocator.Instance.AudioManager.PlayUIRandomPitch(appearClip);
    }
}