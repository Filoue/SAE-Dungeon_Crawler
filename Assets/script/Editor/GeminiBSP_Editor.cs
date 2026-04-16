using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BSPGenerator))]
public class GeminiBSP_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        BSPGenerator generator = (BSPGenerator)target;
        
        DrawDefaultInspector();
        
        EditorGUILayout.Space(25);
        EditorGUILayout.Separator();
        if (GUILayout.Button("Generate"))
        {
            generator.GeminiGenerate();
        }
    }
}
