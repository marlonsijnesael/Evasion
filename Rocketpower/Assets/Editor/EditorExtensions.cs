
using UnityEngine;
using UnityEditor;

/// <summary>
/// The following functions are made to keep the code inside the OnGUI method readable and most of them are not project specific and are reusabale
/// All functions use the screen.width as a scaler to keep the UI responsive, screen.width returns a width relative to the width of the current editor window
/// The main point of this is to do most of the scaling, UI design and input outside of the editor window. I put it inside of this class because I could not extend the editor scripts from unity directly
/// </summary>
public class EditorExtensions
{

    //creates a textfield and a label, returns the input text to the given text parameter
    public static string GetText(string _name, string _text)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(_name, GUILayout.Width(Screen.width / 2));
        string _newText = EditorGUILayout.TextField(_text, GUILayout.Width(Screen.width * 0.45f));
        EditorGUILayout.EndHorizontal();
        return _newText;
    }

    //returns the path of _targetfolder parameter
    public static string GetPath(string _name, string _path, DefaultAsset _targetFolder)
    {
        return AssetDatabase.GetAssetPath(_targetFolder);
    }

    //returns the value of a toggle box
    public static bool GetToggle(string _name, bool _bool)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(_name, GUILayout.Width(Screen.width / 3));
        bool _newBool = EditorGUILayout.Toggle("", _bool, GUILayout.Width(Screen.width * 0.45f));
        EditorGUILayout.EndHorizontal();
        return _newBool;
    }

    //returns the selected gameObject
    public static T ObjectField<T>(string _name, T _param, float _offset) where T : Object
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(_name, GUILayout.Width(Screen.width / 3));

        T newObject = (T)EditorGUILayout.ObjectField(_param, typeof(T), true, GUILayout.Width(_offset)) as T;
        EditorGUILayout.EndHorizontal();
        return newObject;
    }

    //returns sprite

}
