using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeBehaviour : MonoBehaviour
{
    [field: SerializeField] public UpgradeType Type { get; set; }
    [field: SerializeField] public int Level { get; set; }
    
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI modifiersDescriptionText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private Color unlockedColor;
    [SerializeField] private Color lockedColor;
    
    private UpgradeData _data;
    
    private void Start()
    {
        _data = ServiceLocator.Instance.UpgradesManager.UpgradesData[Type][Level - 1];
        
        InitializeUI();
    }

    public bool TryApplyUpgrade(bool isFree)
    {
        if (isFree || ServiceLocator.Instance.PlayerManager.CanAfford(_data.Cost))
        {
            if (!isFree)
            {
                ServiceLocator.Instance.PlayerManager.SpendMoney(_data.Cost);
            }
            
            ApplyUpgrade();
            return true;
        }

        return false;
    }

    public void ApplyUpgrade()
    {
            foreach (var modifier in _data.EmergencyModifiers)
            {
                ServiceLocator.Instance.EmergenciesManager.ModifyEmergenciesStat(modifier);
            }
            
            foreach (var modifier in _data.MinigameModifiers)
            {
                ServiceLocator.Instance.MinigamesManager.ModifyMinigamesStat(modifier);
            }
    }

    private void InitializeUI()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if(ServiceLocator.Instance.PlayerManager.IsUpgradeOwned(Type, Level))
        {
            nameText.text = _data.Name;
            descriptionText.text = _data.Description;
            modifiersDescriptionText.text = _data.ModifiersDescription;
            costText.text = $"Already Owned";
            lockIcon.SetActive(false);
            GetComponent<UnityEngine.UI.Image>().color = unlockedColor;
        }
        else
        {
            if(ServiceLocator.Instance.PlayerManager.IsUpgradeAvailable(Type, Level)){
                nameText.text = _data.Name;
                descriptionText.text = _data.Description;
                modifiersDescriptionText.text = _data.ModifiersDescription;
                costText.text = $"Cost: {_data.Cost}€";
                lockIcon.SetActive(false);
                GetComponent<Image>().color = unlockedColor;
            }
            else
            {
                nameText.text = "Locked Upgrade";
                descriptionText.text = "Purchase the previous upgrade to unlock this one.";
                modifiersDescriptionText.text = "";
                costText.text = $"Cost: ???";
                lockIcon.SetActive(true);
                GetComponent<Image>().color = lockedColor;
            }
        }
    }
}
