using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static SaveManager _instance;
    
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
    
    public bool HasSaveFile() 
    {
        string path = Application.persistentDataPath + "/savefile.json";
        return System.IO.File.Exists(path);
    }
    
    public void SaveGame(SaveData saveData)
    {
        // save to file serialized
        string json = JsonUtility.ToJson(saveData);
        string path = Application.persistentDataPath + "/savefile.json";
        System.IO.File.WriteAllText(path, json);
    }
    
    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (!System.IO.File.Exists(path))
        {
            Debug.LogWarning("No save file found at " + path);
            return;
        }
        
        string json = System.IO.File.ReadAllText(path);
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);
        ServiceLocator.Instance.GameManager.WaveIndex = saveData.WaveIndex;
        ServiceLocator.Instance.GameManager.WonLastWave = saveData.WonLastWave;
        ServiceLocator.Instance.PlayerManager.OwnedUpgrades = saveData.GetOwnedUpgradesDictionary();
        ServiceLocator.Instance.PlayerManager.AddMoney(saveData.PlayerMoney - ServiceLocator.Instance.PlayerManager.Money);
        ServiceLocator.Instance.PlayerManager.StartingUnits = saveData.GetStartingUnitsDictionary();
    }
    
    public void DeleteSaveFile()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }
    }
}