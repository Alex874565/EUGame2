using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeTreeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private Button closeButton;
    [SerializeField] private MenuStaggerAnimation stagger;
    private void Awake()
    {
        closeButton.onClick.AddListener( () =>
        {
           Hide(); 
        });
    }
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        stagger.OpenMenu(); 
        UpdateMoneyText();
    }

    public void Hide()
    {
        stagger.CloseMenu(() =>
        {
            gameObject.SetActive(false);
        });
    }
    
    public void UpdateMoneyText()
    {
        moneyText.text = ServiceLocator.Instance.PlayerManager.Money.ToString();
    }
}
