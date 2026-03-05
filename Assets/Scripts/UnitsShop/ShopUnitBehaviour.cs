using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ShopUnitBehaviour : MonoBehaviour
{
    [SerializeField] private UnitType type;
    
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI ownedText;
    [SerializeField] private Button buyButton;
    
    private UnitData _data;
    
    public EventHandler OnUnitPurchased;
    
    public void TryBuyUnit()
    {
        if (ServiceLocator.Instance.PlayerManager.CanAfford(_data.ShopCost))
        {
            ServiceLocator.Instance.PlayerManager.SpendMoney(_data.ShopCost);
            ServiceLocator.Instance.PlayerManager.AddStartingUnit(type, 1);
            ServiceLocator.Instance.SaveManager.SaveGame(new SaveData(
                ServiceLocator.Instance.GameManager.WaveIndex,
                ServiceLocator.Instance.GameManager.WonLastWave,
                ServiceLocator.Instance.PlayerManager.Money,
                ServiceLocator.Instance.PlayerManager.OwnedUpgrades,
                ServiceLocator.Instance.PlayerManager.StartingUnits
            ));
            OnUnitPurchased?.Invoke(this, EventArgs.Empty);
        }
    }
    
    public void UpdateUI()
    {
        if (_data == null)
        {
            InitializeUnit();
        }
        costText.text = $"{_data.ShopCost}";
        ownedText.text = $"{ServiceLocator.Instance.PlayerManager.StartingUnits[type]}";
        
        if (ServiceLocator.Instance.PlayerManager.CanAfford(_data.ShopCost))
        {
            buyButton.interactable = true;
        }
        else
        {
            buyButton.interactable = false;
        }
    }
    
    private bool initialized = false;

    private void InitializeUnit()
    {
        if (initialized) return;

        _data = ServiceLocator.Instance.UnitsDatabase.Units
            .Find(unit => unit.Data.Type == type).Data;

        buyButton.onClick.AddListener(TryBuyUnit);

        initialized = true;
    }
}