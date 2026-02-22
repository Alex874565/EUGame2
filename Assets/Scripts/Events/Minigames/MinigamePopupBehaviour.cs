using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

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
    private void Start()
    {
        Data = ServiceLocator.Instance.MinigamesDatabase.Minigames.Find(mg => mg.Type == Type);
        ServiceLocator.Instance.MinigamesManager.ActiveMinigames.Add(gameObject);
        IsSelected = false;
        _originalScale = image.transform.localScale;
    }

    public void Update()
    {
        return;
    }

    private void DestroySelf()
    {
        ServiceLocator.Instance.MinigamesManager.ActiveMinigames.Remove(gameObject);
        Destroy(gameObject);
    }

    #region Interactions

    public void Select()
    {
        IsSelected = true;
        ServiceLocator.Instance.UIManager.OpenMinigameDetails(this);
        StartCoroutine(SelectCoroutine());
    }

    private IEnumerator SelectCoroutine()
    {
        image.transform.localScale = _originalScale * clickScaleMultiplier;
        yield return new WaitForSeconds(0.1f);
        image.transform.localScale = _originalScale * selectedScaleMultiplier; 
    }
    
    public void Deselect()
    {
        image.transform.localScale = _originalScale;
        IsSelected = false;
        ServiceLocator.Instance.UIManager.MinigameDetailsMenu.SetActive(false);
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
        if (!ServiceLocator.Instance.PlacementManager.UnitInPlacing)
        {
            ServiceLocator.Instance.CursorManager.SelectObject(gameObject);
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter");
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