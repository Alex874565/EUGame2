using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CursorManager : MonoBehaviour
{
    public GameObject HoveredObject { get; set; }
    public GameObject SelectedObject { get; set; }
    
    private void Update()
    {
        if(ServiceLocator.Instance.InputManager.LeftClickPressed)
        {
            if(!IsHoveringMenu())
            {
                if (HoveredObject == null)
                {
                    SelectObject(null);
                }
                else if (!ServiceLocator.Instance.PlacementManager.UnitInPlacing)
                {
                    SelectObject(HoveredObject);
                }
            }
        }
    }
    
    public bool IsHoveringMenu()
    {
        if (EventSystem.current == null) return false;

        var data = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);
        
        // ACTUAL CHECK: any hit under a UIRoot counts
        foreach (var r in results)
        {
            if (r.gameObject.GetComponentInParent<UIRoot>(true) != null)
                return true;
        }

        return false;
    }
    
    public void SelectObject(GameObject obj)
    {
        if(SelectedObject != null)
        {
            SelectedObject.GetComponent<IInteractable>()?.Deselect();
        }
        SelectedObject = obj;
        SelectedObject?.GetComponent<IInteractable>()?.Select();
    }
}