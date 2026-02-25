using UnityEngine;
using DG.Tweening;

public class MenuStaggerAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform moneyElement;
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

    public void OpenMenu(System.Action onMoneyShown = null)
    {
        currentSequence?.Kill();
        currentSequence = DOTween.Sequence().SetUpdate(true);

        for (int i = 0; i < buttons.Length; i++)
        {
            RectTransform button = buttons[i];
            button.localScale = Vector3.zero;

            Tween scaleTween = button
                .DOScale(1f, duration)
                .SetEase(Ease.OutBack)
                .SetUpdate(true);

            currentSequence.Insert(i * staggerDelay, scaleTween);

            // 👇 If this is the money element, trigger callback
            if (button == moneyElement && onMoneyShown != null)
            {
                scaleTween.OnComplete(() =>
                {
                    onMoneyShown.Invoke();
                });
            }
        }
    }

    public void CloseMenu(System.Action onComplete = null)
    {
        currentSequence?.Kill();

        currentSequence = DOTween.Sequence().SetUpdate(true);

        for (int i = buttons.Length - 1; i >= 0; i--)
        {
            RectTransform button = buttons[i];
            float delay = (buttons.Length - 1 - i) * staggerDelay;

            currentSequence.Insert(delay,
                button.DOScale(0f, duration * 0.6f)
                    .SetEase(Ease.InBack)
                    .SetUpdate(true)); // <-- unscaled
        }

        if (onComplete != null)
            currentSequence.OnComplete(() => onComplete.Invoke());
    }
}