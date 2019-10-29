using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CCTest))]
public class EditorTool : EditorWindow
{
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;


    SerializedProperty m_IntProp;
    SerializedProperty m_VectorProp;
    SerializedProperty m_GameObjectProp;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/My Window")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        EditorTool window = (EditorTool)EditorWindow.GetWindow(typeof(EditorTool));
        window.Show();
    }

    void OnEnable()
    {
        // Fetch the objects from the GameObject script to display in the inspector

    }
}
