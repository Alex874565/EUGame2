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
                ServiceLocator.Instance.SaveManager.DeleteSaveFile();
                SceneManager.LoadScene("HubScene");
            });
        });
        continueButton.onClick.AddListener(() =>
        {
            stagger.CloseMenu(() =>
            {
                ServiceLocator.Instance.SaveManager.LoadGame();
                SceneManager.LoadScene("HubScene");
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
        if(!ServiceLocator.Instance.SaveManager.HasSaveFile())
        {
            continueButton.gameObject.SetActive(false);
        }
        stagger.OpenMenu();
    }

    public void HideMenu()
    {
        stagger.CloseMenu();
    }
}
