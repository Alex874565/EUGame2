using UnityEngine;
using UnityEngine.UI;

public class UnitShopUI : MonoBehaviour
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

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
