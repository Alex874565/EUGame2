using UnityEngine;
using DG.Tweening;

public class MenuStaggerAnimation : MonoBehaviour
{
    [Header("Buttons To Animate")]
    [SerializeField] private RectTransform[] buttons;

    [Header("Animation Settings")]
    [SerializeField] private float duration = 0.4f;
    [SerializeField] private float staggerDelay = 0.08f;

    private Sequence currentSequence;

    private void Awake()
    {
        // Prepare buttons hidden
        foreach (var button in buttons)
        {
            button.localScale = Vector3.zero;
        }
    }

    public void OpenMenu()
    {
        currentSequence?.Kill();

        currentSequence = DOTween.Sequence();

        for (int i = 0; i < buttons.Length; i++)
        {
            RectTransform button = buttons[i];

            // Ensure starting scale is 0
            button.localScale = Vector3.zero;

            // Only animate scale (no position)
            currentSequence.Insert(i * staggerDelay,
                button.DOScale(1f, duration).SetEase(Ease.OutBack));
        }
    }

    public void CloseMenu(System.Action onComplete = null)
    {
        currentSequence?.Kill();

        currentSequence = DOTween.Sequence();

        for (int i = buttons.Length - 1; i >= 0; i--)
        {
            RectTransform button = buttons[i];
            float delay = (buttons.Length - 1 - i) * staggerDelay;

            currentSequence.Insert(delay,
                button.DOScale(0f, duration * 0.6f).SetEase(Ease.InBack));
        }

        if (onComplete != null)
            currentSequence.OnComplete(() => onComplete.Invoke());
    }
}