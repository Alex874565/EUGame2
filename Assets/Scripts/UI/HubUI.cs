using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HubUI : MonoBehaviour
{
    [SerializeField] private Button upgradesButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        upgradesButton.onClick.AddListener( () =>
        {
            ServiceLocator.Instance.UIManager.UpgradeTreeUI.Show();
        });
        shopButton.onClick.AddListener( () =>
        {
            ServiceLocator.Instance.UIManager.UnitShopUI.Show();
        });
        settingsButton.onClick.AddListener( () =>
        {
            ServiceLocator.Instance.UIManager.SettingsUI.Show();
        });
        mainMenuButton.onClick.AddListener( () =>
        {
            SceneManager.LoadScene("MainMenuScene");
        });
    }

}
