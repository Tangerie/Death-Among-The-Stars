using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(PlanetGenerator))]
public class PlanetGeneratorEditor : Editor
{
    PlanetGenerator gen;

    private bool showDefaultInspector;

    private static bool genOnChange = true;

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        showDefaultInspector = EditorGUILayout.Foldout(showDefaultInspector, "Default Inspector");
        EditorGUI.indentLevel++;
        if (showDefaultInspector) DrawDefaultInspector();
        EditorGUI.indentLevel--;

        EditorGUILayout.Space();

        Editor shapeGenEditor = CreateEditor(gen.ShapeGenerator);

        shapeGenEditor.DrawDefaultInspector();

        EditorGUILayout.Space();

        genOnChange = EditorGUILayout.Toggle("Generate On Change", genOnChange);
        gen.qualityLevel = EditorGUILayout.IntSlider("Mesh Quality Level", gen.qualityLevel, 0, 100);

        if (genOnChange && EditorGUI.EndChangeCheck())
        {
            gen.Generate();
        }

        if(GUILayout.Button("Generate"))
        {
            gen.Generate();
        }
    }

    public void OnEnable()
    {
        gen = (PlanetGenerator)target;
    }
}
#endif