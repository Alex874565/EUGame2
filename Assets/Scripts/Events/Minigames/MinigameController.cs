using UnityEngine;

public class MinigameController : MonoBehaviour
{
    [field: SerializeField] public MinigameType Type { get; set; }
    
    public MinigameData Data { get; set; }
    
    public virtual void StartMinigame()
    {
    }

    public void GiveReward(bool won)
    {
        int currentMoney = ServiceLocator.Instance.WavesManager.CurrentMoney;
        ServiceLocator.Instance.WavesManager.UpdateMoney(won ? currentMoney + Data.Reward : currentMoney);
    }
}