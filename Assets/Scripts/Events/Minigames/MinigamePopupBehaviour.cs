using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using DG.Tweening;

[RequireComponent(typeof(SelectableObjectSFX))]
public class MinigamePopupBehaviour : MonoBehaviour, IInteractable, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [field: SerializeField] public MinigameType Type { get; private set; }
    
    [Header("UI Elements")]
    [SerializeField] private Image image;
    
    [Header("Interaction")]
    [SerializeField] private float hoverScaleMultiplier = 1.2f;
    [SerializeField] private float selectedScaleMultiplier = 1.4f;
    [SerializeField] private float clickScaleMultiplier = .8f;
    
    public bool IsSelected { get; set; }
    
    public MinigameData Data { get; private set; }

    private Vector3 _originalScale;
    
    private SelectableObjectSFX _objectSfx;
    
    private void Start()
    {
        _objectSfx = GetComponent<SelectableObjectSFX>();
        Data = ServiceLocator.Instance.MinigamesManager.MinigamesData[Type];
        ServiceLocator.Instance.MinigamesManager.ActiveMinigames.Add(gameObject);
        IsSelected = false;
        _originalScale = image.transform.localScale;
        ServiceLocator.Instance.MinigamesManager.MinigameInPlay = this;
        
        DOTween.Sequence()
            .Append(transform.DOScale(_originalScale * 1.5f, .25f).SetEase(Ease.OutCubic))
            .Append(transform.DOScale(_originalScale, .1f).SetEase(Ease.InOutQuad));
        
        _objectSfx.PlayAppearSFX();
    }

    public void Update()
    {
        return;
    }

    public void DestroySelf()
    {
        ServiceLocator.Instance.MinigamesManager.ActiveMinigames.Remove(gameObject);
        Destroy(gameObject);
    }

    #region Interactions

    public void Select()
    {
        if(!IsSelected){
            IsSelected = true;
            ServiceLocator.Instance.UIManager.OpenMinigameDetails(this);
            StartCoroutine(SelectCoroutine());
            _objectSfx.PlaySelectSFX();
        }
    }

    private IEnumerator SelectCoroutine()
    {
        image.transform.localScale = _originalScale * clickScaleMultiplier;
        yield return new WaitForSeconds(0.1f);
        image.transform.localScale = _originalScale * selectedScaleMultiplier; 
    }
    
    public void Deselect()
    {
        if (IsSelected)
        {
            image.transform.localScale = _originalScale;
            IsSelected = false;
            ServiceLocator.Instance.UIManager.MinigameDetailsMenu.SetActive(false);
            _objectSfx.PlayDeselectSFX();
        }
    }

    public void OnHoverEnter()
    {
        ServiceLocator.Instance.CursorManager.HoveredObject = gameObject;
        GameObject unitInPlacing = ServiceLocator.Instance.PlacementManager.UnitInPlacing;
        if (unitInPlacing == null)
        {
            image.transform.localScale = _originalScale * hoverScaleMultiplier;
        }
    }

    public void OnHoverExit()
    {
        if (!IsSelected)
        {
            image.transform.localScale = _originalScale;
        }
        ServiceLocator.Instance.CursorManager.HoveredObject = null;
    }

    public void OnClick()
    {
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHoverEnter();
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        OnHoverExit();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnClick();
        }
    }
    
    #endregion
    
}