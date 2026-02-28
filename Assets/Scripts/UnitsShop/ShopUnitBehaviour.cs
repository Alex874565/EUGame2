using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopUnitBehaviour : MonoBehaviour
{
    [SerializeField] private UnitType type;
    
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI ownedText;
    [SerializeField] private Button buyButton;
    
    private UnitData _data;
    
    private void Start()
    {
        InitializeUnit();
    }
    
    public void TryBuyUnit()
    {
        if (ServiceLocator.Instance.PlayerManager.CanAfford(_data.ShopCost))
        {
            ServiceLocator.Instance.PlayerManager.SpendMoney(_data.ShopCost);
            ServiceLocator.Instance.PlayerManager.AddStartingUnit(type, 1);
            ServiceLocator.Instance.UnitsManager.SetInventoryUnitCount(type, ServiceLocator.Instance.PlayerManager.StartingUnits[type]);
            UpdateUI();
        }
    }
    
    private void UpdateUI()
    {
        costText.text = $"Cost: {_data.ShopCost}";
        ownedText.text = $"Owned: {ServiceLocator.Instance.PlayerManager.StartingUnits[type]}";
    }
    
    private void InitializeUnit()
    {
        _data = ServiceLocator.Instance.UnitsDatabase.Units.Find(unit => unit.Data.Type == type).Data;
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(TryBuyUnit);
        UpdateUI();
    }
    
    private void OnEnable()
    {
        if (ServiceLocator.Instance.PlayerManager.CanAfford(_data.ShopCost))
        {
            buyButton.enabled = true;
        }
        else
        {
            buyButton.enabled = false;
        }
    }
}