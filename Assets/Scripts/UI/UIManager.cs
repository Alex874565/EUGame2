using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [field: SerializeField] public Canvas Canvas;
    [field: TagField, SerializeField] public string MenusTag { get; private set; } = "Menu";
    
    [field: Header("Pause Menus")]
    [field: SerializeField] public SettingsUI SettingsUI {get; private set;}

    [field: Header("HUD")]
    [field: SerializeField] public GameObject EmergencyDetailsMenu;
    [field: SerializeField] public GameObject MinigameDetailsMenu;
    
    [field: Header("Unit Layers")]
    [field: SerializeField] public GameObject PlacementLayer { get; private set; }
    [field: SerializeField] public GameObject MovementLayer { get; private set; }
    [field: SerializeField] public GameObject EmergenciesLayer {get; private set;}
    
    public void OpenEmergencyDetails(EmergencyBehaviour emergency)
    {
        EmergencyDetailsMenu.SetActive(true);
        EmergencyDetailsMenu.GetComponent<EmergencyDetailsUI>().Initialize(emergency);
    }
    
    public void OpenMinigameDetails(MinigamePopupBehaviour minigame)
    {
        MinigameDetailsMenu.SetActive(true);
        MinigameDetailsMenu.GetComponent<MinigameDetailsUI>().Initialize(minigame);
    }
}