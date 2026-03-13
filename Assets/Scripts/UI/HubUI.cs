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
    [SerializeField] private TextMeshProUGUI shopText;
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
        ServiceLocator.Instance.GameManager.ResumeGame();
        if(ServiceLocator.Instance.GameManager.WaveIndex == 26)
        {
            ServiceLocator.Instance.GameManager.WonLastWave = false;
        }
        if(ServiceLocator.Instance.GameManager.WaveIndex == 0 && ServiceLocator.Instance.GameManager.WonLastWave)
        {
            gameButton.GetComponentInChildren<TextMeshProUGUI>().text = "START WAVE";
            passedText.text = "-";
            passedText.color = Color.black;
        }
        else if (ServiceLocator.Instance.GameManager.WonLastWave)
        {
            gameButton.GetComponentInChildren<TextMeshProUGUI>().text = "NEXT WAVE";
            passedText.text = "PASSED";
            passedText.color = new Color(0f, .5882353f, 0f);
        }
        else
        {
            gameButton.GetComponentInChildren<TextMeshProUGUI>().text = "RETRY WAVE";
            passedText.text = "FAILED";
            passedText.color = new Color(.5882353f, 0f, 0f);
        }

        stagger.OpenMenu();
        ServiceLocator.Instance.DialogueManager.TryShowDialogue();
        if (ServiceLocator.Instance.GameManager.WaveIndex >= 18)
        {       
            shopButton.enabled = true;
            shopButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            shopText.text = "Unit Shop";
        }
        else
        {
            shopButton.interactable = false;
            shopButton.GetComponent<Image>().color = new Color(.5f, .5f, .5f, .5f);
            shopText.text = "Unlocks in 2019";
        }

        ServiceLocator.Instance.AudioManager.PlayMenuMusic();
    }

}
