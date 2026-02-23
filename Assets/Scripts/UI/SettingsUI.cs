using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private MenuStaggerAnimation stagger; // assign in inspector

    private void Awake()
    {
        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });
    }

    private void Start()
    {
        // Start hidden
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true); // must activate first for animation to work
        stagger.OpenMenu();          // stagger in buttons, text, etc.
    }

    public void Hide()
    {
        // Close menu with stagger, then deactivate
        stagger.CloseMenu(() =>
        {
            gameObject.SetActive(false);
        });
    }
}