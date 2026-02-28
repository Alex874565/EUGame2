using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class HubUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI passedText;
    [SerializeField] private Button upgradesButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button gameButton;
    [SerializeField] private MenuStaggerAnimation stagger;

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
            stagger.CloseMenu(() =>
            {
                ServiceLocator.Instance.SaveManager.SaveGame(new SaveData(
                    ServiceLocator.Instance.GameManager.WaveIndex,
                    ServiceLocator.Instance.GameManager.WonLastWave,
                    ServiceLocator.Instance.PlayerManager.Money,
                    ServiceLocator.Instance.PlayerManager.OwnedUpgrades,
                    ServiceLocator.Instance.PlayerManager.StartingUnits
                ));      
                SceneManager.LoadScene("MainMenuScene");
            });
        });
        gameButton.onClick.AddListener( () =>
        {
            stagger.CloseMenu(() =>
            {
                ServiceLocator.Instance.GameManager.StartWave();
            });
        });
    }

    private void Start()
    {
        if(ServiceLocator.Instance.GameManager.WaveIndex == 0 && ServiceLocator.Instance.GameManager.WonLastWave)
        {
            passedText.text = "-";
            passedText.color = Color.black;
        }
        else if (ServiceLocator.Instance.GameManager.WonLastWave)
        {
            gameButton.GetComponentInChildren<TextMeshProUGUI>().text = "Next Wave";
            passedText.text = "PASSED";
            passedText.color = new Color(0f, .5882353f, 0f);
        }
        else
        {
            gameButton.GetComponentInChildren<TextMeshProUGUI>().text = "Retry Wave";
            passedText.text = "FAILED";
            passedText.color = new Color(.5882353f, 0f, 0f);
        }

        stagger.OpenMenu();
        ServiceLocator.Instance.DialogueManager.TryShowDialogue();
        if (ServiceLocator.Instance.GameManager.WaveIndex >= 15)
        {
            shopButton.enabled = true;
        }
        else
        {
            //shopButton.enabled = false;
        }
    }

}
