#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LocationsDatabaseBaker))]
public class LocationsDatabaseBakerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        // target can be null during reload / missing script / etc.
        var baker = target as LocationsDatabaseBaker;
        if (baker == null)
        {
            EditorGUILayout.HelpBox("Baker target is null (likely during script reload or invalid selection).", MessageType.Warning);
            return;
        }

        if (baker.LocationsDatabase == null)
        {
            EditorGUILayout.HelpBox("Assign a LocationsDatabase asset first.", MessageType.Info);
            return;
        }

        if (GUILayout.Button("Bake Locations"))
        {
            baker.BakeLocations();
            EditorUtility.SetDirty(baker.LocationsDatabase);
            AssetDatabase.SaveAssets();
        }
    }
}
#endif