using UnityEngine;

public class SelectableObjectSFX : MonoBehaviour
{
    [SerializeField] private AudioClip selectClip;
    [SerializeField] private AudioClip deselectClip;
    [SerializeField] private AudioClip appearClip;
    
    public void PlaySelectSFX()
    {
        if(selectClip == null) return;
        
        ServiceLocator.Instance.AudioManager.PlayUI(selectClip);
    }
    
    public void PlayDeselectSFX()
    {
        if(deselectClip == null) return;
        
        ServiceLocator.Instance.AudioManager.PlayUI(deselectClip);
    }
    
    public void PlayAppearSFX()
    {
        if(appearClip == null) return;
        
        ServiceLocator.Instance.AudioManager.PlayUI(appearClip);
    }
}