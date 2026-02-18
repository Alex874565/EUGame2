using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WavesDatabase", menuName = "ScriptableObjects/Databases/WavesDatabase")]
public class WavesDatabase : ScriptableObject
{
    [field: SerializeField] public List<WaveData> Waves { get; private set; }
}