using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button hubButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private MenuStaggerAnimation stagger;

    private void Awake()
    {
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
            SceneManager.LoadScene("HubScene");
            });
        });
        quitButton.onClick.AddListener( () =>
        {
            stagger.CloseMenu(() =>
            {
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
