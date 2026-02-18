using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    public static ServiceLocator Instance { get; private set; }
    
    [field: Header("Managers")]
    [field: SerializeField] public InputManager InputManager { get; private set; }
    [field: SerializeField] public EmergenciesManager EmergenciesManager { get; private set; }
    [field: SerializeField] public MinigamesManager MinigamesManager { get; private set; }
    [field: SerializeField] public TimelineManager TimelineManager { get; private set; }
    
    [field: Header("Databases")]
    [field: SerializeField] public LocationsDatabase LocationsDatabase { get; private set; }
    [field: SerializeField] public EmergenciesDatabase EmergenciesDatabase { get; private set; }
    [field: SerializeField] public MinigamesDatabase MinigamesDatabase { get; private set; }
    [field: SerializeField] public WavesDatabase WavesDatabase { get; private set; }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
