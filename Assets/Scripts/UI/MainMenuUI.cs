using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private MenuStaggerAnimation stagger;

    private void Awake()
    {
        newGameButton.onClick.AddListener(() =>
        {
            stagger.CloseMenu(() =>
            {
            SceneManager.LoadScene("HubScene");
            // todo load save
            });
        });
        continueButton.onClick.AddListener(() =>
        {
            stagger.CloseMenu(() =>
            {
            SceneManager.LoadScene("HubScene");
            // todo load save
            });
        });
        settingsButton.onClick.AddListener(() =>
        {
            ServiceLocator.Instance.UIManager.SettingsUI.Show();
        });
        quitButton.onClick.AddListener(() =>
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
        stagger.OpenMenu();
    }

    public void HideMenu()
    {
        stagger.CloseMenu();
    }
}
