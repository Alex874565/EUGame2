using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class TrashBinBehaviour : MonoBehaviour
{
    public UnityAction OnTrashEnterCollider;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<TrashItemBehaviour>() != null)
        {
            OnTrashEnterCollider?.Invoke();
            Destroy(other.gameObject);
        }
    }
}