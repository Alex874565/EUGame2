using UnityEngine;

public class MinigameController : MonoBehaviour
{
    [field: SerializeField] public MinigameType Type { get; set; }
    
    public MinigameData Data { get; set; }
    
    public virtual void StartMinigame()
    {
    }
}