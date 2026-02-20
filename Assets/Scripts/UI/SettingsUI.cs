using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    private void Awake()
    {
        closeButton.onClick.AddListener( () =>
        {
           Hide(); 
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

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
