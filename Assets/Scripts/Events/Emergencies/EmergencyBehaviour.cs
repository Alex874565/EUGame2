using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

[RequireComponent(typeof(SolvableObjectSFX))]
public class EmergencyBehaviour : MonoBehaviour, IInteractable, IPointerEnterHandler, IPointerExitHandler
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
    [SerializeField] private float wiggleDegrees = 10f;
    [SerializeField] private float wiggleDuration = 0.5f;
    [SerializeField] private float wiggleScaleMultiplier = 1.1f;

    private Action onCompletedEmergency;

    public Action OnCompletedEmergency
    {
        get => onCompletedEmergency;
        set => onCompletedEmergency = value;
    }

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
    
    public bool IsSolving { get; private set; }
    
    public float ExpirationTimeLeft { get; set; }
    public float SolvingTimeLeft { get; set; }
    
    private EmergencyStateMachine _emergencyStateMachine;
    
    private Vector3 _originalScale;
    
    private bool _isWiggling;
    private bool _isHovered;

    private SolvableObjectSFX _solvableObjectSfx;
    
    private void Start()
    {
        _solvableObjectSfx = GetComponent<SolvableObjectSFX>();
        _originalScale = transform.localScale;
        _activeUnits = new  Dictionary<UnitType, int>();
        _incomingUnits = new Dictionary<UnitType, int>();
        EmergencyData = ServiceLocator.Instance.EmergenciesManager.EmergenciesData[emergencyType];
        ServiceLocator.Instance.EmergenciesManager.ActiveEmergencies.Add(gameObject);
        foreach (RequiredUnitData resourceData in EmergencyData.RequiredResources)
        {
            _activeUnits.Add(resourceData.Type, 0);
        }

        ExpirationTimeLeft = EmergencyData.TimeUntilExpiry;
        SolvingTimeLeft = EmergencyData.TimeToSolve;

        _emergencyStateMachine = new EmergencyStateMachine(this);
        
        _isWiggling = false;
        _isHovered = false;
        
        transform.localScale = _originalScale * 0f;
        DOTween.Sequence()
            .Append(transform.DOScale(_originalScale * 1.5f, .25f).SetEase(Ease.OutCubic))
            .Append(transform.DOScale(_originalScale, .1f).SetEase(Ease.InOutQuad)).SetUpdate(true);
        
        _solvableObjectSfx.PlayAppearSFX();
    }

    private void Update()
    {
        _emergencyStateMachine.Update();
        
        Quaternion target = Quaternion.identity;

        if (_isWiggling)
        {
            float angle = Mathf.Sin(Time.time * (2 * Mathf.PI / wiggleDuration)) * wiggleDegrees;
            target = Quaternion.Euler(0, 0, angle);
        }

        image.rectTransform.localRotation =
            Quaternion.Lerp(image.rectTransform.localRotation, target, Time.deltaTime * 10f);
    }

    public void Solve()
    {
        if (IsSelected)
        {
            ServiceLocator.Instance.CursorManager.SelectObject(null);
        }
        ReturnInventoryUnits();
        _solvableObjectSfx.PlaySolveSFX();
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
        _solvableObjectSfx.PlayUnsolveSFX();
        Destroy(gameObject);
    }
    
    public void StartWiggle()
    {
        _isWiggling = true;
        transform.localScale *= wiggleScaleMultiplier;
        Debug.Log("StartWiggle:"+transform.localScale);
    }
    
    public void TryStopWiggle()
    {
        if (_isWiggling)
        {
            _isWiggling = false;
            transform.localScale /= wiggleScaleMultiplier;
            Debug.Log("StopWiggle:"+transform.localScale);
        }
    }

    private void OnDestroy()
    {
        onCompletedEmergency?.Invoke();
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
        
        int incomingAmount = _incomingUnits.GetValueOrDefault(unitType, 0);
        int activeAmount = _activeUnits.GetValueOrDefault(unitType, 0);
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

        foreach (Transform child in _activeUnitsContainer.transform)
        {
            Destroy(child.gameObject);
        }

        Canvas.ForceUpdateCanvases();

        foreach (var kvp in _activeUnits)
        {
            if (kvp.Value <= 0) continue;

            GameObject unit = ServiceLocator.Instance.UnitsManager.UnitFactory.SpawnUnit(kvp.Key);
            unit.transform.SetParent(_activeUnitsContainer.transform, false);

            RectTransform rt = unit.GetComponent<RectTransform>();
            rt.localScale = Vector3.one;
            rt.localRotation = Quaternion.identity;
            rt.anchoredPosition = Vector2.zero;

            var unitBehaviour = unit.GetComponent<UnitBehaviour>();
            unitBehaviour.UpdateCount(kvp.Value);
            unitBehaviour.IsInteractable = false;

            unit.GetComponentInChildren<Image>().raycastTarget = false;
        }

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(
            _activeUnitsContainer.GetComponent<RectTransform>()
        );
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
        TrySelect();
    }
    
    public void Deselect()
    {
        TryDeselect();
    }
    
    public void TrySelect()
    {
        if (!IsSelected)
        {
            IsSelected = true;
            ServiceLocator.Instance.UIManager.OpenEmergencyDetails(this);
            StartCoroutine(SelectCoroutine());
            GetComponent<TutorialTarget>()?.NotifyAction("EmergencySelected");
            _solvableObjectSfx.PlaySelectSFX();
        }
    }

    public IEnumerator SelectCoroutine()
    {
        transform.localScale *= clickScaleMultiplier;
        yield return new WaitForSecondsRealtime(0.1f);
        transform.localScale /= clickScaleMultiplier;
        transform.localScale *= selectedScaleMultiplier;
    }

    public void TryDeselect()
    {
        if (IsSelected)
        {
            transform.localScale /= selectedScaleMultiplier;
            IsSelected = false;
            ServiceLocator.Instance.UIManager.EmergencyDetailsMenu.SetActive(false);
            _solvableObjectSfx.PlayDeselectSFX();
        }
    }
    
    public void ReactToUnitHover(GameObject unit)
    {
        UnitType unitType = unit.GetComponent<UnitBehaviour>().Type;
        if (AcceptsUnitsOfType(unitType) && (Vector2)transform.position != unit.GetComponent<PlacementController>().StartPosition)
        {
            Hover();
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
            Hover();
        }
    }

    public void OnHoverExit()
    {
        if (_isHovered)
        {
            transform.localScale /= hoverScaleMultiplier;
            _isHovered = false;
            Debug.Log("Hover exit:" + transform.localScale);
        }

        ServiceLocator.Instance.CursorManager.HoveredObject = null;
    }

    private void Hover()
    {
        if (!_isHovered)
        {
            _isHovered = true;
            transform.localScale *= hoverScaleMultiplier;
            Debug.Log("Hover enter:" + transform.localScale);
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
    
    #endregion
}