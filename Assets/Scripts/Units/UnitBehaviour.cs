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
    [SerializeField] private GameObject counterBg;
    [SerializeField] private Image image;
    [SerializeField] private float hoverScaleMultiplier = 1.2f;
    
    public EmergencyBehaviour OwningEmergency { get; set; } = null;
    public bool IsIncoming { get; set; } =  false;
    
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
        if (OwningEmergency == null)
        {
            counter.text = Count.ToString();

            image.color = Count > 0 ? Color.white : Color.black;
            counter.gameObject.SetActive(Count > 1);
            counterBg.SetActive(Count > 1);
        }
        else
        {
            if (IsIncoming)
            {
                counter.text = $"{Count}";
                counter.color = new Color(0.6f, .6f, 0.2f);
            }
            else
            {
                int requiredUnits = OwningEmergency.EmergencyData.RequiredResources
                    .Find(resource => resource.Type == Type).Amount;
                counter.text = $"{Count}/{requiredUnits}";
                counter.color = Count >= requiredUnits ? new Color(0.2f, .6f, 0.2f) : new Color(.6f, 0.2f, 0.2f);
            }
        }
    }

    public void OnHoverEnter()
    {
        ServiceLocator.Instance.CursorManager.HoveredObject = gameObject;
        if (Count > 0 && GetComponent<PlacementController>() == null)
        {
            gameObject.transform.localScale *= hoverScaleMultiplier;
        }
    }

    public void OnHoverExit()
    {
        ServiceLocator.Instance.CursorManager.HoveredObject = null;
        gameObject.transform.localScale = _originalScale;
    }
    
    public void OnClick()
    {
        if (Count > 0 && IsIncoming == false)
        {
            GameObject unitInPlacing = ServiceLocator.Instance.PlacementManager.UnitInPlacing;
            if (unitInPlacing == null)
            {
                ServiceLocator.Instance.PlacementManager.StartPlacingUnit(Type, ServiceLocator.Instance.PlacementManager.DefaultStartLocation);
            }
            else if (unitInPlacing.GetComponent<UnitBehaviour>().Type == Type)
            {
                unitInPlacing.GetComponent<UnitBehaviour>().UpdateCount(unitInPlacing.GetComponent<UnitBehaviour>().Count + 1); 
            }
            else
            {
                ServiceLocator.Instance.PlacementManager.ClearPlacement();
                ServiceLocator.Instance.PlacementManager.StartPlacingUnit(Type, OwningEmergency.LocationData.Name);
            }

            UpdateCount(Count - 1);
        }
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
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            OnClick();
        }
    }
}