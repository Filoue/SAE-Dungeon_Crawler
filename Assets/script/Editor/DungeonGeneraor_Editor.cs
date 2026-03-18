using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DungeonGenerators))]
public class DungeonGeneraor_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        
        DungeonGenerators generator = (DungeonGenerators)target;
        
        DrawDefaultInspector();
        
        EditorGUILayout.Space(25);
        EditorGUILayout.Separator();
        
        
        if (GUILayout.Button("Generate"))
        {
            generator.Generator();
        }
    }
}
