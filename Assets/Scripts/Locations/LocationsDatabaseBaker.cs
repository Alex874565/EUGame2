#if UNITY_EDITOR
using UnityEngine;
using System.IO;
using System.Text;
using UnityEditor;

public class LocationsDatabaseBaker : MonoBehaviour
{
    [field: SerializeField] public LocationsDatabase LocationsDatabase { get; private set; }
    
    public void BakeLocations()
    {
        if (LocationsDatabase == null)
        {
            return;
        }
        
        LocationsDatabase.locations.Clear();
        
        MapLocation[] mapLocations = FindObjectsByType<MapLocation>(FindObjectsSortMode.None);
        
        GenerateEnum(mapLocations);
        
        foreach (MapLocation mapLocation in mapLocations)
        {
            LocationData locationData = new LocationData
            {
                Name = (LocationName)System.Enum.Parse(typeof(LocationName), mapLocation.name.Replace(" ", "_")),
                Type = mapLocation.LocationType,
                Coordinates = mapLocation.transform.position
            };
            
            LocationsDatabase.locations.Add(locationData);
        }
    }
    
    private void GenerateEnum(MapLocation[] mapLocations)
    {
        string path = "Assets/Generated/LocationName.cs";

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("public enum LocationName");
        sb.AppendLine("{");

        foreach (var loc in mapLocations)
        {
            string clean = loc.name.Replace(" ", "_");
            sb.AppendLine($"    {clean},");
        }

        sb.AppendLine("}");

        Directory.CreateDirectory("Assets/Generated");
        File.WriteAllText(path, sb.ToString());

        AssetDatabase.Refresh();
    }
}
#endif