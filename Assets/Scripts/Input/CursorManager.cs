using Unity.VisualScripting;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private LayerMask _interactableMask;
    
    public GameObject HoveredObject { get; private set; }
    
    IInteractable _interactable;
    
    void Update()
    {
        if (ServiceLocator.Instance.InputManager.LeftClickHeld)
        {
            return;
        }
        
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos,_interactableMask);
        
        if (hit != null)
        {
            IInteractable interactable = hit.gameObject.GetComponentInChildren<IInteractable>();
            
            if(interactable != null)
            {
                if(ServiceLocator.Instance.InputManager.LeftClickPressed)
                {
                    interactable.OnClick();
                }
                else
                {
                    if(interactable != _interactable)
                    {
                        if(_interactable != null)
                            _interactable.OnHoverExit();
                        
                        interactable.OnHoverEnter();
                        _interactable = interactable;
                        HoveredObject = hit.gameObject;
                    }
                }
            }
        }
        else
        {
            if(_interactable != null)
                _interactable.OnHoverExit();
            
            _interactable = null;
            HoveredObject = null;
        }
    }

    public bool IsHoveringUI()
    {
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }
}