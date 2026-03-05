using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class UIButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Tween currentTween;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(GetComponent<Button>() != null && !GetComponent<Button>().interactable)
            return;
        
        currentTween?.Kill();

        currentTween = transform.DOScale(1.08f, 0.2f)
                                .SetEase(Ease.OutBack).SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        currentTween?.Kill();

        currentTween = transform.DOScale(1f, 0.2f)
                                .SetEase(Ease.OutCubic).SetUpdate(true);
    }
}