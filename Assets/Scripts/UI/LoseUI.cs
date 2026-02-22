using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseUI : MonoBehaviour
{
    [SerializeField] Button hubButton;
    private void Awake()
    {
        hubButton.onClick.AddListener( () =>
        {
            SceneManager.LoadScene("HubScene");
        });
    }
    private void Start()
    {
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
