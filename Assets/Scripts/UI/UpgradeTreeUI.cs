using UnityEngine;
using UnityEngine.UI;

public class UpgradeTreeUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private MenuStaggerAnimation stagger;
    private void Awake()
    {
        closeButton.onClick.AddListener( () =>
        {
           Hide(); 
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
