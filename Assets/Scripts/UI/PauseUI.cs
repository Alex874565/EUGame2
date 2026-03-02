using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button hubButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private MenuStaggerAnimation stagger;

    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            ServiceLocator.Instance.GameManager.ResumeGame();
            Hide();
            
        });
        closeButton.onClick.AddListener(() =>
        {
            Hide();
            ServiceLocator.Instance.GameManager.ResumeGame();
        });
        settingsButton.onClick.AddListener( () =>
        {
            ServiceLocator.Instance.UIManager.SettingsUI.Show();
        });
        mainMenuButton.onClick.AddListener( () =>
        {
            stagger.CloseMenu(() =>
            {
            SceneManager.LoadScene("MainMenuScene");
            });
        });
        hubButton.onClick.AddListener( () =>
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
                SceneManager.LoadScene("HubScene");
            });
        });
        quitButton.onClick.AddListener( () =>
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
                Debug.Log("quit clicked");
                Application.Quit();
            });
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
    }

    public void Hide()
    {
        stagger.CloseMenu(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
