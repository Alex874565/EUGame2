using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        newGameButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("HubScene");
            // todo delete old save
        });
        continueButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("HubScene");
            // todo load save
        });
        settingsButton.onClick.AddListener(() =>
        {
            ServiceLocator.Instance.UIManager.SettingsUI.Show();
        });
        quitButton.onClick.AddListener(() =>
        {
            Debug.Log("quit clicked");
            Application.Quit();
        });
    }

}
