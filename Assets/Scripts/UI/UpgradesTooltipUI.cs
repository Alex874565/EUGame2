using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class UpgradesTooltipUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI modifiersText;

    private RectTransform _rectTransform;
    private Image _background;
    
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _background = GetComponentInChildren<Image>();
    }
    
    private void Start()
    {
        Hide();
    }
    
    public void Initialize(UpgradeData upgradeData)
    {
        nameText.text = upgradeData.Name;
        descriptionText.text = upgradeData.Description;
        modifiersText.text = upgradeData.ModifiersDescription;
    }
    
    public void Show(UpgradeData upgradeData, Vector2 position, float upgradeHeight)
    {
        if (Input.mousePosition.y > Screen.height / 2)
        {
            _rectTransform.pivot = new Vector2(0.5f, 1f);
            _background.rectTransform.localScale = new  Vector3(1f, -1f, 1f);
            upgradeHeight *= -1;
        }
        else
        {
            _rectTransform.pivot = new Vector2(0.5f, 0f);
            _background.rectTransform.localScale = new Vector3(1f, 1f, 1f);
        }
        transform.position = position + new Vector2(0, upgradeHeight/2);
        Initialize(upgradeData);
        gameObject.SetActive(true);
        transform.localScale = new Vector3(1, 0, 1); // Start squished vertically
        DOTween.Sequence()
            .Append(transform.DOScaleY(1.25f, .2f).SetEase(Ease.OutCubic))
            .Append(transform.DOScaleY(1, .1f).SetEase(Ease.InOutQuad));
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}