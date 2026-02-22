using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button hubButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        settingsButton.onClick.AddListener( () =>
        {
            ServiceLocator.Instance.UIManager.SettingsUI.Show();
        });
        mainMenuButton.onClick.AddListener( () =>
        {
            SceneManager.LoadScene("MainMenuScene");
        });
        hubButton.onClick.AddListener( () =>
        {
            SceneManager.LoadScene("HubScene");
        });
        quitButton.onClick.AddListener( () =>
        {
            Debug.Log("quit clicked");
            Application.Quit();
        });
    }

    private void Start()
    {
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
