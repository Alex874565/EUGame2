using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CursorManager : MonoBehaviour
{
    public GameObject HoveredObject { get; set; }
    
    public bool IsHoveringMenu()
    {
        string menuTag = ServiceLocator.Instance.UIManager.MenusTag;
        
        PointerEventData data = new PointerEventData(EventSystem.current);
        data.position = Input.mousePosition;

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);

        foreach (var r in results)
        {
            if (r.gameObject.CompareTag(menuTag) ||
                r.gameObject.GetComponentInParent<Canvas>()?.CompareTag(menuTag) == true)
            {
                return true;
            }
        }

        return false;
    }
}