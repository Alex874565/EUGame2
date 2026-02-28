using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class CleanupMinigameController : MinigameController
{
    [SerializeField] private TrashBinBehaviour _trashBin;
    [SerializeField] private List<GameObject> _trashItemPrefabs;
    
    private List<GameObject> _spawnedTrashItems;
    
    private Collider2D _spawnArea;
    
    private void Awake()
    {
        _spawnArea = GetComponent<Collider2D>();
        _spawnedTrashItems = new List<GameObject>();
        base.Awake();
    }
    
    private void Update()
    {
        base.Update();
    }
    
    public override void StartMinigame()
    {
        _trashBin.OnTrashEnterCollider = null;
        _trashBin.OnTrashEnterCollider = OnTrashEnterBin;
        SpawnTrashItems(Data.ScoreToWin);
        
        base.StartMinigame();
    }
    
    private void SpawnTrashItems(int count)
    {
        int spawned = 0;
        while (spawned < count)
        {
            Vector2 spawnPosition = new Vector2(
                Random.Range(_spawnArea.bounds.min.x, _spawnArea.bounds.max.x),
                Random.Range(_spawnArea.bounds.min.y, _spawnArea.bounds.max.y)
            );
            
            Vector2 spawnScale = new Vector2(
                Random.Range(0.8f, 1.2f),
                Random.Range(0.8f, 1.2f)
            );
            
            Vector2 spawnRotation = new Vector3(
                0f,
                0f,
                Random.Range(0f, 360f)
            );
        
            GameObject trashPrefab = _trashItemPrefabs[Random.Range(0, _trashItemPrefabs.Count)];
            GameObject trashItem = Instantiate(trashPrefab, spawnPosition, Quaternion.Euler(spawnRotation), transform);
            trashItem.transform.localScale = spawnScale;
            spawned++;
            _spawnedTrashItems.Add(trashItem);
        }
    }

    private void OnTrashEnterBin()
    {
        StartCoroutine(HandleTrashEnteredBin());
    }
    
    private IEnumerator HandleTrashEnteredBin()
    {
        Time.timeScale = 0.2f;
        AddScore(1);
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
    }
    
    protected override void Cleanup()
    {
        foreach (GameObject trashItem in _spawnedTrashItems)
        {
            if (trashItem != null)
            {
                Destroy(trashItem);
            }
        }
        _spawnedTrashItems.Clear();
    }
}