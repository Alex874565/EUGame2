using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class EmergencyBehaviour : MonoBehaviour, IInteractable, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private EmergencyType emergencyType;
    [Header("UI")] 
    [SerializeField] private Image image;
    [field: SerializeField] public Image FillImage { get; private set; }
    [field: SerializeField] public TMP_Text TimerText { get; private set; }
    [SerializeField] private GameObject _activeUnitsContainer;
    
    [field: Header("Interaction")]
    [SerializeField] private float hoverScaleMultiplier = 1.2f;
    [SerializeField] private float selectedScaleMultiplier = 1.4f;
    [SerializeField] private float clickScaleMultiplier = .8f;
    
    public bool IsSelected { get; set; }
    
    public LocationData LocationData { get; set; }
    
    public EmergencyData EmergencyData { get; private set; }

    public List<RequiredUnitData> AvailableUnits
    {
        get
        {
            List<RequiredUnitData> availableUnits = new List<RequiredUnitData>();
            foreach (var kvp in _activeUnits)
            {
                availableUnits.Add(new RequiredUnitData { Type = kvp.Key, Amount = kvp.Value });
            }
            return availableUnits;
        }
    }
    
    public List<RequiredUnitData> IncomingUnits
    {
        get
        {
            List<RequiredUnitData> incomingUnits = new List<RequiredUnitData>();
            foreach (var kvp in _incomingUnits)
            {
                incomingUnits.Add(new RequiredUnitData { Type = kvp.Key, Amount = kvp.Value });
            }
            return incomingUnits;
        }
    }
    
    private Dictionary<UnitType, int> _activeUnits;
    private Dictionary<UnitType, int> _incomingUnits;
    
    public bool IsSolving { get; set; }
    
    public float ExpirationTimeLeft { get; set; }
    public float SolvingTimeLeft { get; set; }
    
    private EmergencyStateMachine _emergencyStateMachine;
    
    private Vector3 _originalScale;
    
    private void Start()
    {
        _originalScale = image.transform.localScale;
        _activeUnits = new  Dictionary<UnitType, int>();
        _incomingUnits = new Dictionary<UnitType, int>();
        EmergencyData = ServiceLocator.Instance.EmergenciesManager.EmergenciesStats[emergencyType];
        ServiceLocator.Instance.EmergenciesManager.ActiveEmergencies.Add(gameObject);
        foreach (RequiredUnitData resourceData in EmergencyData.RequiredResources)
        {
            _activeUnits.Add(resourceData.Type, 0);
        }

        ExpirationTimeLeft = EmergencyData.TimeUntilExpiry;
        SolvingTimeLeft = EmergencyData.TimeToSolve;

        _emergencyStateMachine = new EmergencyStateMachine(this);
    }

    private void Update()
    {
        _emergencyStateMachine.Update();
    }

    public void Solve()
    {
        if (IsSelected)
        {
            ServiceLocator.Instance.CursorManager.SelectObject(null);
        }
        ReturnInventoryUnits();
        Destroy(gameObject);
    }

    public void Expire()
    {
        if(IsSelected)
        {
            ServiceLocator.Instance.CursorManager.SelectObject(null);
        }
        ReturnInventoryUnits();
        ServiceLocator.Instance.WavesManager.UpdateEmergenciesFailed(ServiceLocator.Instance.WavesManager.CurrentEmergenciesFailed + 1);
        Destroy(gameObject);
    }
    
    #region Unit Checks
    
    public bool HasAllRequiredUnits()
    {
        foreach (RequiredUnitData requiredUnitData in EmergencyData.RequiredResources)
        {
            if (_activeUnits[requiredUnitData.Type] < requiredUnitData.Amount)
            {
                return false;
            }
        }

        return true;
    }
    
    public bool AcceptsUnitsOfType(UnitType unitType)
    {
        return RequiredUnitsOfType(unitType) > 0;
    }
    
    public int RequiredUnitsOfType(UnitType unitType)
    {
        RequiredUnitData requiredUnitData = EmergencyData.RequiredResources.Find(resource => resource.Type == unitType);
        
        int incomingAmount = _incomingUnits.ContainsKey(unitType) ? _incomingUnits[unitType] : 0;
        int activeAmount = _activeUnits.ContainsKey(unitType) ? _activeUnits[unitType] : 0;
        int requiredAmount = requiredUnitData != null ? requiredUnitData.Amount : 0;

        return requiredAmount - activeAmount - incomingAmount;
    }
    
    #endregion
    
    #region Unit Management

    private void ReturnInventoryUnits()
    {
        foreach (var kvp in _activeUnits)
        {
            if (kvp.Value > 0)
            {
                int currentInventoryCount = ServiceLocator.Instance.UnitsManager.InventoryUnits.Find(unit => unit.GetComponent<UnitBehaviour>().Type == kvp.Key)
                    .GetComponent<UnitBehaviour>().Count;
                ServiceLocator.Instance.UnitsManager.SetInventoryUnitCount(kvp.Key, currentInventoryCount + kvp.Value);
            }
        }
    }
    
    public void SetActiveUnits(UnitType unitType, int amount)
    {
        if (_activeUnits.ContainsKey(unitType))
        {
            _activeUnits[unitType] = amount;
        }
        else
        {
            _activeUnits.Add(unitType, amount);
        }
        
        if (!IsSolving && HasAllRequiredUnits())
        {
            IsSolving = true;
        }
        else if (IsSolving && !HasAllRequiredUnits())
        {
            IsSolving = false;
        }

        if (IsSelected)
        {
            ServiceLocator.Instance.UIManager.OpenEmergencyDetails(this);
        }

        foreach (var child in _activeUnitsContainer.transform)
        {
            Destroy(((Transform)child).gameObject);
        }
        foreach(var kvp in _activeUnits)
        {
            if (kvp.Value > 0)
            {
                GameObject unit = ServiceLocator.Instance.UnitsManager.UnitFactory.SpawnUnit(kvp.Key);
                unit.transform.SetParent(_activeUnitsContainer.transform, false);
                unit.GetComponent<UnitBehaviour>().UpdateCount(kvp.Value);
                unit.GetComponent<UnitBehaviour>().IsInteractable = false;
                unit.GetComponentInChildren<Image>().raycastTarget = false;
            }
        }
    }
    
    public void SetIncomingUnits(UnitType unitType, int amount)
    {
        if (_incomingUnits.ContainsKey(unitType))
        {
            _incomingUnits[unitType] = amount;
        }
        else
        {
            _incomingUnits.Add(unitType, amount);
        }
        
        if(IsSelected)
        {
            ServiceLocator.Instance.UIManager.OpenEmergencyDetails(this);
        }
    }
    
    public int GetActiveUnitsOfType(UnitType unitType)
    {
        return _activeUnits.ContainsKey(unitType) ? _activeUnits[unitType] : 0;
    }
    
    public int GetIncomingUnitsOfType(UnitType unitType)
    {
        return _incomingUnits.ContainsKey(unitType) ? _incomingUnits[unitType] : 0;
    }

    #endregion
    
    #region Interactions

    public void Select()
    {
        IsSelected = true;
        ServiceLocator.Instance.UIManager.OpenEmergencyDetails(this);
        StartCoroutine(SelectCoroutine());
    }

    public IEnumerator SelectCoroutine()
    {
        image.transform.localScale = _originalScale * clickScaleMultiplier;
        yield return new WaitForSeconds(0.1f);
        image.transform.localScale = _originalScale * selectedScaleMultiplier; 
    }
    
    public void Deselect()
    {
        image.transform.localScale = _originalScale;
        IsSelected = false;
        ServiceLocator.Instance.UIManager.EmergencyDetailsMenu.SetActive(false);
    }
    
    public void ReactToUnitHover(GameObject unit)
    {
        UnitType unitType = unit.GetComponent<UnitBehaviour>().Type;
        if (AcceptsUnitsOfType(unitType) && (Vector2)transform.position != unit.GetComponent<PlacementController>().StartPosition)
        {
            image.transform.localScale *= hoverScaleMultiplier;
        }
        else
        {
            image.transform.localScale = _originalScale;
        }
    }

    public void OnHoverEnter()
    {
        ServiceLocator.Instance.CursorManager.HoveredObject = gameObject;

        GameObject unitInPlacing = ServiceLocator.Instance.PlacementManager.UnitInPlacing;
        if (unitInPlacing != null)
        {
            ReactToUnitHover(unitInPlacing);
        }
        else
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