using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class EmergencyBehaviour : MonoBehaviour, IInteractable, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private EmergencyType emergencyType;
    [field: Header("UI")]
    [field: SerializeField] public Image FillImage { get; private set; }
    [field: SerializeField] public TMP_Text TimerText { get; private set; }
    [field: Header("Interaction")]
    [SerializeField] private float hoverScaleMultiplier = 1.2f;
    [field: Header("Units UI")]
    [SerializeField] private Transform activeUnitsParent;
    [SerializeField] private Transform incomingUnitsParent;
    
    public LocationData LocationData { get; set; }
    
    public EmergencyData EmergencyData { get; private set; }

    private Dictionary<UnitType, int> _activeUnits;
    private Dictionary<UnitType, int> _incomingUnits;
    
    private Dictionary<UnitType, UnitBehaviour> _activeUIUnits;
    private Dictionary<UnitType, UnitBehaviour> _incomingUIUnits;
    
    public bool IsSolving { get; set; }
    
    public float ExpirationTimeLeft { get; set; }
    public float SolvingTimeLeft { get; set; }
    
    private EmergencyStateMachine _emergencyStateMachine;
    
    private Vector3 _originalScale;
    
    private void Start()
    {
        _originalScale = gameObject.transform.localScale;
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
        
        InitializeUIUnits();
    }

    private void Update()
    {
        _emergencyStateMachine.Update();
    }

    public void Solve()
    {
        Destroy(gameObject);
    }
    
    private void InitializeUIUnits()
    {
        _activeUIUnits = new Dictionary<UnitType, UnitBehaviour>();
        _incomingUIUnits = new Dictionary<UnitType, UnitBehaviour>();
        
        foreach (RequiredUnitData requiredUnitData in EmergencyData.RequiredResources)
        {
            GameObject activeUnitGO = ServiceLocator.Instance.UnitsManager.UnitFactory.SpawnUnit(requiredUnitData.Type);
            activeUnitGO.transform.SetParent(activeUnitsParent);
            UnitBehaviour activeUnit = activeUnitGO.GetComponent<UnitBehaviour>();
            activeUnit.OwningEmergency = this;
            activeUnit.UpdateCount(0);
            _activeUIUnits.Add(requiredUnitData.Type, activeUnit);
            
            GameObject incomingUnitGO = ServiceLocator.Instance.UnitsManager.UnitFactory.SpawnUnit(requiredUnitData.Type);
            incomingUnitGO.transform.SetParent(incomingUnitsParent);
            UnitBehaviour incomingUnit = incomingUnitGO.GetComponent<UnitBehaviour>();
            incomingUnit.IsIncoming = true;
            incomingUnit.OwningEmergency = this;
            incomingUnit.UpdateCount(0);
            _incomingUIUnits.Add(requiredUnitData.Type, incomingUnit);
        }
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
    
    public void AddActiveUnits(UnitType unitType, int amount)
    {
        if (_activeUnits.ContainsKey(unitType))
        {
            _activeUnits[unitType]++;
        }
        
        if (HasAllRequiredUnits())
        {
            IsSolving = true;
        }
        
        _activeUIUnits[unitType].UpdateCount(_activeUnits[unitType]);
    }
    
    public void RemoveActiveUnits(UnitType unitType, int amount)
    {
        if (_activeUnits.ContainsKey(unitType))
        {
            _activeUnits[unitType]--;
        }
        
        if (!HasAllRequiredUnits())
        {
            IsSolving = false;
        }
        
        _activeUIUnits[unitType].UpdateCount(_activeUnits[unitType]);
    }
    
    public void AddIncomingUnits(UnitType unitType, int amount)
    {
        if (_incomingUnits.ContainsKey(unitType))
        {
            _incomingUnits[unitType] += amount;
        }
        else
        {
            _incomingUnits.Add(unitType, amount);
        }
        
        _incomingUIUnits[unitType].UpdateCount(_incomingUnits[unitType]);
    }
    
    public void RemoveIncomingUnits(UnitType unitType, int amount)
    {
        if (_incomingUnits.ContainsKey(unitType))
        {
            _incomingUnits[unitType] -= amount;
            
            _incomingUIUnits[unitType].UpdateCount(_incomingUnits[unitType]);
        }
    }

    #endregion
    
    #region Interactions

    public void ReactToUnitHover(GameObject unit)
    {
        UnitType unitType = unit.GetComponent<UnitBehaviour>().Type;
        if (AcceptsUnitsOfType(unitType) && (Vector2)transform.position != unit.GetComponent<PlacementController>().StartPosition)
        {
            gameObject.transform.localScale *= hoverScaleMultiplier;
        }
        else
        {
            gameObject.transform.localScale = _originalScale;
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
        return;
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
        OnClick();
    }
    
    #endregion
}