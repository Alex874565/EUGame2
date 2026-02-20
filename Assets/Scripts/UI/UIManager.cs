using Unity.Cinemachine;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [field: SerializeField] public Canvas Canvas;
    [field: TagField, SerializeField] public string MenusTag { get; private set; } = "Menu";
    [field: Header("UI Menus")]
    [field: SerializeField] public GameObject HUD;
    [field: Header("Unit Layers")]
    [field: SerializeField] public GameObject PlacementLayer { get; private set; }
    [field: SerializeField] public GameObject MovementLayer { get; private set; }
}