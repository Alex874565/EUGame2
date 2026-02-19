using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class EmergencyBehaviour : MonoBehaviour, IInteractable
{
    [SerializeField] private EmergencyType emergencyType;
    [field: Header("UI")]
    [field: SerializeField] public Image FillImage { get; private set; }
    [field: SerializeField] public TMP_Text TimerText { get; private set; }
    [SerializeField] private float hoverScaleMultiplier = 1.2f;
    
    private Vector3 _originalScale;
    
    public EmergencyData EmergencyData { get; private set; }
    public Dictionary<UnitType, int> ActiveResources { get; private set; }
    
    public float ExpirationTimeLeft { get; set; }
    public float SolvingTimeLeft { get; set; }
    
    private EmergencyStateMachine _emergencyStateMachine;
    
    private void Start()
    {
        _originalScale = gameObject.transform.localScale;
        ActiveResources = new  Dictionary<UnitType, int>();
        EmergencyData = ServiceLocator.Instance.EmergenciesManager.EmergenciesStats[emergencyType];
        ServiceLocator.Instance.EmergenciesManager.ActiveEmergencies.Add(gameObject);
        foreach (RequiredUnitData resourceData in EmergencyData.RequiredResources)
        {
            ActiveResources.Add(resourceData.Type, 0);
        }

        ExpirationTimeLeft = EmergencyData.TimeUntilExpiry;
        SolvingTimeLeft = EmergencyData.TimeToSolve;

        _emergencyStateMachine = new EmergencyStateMachine(this);
    }

    private void Update()
    {
        _emergencyStateMachine.Update();
    }
    
    public bool HasAllRequiredUnits()
    {
        foreach (RequiredUnitData requiredUnitData in EmergencyData.RequiredResources)
        {
            if (ActiveResources[requiredUnitData.Type] < requiredUnitData.Amount)
            {
                return false;
            }
        }

        return true;
    }
    
    public bool HasAllUnitsOfType(UnitType unitType)
    {
        RequiredUnitData requiredUnitData = EmergencyData.RequiredResources.Find(resource => resource.Type == unitType);
        
        if (requiredUnitData == null)
        {
            return true;
        }
        
        return ActiveResources[unitType] >= requiredUnitData.Amount;
    }
    
    public void OnHoverEnter()
    {
        GameObject unitInPlacing = ServiceLocator.Instance.PlacementManager.UnitInPlacing;
        if (unitInPlacing != null)
        {
            UnitType unitType = unitInPlacing.GetComponent<UnitBehaviour>().Type;
            if (!HasAllUnitsOfType(unitType) && (Vector2)transform.position != unitInPlacing.GetComponent<PlacementController>().StartPosition)
            {
                gameObject.transform.localScale *= hoverScaleMultiplier;
            }
        }
    }
    
    public void OnHoverExit()
    {
        gameObject.transform.localScale = _originalScale;
    }
}