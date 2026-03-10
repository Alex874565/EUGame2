using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D), typeof(SelectableObjectSFX))]
public class TrashItemBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("UI Settings")]
    [SerializeField] private float hoverScaleMultiplier = 1.1f;
    [SerializeField] private float selectedScaleMultiplier = 1.2f;
    
    private bool _isSelected;
    private Vector3 _originalScale;
    private Canvas _canvas;
    private Rigidbody2D _rb;
    
    private SelectableObjectSFX _objectSfx;

    private void Awake()
    {
        _objectSfx = GetComponent<SelectableObjectSFX>();
        _originalScale = transform.localScale;
        _canvas = GetComponentInParent<Canvas>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!_isSelected) return;

        Camera cam = _canvas.worldCamera; // assign your camera-space canvas camera

        Vector3 mouse = Input.mousePosition;

        // IMPORTANT: set Z to distance from camera to the UI plane
        float z = Mathf.Abs(cam.transform.position.z - transform.position.z);
        mouse.z = z;

        _rb.MovePosition(cam.ScreenToWorldPoint(mouse));
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isSelected) return;
        transform.localScale *= hoverScaleMultiplier;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isSelected) return;
        transform.localScale = _originalScale;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        _isSelected = true;
        _objectSfx.PlaySelectSFX();
        transform.localScale *= selectedScaleMultiplier;
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        _objectSfx.PlayDeselectSFX();
        _isSelected = false;
        transform.localScale = _originalScale * hoverScaleMultiplier;
    }
    
    
}