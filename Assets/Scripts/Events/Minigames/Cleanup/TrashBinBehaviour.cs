using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D), typeof(SelectableObjectSFX))]
public class TrashBinBehaviour : MonoBehaviour
{
    public UnityAction OnTrashEnterCollider;
    
    private SelectableObjectSFX _objectSfx;
    
    private void Start()
    {
        _objectSfx = GetComponent<SelectableObjectSFX>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<TrashItemBehaviour>() != null)
        {
            _objectSfx.PlaySelectSFX();
            OnTrashEnterCollider?.Invoke();
            Destroy(other.gameObject);
        }
    }
}