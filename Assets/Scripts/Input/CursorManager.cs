using Unity.VisualScripting;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private LayerMask _interactableMask;
    
    void Update()
    {
        if (ServiceLocator.Instance.InputManager.RightClickHeld)
        {
            return;
        }
        
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos,_interactableMask);
        
        if (hit != null)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();
            
            if(interactable != null)
            {
                if(ServiceLocator.Instance.InputManager.LeftClickPressed)
                {
                    interactable.OnClick();
                }
                else
                {
                    interactable.OnHover();
                }
            }
        }
    }    
}