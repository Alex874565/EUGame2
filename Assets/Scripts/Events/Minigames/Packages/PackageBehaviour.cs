using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SelectableObjectSFX))]
public class PackageBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [field: SerializeField] public bool IsFood { get; private set; }
    [Header("UI Settings")]
    [field: SerializeField] private float _hoverScaleMultiplier = 1.1f;
    [field: SerializeField] private float _clickScaleMultiplier = 0.9f;
    [field: SerializeField] private float _selectedScaleMultiplier = 1.2f;
    
    public UnityAction<PackageBehaviour> OnClick;
    
    public bool IsSelected { get; private set; }
    
    private Vector3 _originalScale;
    
    private bool _isHovered;
    
    private SelectableObjectSFX _objectSfx;

    private void Awake()
    {
        _objectSfx = GetComponent<SelectableObjectSFX>();
        _originalScale = transform.localScale;
        IsSelected = false;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHovered = true;
        transform.localScale *= _hoverScaleMultiplier;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        _isHovered = false;
        if (!IsSelected)
        {
            transform.localScale = _originalScale;
        }
        else
        {
            transform.localScale = _originalScale * _selectedScaleMultiplier;
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(ClickCoroutine());
        OnClick?.Invoke(this);
    }

    private IEnumerator ClickCoroutine()
    {
        IsSelected = !IsSelected;
        transform.localScale *= _clickScaleMultiplier;
        yield return new WaitForSeconds(0.1f);
        if(!IsSelected)
        {
            _objectSfx.PlaySelectSFX();
            transform.localScale = _originalScale * (_isHovered ? _hoverScaleMultiplier : 1f);
        }
        else
        {
            _objectSfx.PlayDeselectSFX();
            transform.localScale = _originalScale * _selectedScaleMultiplier * (_isHovered ? _hoverScaleMultiplier : 1f);
        }
    }

    private void OnDestroy()
    {
        OnClick = null;
    }
    
}