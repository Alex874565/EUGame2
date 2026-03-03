using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.PlayerLoop;

public class UnitShopUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private Button closeButton;
    [SerializeField] private MenuStaggerAnimation stagger;
    [SerializeField] private List<ShopUnitBehaviour> shopUnits;
    
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
        foreach (ShopUnitBehaviour shopUnit in shopUnits)
        {
            shopUnit.OnUnitPurchased += (sender, args) =>
            {
                UpdateUIs();
            };
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        stagger.OpenMenu();
        UpdateUIs();
    }

    public void Hide()
    {
        stagger.CloseMenu(() =>
        {
            gameObject.SetActive(false);
        });
    }
    
    public void UpdateUIs()
    {
        moneyText.text = ServiceLocator.Instance.PlayerManager.Money.ToString();

        foreach (ShopUnitBehaviour shopUnit in shopUnits)
        {
            shopUnit.UpdateUI();
        }
    }
}
