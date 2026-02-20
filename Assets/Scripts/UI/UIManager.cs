using UnityEngine;

public class UIManager : MonoBehaviour
{
    [field: SerializeField] public GameObject DragLayer {get; private set;}
    [field: SerializeField] public SettingsUI SettingsUI {get; private set;}
}