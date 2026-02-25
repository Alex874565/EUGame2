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

        // Create the sequence and EXPLICITLY set it to be unscaled
        currentSequence = DOTween.Sequence().SetUpdate(true); 

        for (int i = 0; i < buttons.Length; i++)
        {
            RectTransform button = buttons[i];
            button.localScale = Vector3.zero;

            // You can keep SetUpdate(true) here, but the Sequence.SetUpdate(true) 
            // is the most important part for the stagger timing to work.
            currentSequence.Insert(i * staggerDelay,
                button.DOScale(1f, duration)
                    .SetEase(Ease.OutBack));
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