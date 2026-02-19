using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WavesDatabase", menuName = "ScriptableObjects/Databases/WavesDatabase")]
public class WavesDatabase : ScriptableObject
{
    [field: SerializeField] public List<WaveData> Waves { get; private set; }
    
    private void OnValidate()
    {
        for (int i = 0; i < Waves.Count; i++)
        {
            Waves[i].WaveNumber = i + 1;
        }
    }
}