using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeBehaviour : MonoBehaviour
{
    [field: SerializeField] public UpgradeType Type { get; set; }
    [field: SerializeField] public int Level { get; set; }

    [Header("UI Elements")] 
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private GameObject costImage;
    [SerializeField] private Button buyButton;
    [Header("Colors")]
    [SerializeField] private Color unlockedColor = Color.white;
    [SerializeField] private Color unlockableColor = Color.gray;
    [SerializeField] private Color lockedColor = Color.black;
    
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
        }
        else
        {
            if(ServiceLocator.Instance.PlayerManager.IsUpgradeAvailable(Type, Level)){
                costText.text = $"{_data.Cost}";
                costImage.SetActive(true);
                image.color = unlockableColor;
            }
            else
            {
                costText.text = $"???";
                costImage.SetActive(false);
                image.color = lockedColor;
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
}
