using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int Money { get; private set; }
    
    public void AddMoney(int amount)
    {
        Money += amount;
    }
    
    public void SpendMoney(int amount)
    {
        if (Money >= amount)
        {
            Money -= amount;
        }
    }
    
    public bool CanAfford(int amount)
    {
        return Money >= amount;
    }
}