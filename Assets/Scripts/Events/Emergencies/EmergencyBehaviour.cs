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
    [SerializeField] private float hoverScaleMultiplier = 1.2f;
    
    private Vector3 _originalScale;
    
    public EmergencyData EmergencyData { get; private set; }
    public Dictionary<UnitType, int> ActiveUnits { get; private set; }
    public Dictionary<UnitType, int> IncomingUnits { get; private set; }
    public bool IsSolving { get; set; }
    
    public float ExpirationTimeLeft { get; set; }
    public float SolvingTimeLeft { get; set; }
    
    private EmergencyStateMachine _emergencyStateMachine;
    
    private void Start()
    {
        _originalScale = gameObject.transform.localScale;
        ActiveUnits = new  Dictionary<UnitType, int>();
        IncomingUnits = new Dictionary<UnitType, int>();
        EmergencyData = ServiceLocator.Instance.EmergenciesManager.EmergenciesStats[emergencyType];
        ServiceLocator.Instance.EmergenciesManager.ActiveEmergencies.Add(gameObject);
        foreach (RequiredUnitData resourceData in EmergencyData.RequiredResources)
        {
            ActiveUnits.Add(resourceData.Type, 0);
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
        Destroy(gameObject);
    }

    #region Unit Checks
    
    public bool HasAllRequiredUnits()
    {
        foreach (RequiredUnitData requiredUnitData in EmergencyData.RequiredResources)
        {
            if (ActiveUnits[requiredUnitData.Type] < requiredUnitData.Amount)
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
        
        int incomingAmount = IncomingUnits.ContainsKey(unitType) ? IncomingUnits[unitType] : 0;
        int activeAmount = ActiveUnits.ContainsKey(unitType) ? ActiveUnits[unitType] : 0;
        int requiredAmount = requiredUnitData != null ? requiredUnitData.Amount : 0;

        return requiredAmount - activeAmount - incomingAmount;
    }
    
    #endregion
    
    #region Unit Management
    
    public void AddActiveUnits(UnitType unitType, int amount)
    {
        if (ActiveUnits.ContainsKey(unitType))
        {
            ActiveUnits[unitType]++;
        }
        
        if (HasAllRequiredUnits())
        {
            IsSolving = true;
        }
    }
    
    public void RemoveActiveUnits(UnitType unitType, int amount)
    {
        if (ActiveUnits.ContainsKey(unitType))
        {
            ActiveUnits[unitType]--;
        }
    }
    
    public void AddIncomingUnits(UnitType unitType, int amount)
    {
        if (IncomingUnits.ContainsKey(unitType))
        {
            IncomingUnits[unitType] += amount;
        }
        else
        {
            IncomingUnits.Add(unitType, amount);
        }
    }
    
    public void RemoveIncomingUnits(UnitType unitType, int amount)
    {
        if (IncomingUnits.ContainsKey(unitType))
        {
            IncomingUnits[unitType] -= amount;
            if (IncomingUnits[unitType] <= 0)
            {
                IncomingUnits.Remove(unitType);
            }
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