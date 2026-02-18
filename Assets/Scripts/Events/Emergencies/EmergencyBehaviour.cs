using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class EmergencyBehaviour : MonoBehaviour
{
    [SerializeField] private EmergencyType emergencyType;
    [field: Header("UI")]
    [field: SerializeField] public Image FillImage { get; private set; }
    [field: SerializeField] public TMP_Text TimerText { get; private set; }
    
    public EmergencyData EmergencyData { get; private set; }
    public Dictionary<ResourceType, int> ActiveResources { get; private set; }
    
    public float ExpirationTimeLeft { get; set; }
    public float SolvingTimeLeft { get; set; }
    
    private EmergencyStateMachine _emergencyStateMachine;
    
    private void Start()
    {
        ActiveResources = new  Dictionary<ResourceType, int>();
        EmergencyData = ServiceLocator.Instance.EmergenciesManager.EmergenciesStats[emergencyType];
        ServiceLocator.Instance.EmergenciesManager.ActiveEmergencies.Add(gameObject);
        foreach (RequiredResourceData resourceData in EmergencyData.RequiredResources)
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
    
    public bool HasAllRequiredResources()
    {
        foreach (RequiredResourceData requiredResourceData in EmergencyData.RequiredResources)
        {
            if (ActiveResources[requiredResourceData.Type] < requiredResourceData.Amount)
            {
                return false;
            }
        }

        return true;
    }
    
    public bool HasAllResourcesOfType(ResourceType resourceType)
    {
        RequiredResourceData requiredResourceData = EmergencyData.RequiredResources.Find(resource => resource.Type == resourceType);
        
        if (requiredResourceData == null)
        {
            return true;
        }
        
        return ActiveResources[resourceType] >= requiredResourceData.Amount;
    }
}