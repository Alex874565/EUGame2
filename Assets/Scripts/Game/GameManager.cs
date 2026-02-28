using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    
    public bool IsPaused { get; private set; }
    
    public bool WonLastWave { get; set; }
    
    public int WaveIndex { get; set; }
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        IsPaused = false;

        if (ServiceLocator.Instance.InputManager)
        {
            ServiceLocator.Instance.InputManager.OnEscapeAction += InputManager_OnEscapeAction;
        }

        WonLastWave = true;
    }

    private void OnDestroy()
    {
        if (ServiceLocator.Instance.InputManager)
        {
            ServiceLocator.Instance.InputManager.OnEscapeAction -= InputManager_OnEscapeAction;
        }
    }

    private void InputManager_OnEscapeAction(object sender, EventArgs e)
    {
        var uiManager = ServiceLocator.Instance.UIManager;

        // 1. If Settings is open → close it
        if (uiManager.SettingsUI.gameObject.activeSelf)
        {
            uiManager.SettingsUI.Hide();
            return;
        }

        // 2. If Pause menu is open → resume
        if (uiManager.PauseUI.gameObject.activeSelf)
        {
            Debug.Log("resuming game");
            
            uiManager.PauseUI.Hide();
            ResumeGame();
            return;
        }

        // 3. Otherwise → pause
        Debug.Log("pausing game");
        
        uiManager.PauseUI.Show();
        PauseGame();
    }
    
    public void StartWave()
    {
        WonLastWave = false;
        SceneManager.LoadScene("SampleScene");
    }

    public void PauseGame()
    {
        IsPaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        IsPaused = false;
        Time.timeScale = 1f;
    }
}