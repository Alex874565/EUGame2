using TMPro;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SolvableObjectSFX))]
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
    
    private SolvableObjectSFX _objectSfx;

    private void Awake()
    {
        _objectSfx = GetComponent<SolvableObjectSFX>();
        startPos = new Vector2(0, -Screen.height); // below screen
        endPos = new Vector2(0, Screen.height);    // above screen
    }

    private void Start()
    {
        Hide();
    }

    public void Show(bool completed)
    {
        string message = completed ? "Task Completed!" : "Task Failed!";
        
        if(completed)
            _objectSfx.PlaySolveSFX();
        else
            _objectSfx.PlayUnsolveSFX();
        
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