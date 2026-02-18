using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LocationsDatabase", menuName = "ScriptableObjects/Databases/LocationsDatabase", order = 1)]
public class LocationsDatabase : ScriptableObject
{
    public List<LocationData> locations;
    
    public LocationData GetLocationByName(LocationName name)
    {
        return locations.Find(location => location.Name == name);
    }
}