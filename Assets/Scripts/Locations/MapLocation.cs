using UnityEngine;

public class MapLocation : MonoBehaviour
{
    [field: SerializeField] public string LocationName { get; private set; }
    [field: SerializeField] public LocationType LocationType { get; private set; }
}