using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class PackageMinigameController : MinigameController
{
    [SerializeField] private List<GameObject> _packagePrefabs; 
    
    [Header("UI Elements")]
    [SerializeField] private Transform _packagesContainer;
    [SerializeField] private Button _sendButton;

    [Header("Animation Settings")]
    [SerializeField] private float _staggerDelay = 0.05f;
    [SerializeField] private float _animDuration = 0.3f;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _popSound;

    private List<PackageBehaviour> _selectedPackages = new();
    
    private new void Awake()
    {
        _sendButton.onClick.AddListener(OnSendButtonClicked);
        
        base.Awake();
    }

    /*private void Start()
    {
        StartMinigame();
    }*/

    private new void Update()
    {
        base.Update();
    }

    public override void StartMinigame()
    {
        SpawnNewPackages();
        
        base.StartMinigame();
    }
    
    private void ClearPackages(System.Action onCleared = null)
    {
        // If there are no children, just invoke the callback and return
        if (_packagesContainer.childCount == 0)
        {
            onCleared?.Invoke();
            return;
        }

        Sequence clearSeq = DOTween.Sequence().SetUpdate(true);
        
        // Reverse loop for a "last-in-first-out" feel
        for (int i = _packagesContainer.childCount - 1; i >= 0; i--)
        {
            Transform child = _packagesContainer.GetChild(i);
            float delay = (_packagesContainer.childCount - 1 - i) * _staggerDelay;

            clearSeq.Insert(delay, child.DOScale(0f, _animDuration * 0.7f).SetEase(Ease.InBack));
        }

        // Wait for all animations to finish, then destroy and spawn new ones
        clearSeq.OnComplete(() =>
        {
            foreach (Transform child in _packagesContainer)
            {
                Destroy(child.gameObject);
            }
            onCleared?.Invoke();
        });
    }

    private void SpawnNewPackages()
    {
        // We pass the actual spawning logic as a callback to ClearPackages
        ClearPackages(() =>
        {
            Sequence spawnSeq = DOTween.Sequence().SetUpdate(true);

            for (int i = 0; i < 9; i++)
            {
                GameObject packagePrefab = _packagePrefabs[Random.Range(0, _packagePrefabs.Count)];
                GameObject packageGo = Instantiate(packagePrefab, _packagesContainer);
                
                // Start at scale 0
                packageGo.transform.localScale = Vector3.zero;
                packageGo.GetComponent<PackageBehaviour>().OnClick = OnClickPackage;

                float delay = i * _staggerDelay;

                // Add sound
                if (_popSound != null && _audioSource != null)
                {
                    spawnSeq.InsertCallback(delay, () => _audioSource.PlayOneShot(_popSound));
                }

                // Add animation
                spawnSeq.Insert(delay, packageGo.transform.DOScale(1f, _animDuration).SetEase(Ease.OutBack));
            }
        });
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