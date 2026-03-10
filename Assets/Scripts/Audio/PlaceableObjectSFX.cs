using UnityEngine;

public class PlaceableObjectSFX : SelectableObjectSFX
{
    [SerializeField] private AudioClip placeClip;
    [SerializeField] private AudioClip removeClip;
    [SerializeField] private AudioClip reachClip;
    
    public void PlayPlaceSFX()
    {
        if(placeClip == null) return;
        
        ServiceLocator.Instance.AudioManager.PlayUIRandomPitch(placeClip);
    }
    
    public void PlayReachSFX()
    {
        if(reachClip == null) return;
        
        ServiceLocator.Instance.AudioManager.PlayUIRandomPitch(reachClip);
    }
}