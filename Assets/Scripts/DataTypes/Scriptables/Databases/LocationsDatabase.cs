using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LocationsDatabase", menuName = "ScriptableObjects/Databases/LocationsDatabase", order = 1)]
public class LocationsDatabase : ScriptableObject
{
    [field: SerializeField] public List<LocationData> Locations { get; set; }
    
    public LocationData GetLocationByName(LocationName name)
    {
        return Locations.Find(location => location.Name == name);
    }
}