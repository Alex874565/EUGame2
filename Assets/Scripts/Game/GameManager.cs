using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool IsPaused { get; private set; }
    
    private void Start()
    {
        IsPaused = false;

        ServiceLocator.Instance.InputManager.OnEscapeAction += InputManager_OnEscapeAction;
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
            ResumeGame();
            uiManager.PauseUI.Hide();
            return;
        }

        // 3. Otherwise → pause
        Debug.Log("pausing game");
        PauseGame();
        uiManager.PauseUI.Show();
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