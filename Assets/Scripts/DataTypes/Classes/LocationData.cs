using System;
using UnityEngine;

[Serializable]
public class LocationData
{
    [field: SerializeField] public LocationName Name { get; set; }
    [field: SerializeField] public LocationType Type { get; set; }
    [field: SerializeField] public Vector3 Coordinates { get; set; }
}