using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool IsPaused { get; private set; }
    
    private void Start()
    {
        IsPaused = false;
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