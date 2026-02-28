using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class PackageMinigameController : MinigameController
{
    [SerializeField] private List<GameObject> _packagePrefabs; 
    
    [Header("UI Elements")]
    [SerializeField] private Transform _packagesContainer;
    [SerializeField] private Button _sendButton;

    private List<PackageBehaviour> _selectedPackages;
    
    private void Awake()
    {
        _sendButton.onClick.AddListener(OnSendButtonClicked);
        
        base.Awake();
    }
    
    private void Update()
    {
        base.Update();
    }

    public override void StartMinigame()
    {
        _selectedPackages = new List<PackageBehaviour>();
        SpawnNewPackages();
        
        base.StartMinigame();
    }
    
    private void ClearPackages()
    {
        foreach (Transform child in _packagesContainer)
        {
            Destroy(child.gameObject);
        }
    }
    
    private void SpawnNewPackages()
    {
        ClearPackages();
        for (int i = 0; i < 9; i++)
        {
            GameObject packagePrefab = _packagePrefabs[Random.Range(0, _packagePrefabs.Count)];
            GameObject packageGo = Instantiate(packagePrefab, _packagesContainer);
            packageGo.GetComponent<PackageBehaviour>().OnClick = OnClickPackage;
        }
    }

    private void OnClickPackage(PackageBehaviour package)
    {
        if (_selectedPackages.Contains(package))
        {
            _selectedPackages.Remove(package);
        }
        else
        {
            _selectedPackages.Add(package);
        }
    }
    
    private void OnSendButtonClicked()
    {
        int scoreToAdd = 0;
        foreach (PackageBehaviour package in _selectedPackages)
        {
            if(package.IsFood)
            {
                scoreToAdd++;
            }
            else
            {
                scoreToAdd--;
            }
        }
        
        _selectedPackages.Clear();
        
        AddScore(scoreToAdd);
        
        SpawnNewPackages();
    }
    
    protected override void Cleanup()
    {
        ClearPackages();
    }
}