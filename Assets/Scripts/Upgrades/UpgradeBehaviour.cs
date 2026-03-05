using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [field: SerializeField] public UpgradeType Type { get; set; }
    [field: SerializeField] public int Level { get; set; }
    [field: SerializeField] private Image background;

    [Header("UI Elements")] 
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private GameObject costImage;
    [SerializeField] private Button buyButton;
    [SerializeField] private UpgradesTooltipUI tooltip;
    [Header("Colors")]
    [SerializeField] private Color unlockedColor = Color.white;
    [SerializeField] private Color unlockableColor = Color.gray;
    [SerializeField] private Color lockedColor = Color.black;
    [SerializeField] private Color bgUnlockedColor = Color.green;
    [SerializeField] private Color bgUnlockableColor = Color.gray;
    
    private UpgradeData _data;
    
    public void InitializeUpgrade()
    {
        _data = ServiceLocator.Instance.UpgradesManager.UpgradesData[Type][Level - 1];
        
        buyButton.onClick.AddListener(() =>
        {
            ServiceLocator.Instance.UpgradesManager.ApplyUpgrade(Type, Level, false);
        });
            
        UpdateUI();
    }

    public bool TryApplyUpgrade(bool isFree)
    {
        if (isFree || ServiceLocator.Instance.PlayerManager.CanAfford(_data.Cost))
        {
            if (!isFree)
            {
                ServiceLocator.Instance.PlayerManager.SpendMoney(_data.Cost);
            }
            return true;
        }

        return false;
    }

    public void UpdateUI()
    {
        if(ServiceLocator.Instance.PlayerManager.IsUpgradeOwned(Type, Level))
        {
            costText.text = $"Owned";
            costImage.SetActive(false);
            image.color = unlockedColor;
            background.color = bgUnlockedColor;
        }
        else
        {
            if(ServiceLocator.Instance.PlayerManager.IsUpgradeAvailable(Type, Level)){
                costText.text = $"{_data.Cost}";
                costImage.SetActive(true);
                image.color = unlockedColor;
                background.color = bgUnlockableColor;
            }
            else
            {
                costText.text = $"???";
                costImage.SetActive(false);
                image.color = lockedColor;
                background.color = unlockableColor;
            }
        }
        
        if(ServiceLocator.Instance.PlayerManager.CanAfford(_data.Cost) && ServiceLocator.Instance.PlayerManager.IsUpgradeAvailable(Type, Level))
        {
            buyButton.interactable = true;
        }
        else
        {
            buyButton.interactable = false;
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.Show(_data);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.Hide();
    }
}
