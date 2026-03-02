using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    public static ServiceLocator Instance { get; private set; }
    
    [field: Header("Managers")]
    [field: SerializeField] public InputManager InputManager { get; private set; }
    [field: SerializeField] public EmergenciesManager EmergenciesManager { get; private set; }
    [field: SerializeField] public MinigamesManager MinigamesManager { get; private set; }
    [field: SerializeField] public WavesManager WavesManager { get; private set; }
    [field: SerializeField] public UnitsManager UnitsManager { get; private set; }
    [field: SerializeField] public CursorManager CursorManager { get; private set; }
    [field: SerializeField] public PlacementManager PlacementManager { get; private set; }
    [field: SerializeField] public CameraManager CameraManager { get; private set; }
    [field: SerializeField] public UIManager UIManager { get; private set; }
    [field: SerializeField] public UpgradesManager UpgradesManager { get; private set; }
    [field: SerializeField] public DialogueManager DialogueManager { get; private set; }
    public SaveManager SaveManager { get; private set; }
    public GameManager GameManager { get; private set; }
    public PlayerManager PlayerManager { get; private set; }

    [field: Header("Databases")]
    [field: SerializeField] public LocationsDatabase LocationsDatabase { get; private set; }
    [field: SerializeField] public EmergenciesDatabase EmergenciesDatabase { get; private set; }
    [field: SerializeField] public WavesDatabase WavesDatabase { get; private set; }
    [field: SerializeField] public UnitsDatabase UnitsDatabase { get; private set; }
    [field: SerializeField] public UpgradesDatabase UpgradesDatabase { get; private set; }
    [field: SerializeField] public QuizzesDatabase QuizzesDatabase { get; private set; }
    
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
            return;
        }

        PlayerManager playerManager = FindFirstObjectByType<PlayerManager>();

        if (playerManager)
        {
            PlayerManager = playerManager;
        }
        
        GameManager gameManager = FindFirstObjectByType<GameManager>();
        
        if (gameManager)
        {
            GameManager = gameManager;
        }
        
        SaveManager saveManager = FindFirstObjectByType<SaveManager>();
        
        if (saveManager)
        {
            SaveManager = saveManager;
        }
    }
}
