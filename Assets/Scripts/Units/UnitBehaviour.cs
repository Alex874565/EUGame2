using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [field: SerializeField] public UnitType Type { get; private set; }
    [field: Header("UI Elements")]
    [SerializeField] private TMP_Text counter;
    [SerializeField] private GameObject counterBg;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text cost;
    [Header("Interaction")]
    [SerializeField] private float hoverScaleMultiplier = 1.2f;
    [SerializeField] private float clickScaleMultiplier = .8f;
    
    public EmergencyBehaviour OwningEmergency { get; set; } = null;
    public bool IsIncoming { get; set; } =  false;
    
    private Vector3 _originalScale;
    
    public UnitData Data { get; private set; }

    public int Count { get; private set; }

    public bool IsInteractable { get; set; }
    
    private void Awake()
    {
        _originalScale = gameObject.transform.localScale;
        
        IsInteractable = true;
    }

    public void Start()
    {
        Data = ServiceLocator.Instance.UnitsDatabase.Units.Find(unit => unit.Data.Type == Type).Data;
        cost.text = $"{Data.MovementCost}€";
    }
    
    public void UpdateCount(int count)
    {
        Count = count;
        if (OwningEmergency == null || GetComponent<PlacementController>() != null || GetComponent<MovementController>() != null)
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
                if (Count > 0)
                {
                    image.gameObject.SetActive(true);
                    counterBg.SetActive(true);
                    counter.text = $"{Count}";
                    counter.color = new Color(0.6f, .6f, 0.2f);
                }
                else
                {
                    counterBg.SetActive(false);
                    image.gameObject.SetActive(false);
                }
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

    private void OnDestroy()
    {
        if (ServiceLocator.Instance != null)
        {
            if (ServiceLocator.Instance.CursorManager.HoveredObject == gameObject)
            {
                ServiceLocator.Instance.CursorManager.HoveredObject = null;
            }
            ServiceLocator.Instance.UnitsManager.ActiveUnits.Remove(gameObject);
        }
    }
    
    #region Interaction
    
    private IEnumerator ClickCoroutine()
    {
        gameObject.transform.localScale = _originalScale * clickScaleMultiplier;
        yield return new WaitForSeconds(0.1f);
        if (ServiceLocator.Instance.CursorManager.HoveredObject == gameObject)
        {
            gameObject.transform.localScale = _originalScale * hoverScaleMultiplier;
        }
        else
        {
            gameObject.transform.localScale = _originalScale;
        }
    }

    public void OnHoverEnter()
    {
        ServiceLocator.Instance.CursorManager.HoveredObject = gameObject;
        if (Count > 0 && IsInteractable)
        {
            GameObject unitInPlacing = ServiceLocator.Instance.PlacementManager.UnitInPlacing;
            if (unitInPlacing == null)
            {
                gameObject.transform.localScale = _originalScale * hoverScaleMultiplier;
            }
            else
            {
                EmergencyBehaviour unitInPlacingEmergency = unitInPlacing.GetComponent<UnitBehaviour>().OwningEmergency;
                if (OwningEmergency == unitInPlacingEmergency)
                {
                    gameObject.transform.localScale = _originalScale * hoverScaleMultiplier;
                }
                else
                {
                    gameObject.transform.localScale = _originalScale;
                }
            }
        }
    }

    public void OnHoverExit()
    {
        ServiceLocator.Instance.CursorManager.HoveredObject = null;
        gameObject.transform.localScale = _originalScale;
    }
    
    public void OnClick()
    {
        if (Count > 0 && IsInteractable)
        {
            GameObject unitInPlacing = ServiceLocator.Instance.PlacementManager.UnitInPlacing;
            if (unitInPlacing == null)
            {
                StartCoroutine(ClickCoroutine());
                LocationName startLocation = OwningEmergency != null ? OwningEmergency.LocationData.Name : ServiceLocator.Instance.PlacementManager.DefaultStartLocation;
                ServiceLocator.Instance.PlacementManager.StartPlacingUnit(Type, startLocation, OwningEmergency);
                UpdateCount(Count - 1);
            }
            else if (unitInPlacing.GetComponent<UnitBehaviour>().Type == Type)
            {
                EmergencyBehaviour unitInPlacingEmergency = unitInPlacing.GetComponent<UnitBehaviour>().OwningEmergency;
                if (OwningEmergency == unitInPlacingEmergency)
                {
                    StartCoroutine(ClickCoroutine());
                    unitInPlacing.GetComponent<UnitBehaviour>()
                        .UpdateCount(unitInPlacing.GetComponent<UnitBehaviour>().Count + 1);
                    UpdateCount(Count - 1);
                }
            }
            else
            {
                EmergencyBehaviour unitInPlacingEmergency = unitInPlacing.GetComponent<UnitBehaviour>().OwningEmergency;
                if (OwningEmergency == unitInPlacingEmergency)
                {
                    StartCoroutine(ClickCoroutine());
                    ServiceLocator.Instance.PlacementManager.ClearPlacement();
                    LocationName startLocation = OwningEmergency != null
                        ? OwningEmergency.LocationData.Name
                        : ServiceLocator.Instance.PlacementManager.DefaultStartLocation;
                    ServiceLocator.Instance.PlacementManager.StartPlacingUnit(Type, startLocation, OwningEmergency);
                    
                    UpdateCount(Count - 1);
                }
            }
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
    
    #endregion
}