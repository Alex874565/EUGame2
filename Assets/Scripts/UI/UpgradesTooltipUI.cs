using UnityEngine;
using TMPro;

public class UpgradesTooltipUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI modifiersText;
    [Header("Settings")]
    [SerializeField] private int xOffset;
    [SerializeField] private int yOffset;
    
    private void Start()
    {
        Hide();
    }
    
    private void Update()
    {
        transform.position = Input.mousePosition + new Vector3(xOffset, yOffset, 0);
    }
    
    public void Initialize(UpgradeData upgradeData)
    {
        nameText.text = upgradeData.Name;
        descriptionText.text = upgradeData.Description;
        modifiersText.text = upgradeData.ModifiersDescription;
    }
    
    public void Show(UpgradeData upgradeData)
    {
        Initialize(upgradeData);
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}