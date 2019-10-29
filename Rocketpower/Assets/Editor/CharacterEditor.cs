using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(CCTest))]
public class CharacterEditor : Editor
{
    private const float editorOffset = 0.6f;
    private Animator animator;
    private CharacterController cc;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        // CCTest controller = (CCTest)target;
        // controller.anim = EditorExtensions.ObjectField<Animator>("test", animator, Screen.width * editorOffset);
        // controller.controller = EditorExtensions.ObjectField<CharacterController>("charactercontroller", cc, Screen.width * editorOffset);

        if (GUILayout.Button("generate"))
        {

        }

    }
}
