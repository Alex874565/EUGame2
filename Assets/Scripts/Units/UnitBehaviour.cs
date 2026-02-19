using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitBehaviour : MonoBehaviour, IInteractable, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [field: SerializeField] public UnitType Type { get; private set; }
    [field: Header("UI Elements")]
    [SerializeField] private TMP_Text counter;
    [SerializeField] private Image image;
    [SerializeField] private float hoverScaleMultiplier = 1.2f;
    
    private Vector3 _originalScale;
    
    public UnitData Data { get; private set; }

    public int Count { get; private set; }
    
    private void Start()
    {
        Data = ServiceLocator.Instance.UnitsDatabase.Units.Find(unit => unit.Data.Type == Type).Data;
        _originalScale = gameObject.transform.localScale;
    }
    
    public void UpdateCount(int count)
    {
        Count = count;
        counter.text = Count.ToString();
        
        image.color = Count > 0 ? Color.white : Color.black;
        counter.gameObject.SetActive(Count > 1);
    }

    public void OnHoverEnter()
    {
        if (Count > 0)
        {
            gameObject.transform.localScale *= hoverScaleMultiplier;
        }
    }

    public void OnHoverExit()
    {
        gameObject.transform.localScale = _originalScale;
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
        if (Count > 0)
        {
            GameObject unitInPlacing = ServiceLocator.Instance.PlacementManager.UnitInPlacing;
            if (unitInPlacing == null)
            {
                ServiceLocator.Instance.PlacementManager.StartPlacingUnit(Type);
            }
            else
            {
               unitInPlacing.GetComponent<UnitBehaviour>().UpdateCount(unitInPlacing.GetComponent<UnitBehaviour>().Count + 1); 
            }

            UpdateCount(Count - 1);
        }
        return;
    }
}