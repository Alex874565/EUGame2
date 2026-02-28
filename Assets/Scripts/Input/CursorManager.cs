using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CursorManager : MonoBehaviour
{
    public GameObject HoveredObject { get; set; }
    public GameObject SelectedObject { get; set; }
    
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

    private void OnClick(System.Object sender, System.EventArgs e)
    {
        if(!IsHoveringMenu())
        {
            if (!ServiceLocator.Instance.PlacementManager.UnitInPlacing)
            {
                if (HoveredObject == null)
                {
                    SelectObject(null);
                }
                else
                {
                    SelectObject(HoveredObject);
                }
            }
        }
    }
    
    private void Start()
    {
        ServiceLocator.Instance.InputManager.OnLeftClickActionFirst += OnClick;
    }
    
    private void OnDestroy()
    {
        ServiceLocator.Instance.InputManager.OnLeftClickActionFirst -= OnClick;
    }
}