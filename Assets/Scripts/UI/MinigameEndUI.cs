using TMPro;
using UnityEngine;
using DG.Tweening;

public class MinigameEndUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI taskStatusText;
    [SerializeField] private RectTransform rect;

    [Header("Animation")]
    [SerializeField] private float flyInDuration = 0.6f;
    [SerializeField] private float stayDuration = 1.5f;
    [SerializeField] private float flyOutDuration = 0.6f;

    private Vector2 centerPos = Vector2.zero;
    private Vector2 startPos;
    private Vector2 endPos;

    private void Awake()
    {
        startPos = new Vector2(0, -Screen.height); // below screen
        endPos = new Vector2(0, Screen.height);    // above screen
    }

    private void Start()
    {
        //Hide();
        ServiceLocator.Instance.UIManager.MinigameEndUI.Show("Task Failed!");
    }

    public void Show(string message)
    {
        taskStatusText.text = message;
        gameObject.SetActive(true);

        rect.anchoredPosition = startPos;

        Sequence seq = DOTween.Sequence();

        seq.Append(rect.DOAnchorPos(centerPos, flyInDuration)
            .SetEase(Ease.OutBack))

           .AppendInterval(stayDuration)

           .Append(rect.DOAnchorPos(endPos, flyOutDuration)
            .SetEase(Ease.InBack))

           .OnComplete(Hide);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}