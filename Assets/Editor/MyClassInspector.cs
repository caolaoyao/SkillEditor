using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MyClass))]
public class MyClassInspector : Editor 
{
    void OnInspectorGUI()
    {
        SerializedProperty s_person = serializedObject.FindProperty("person");
        EditorGUILayout.PropertyField(s_person);
        SerializedProperty s_myScale = serializedObject.FindProperty("myScale");
        EditorGUILayout.PropertyField(s_myScale);
        serializedObject.ApplyModifiedProperties();
    }
}
