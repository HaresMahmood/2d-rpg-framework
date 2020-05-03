using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TypingUserInterface)), CanEditMultipleObjects]
public class TypingUserInterfaceEditor : Editor
{
    private new TypingUserInterface target;

    private void OnEnable()
    {
        target = (TypingUserInterface)base.target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginVertical("Box");
        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField(new GUIContent("Typing", "Dex number of this Pokémon.\n\n" +
        "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
        target.Typing = (Pokemon.Typing)EditorGUILayout.EnumPopup(target.Typing);

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField(new GUIContent("Sprite", "Dex number of this Pokémon.\n\n" +
        "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));

        //GUILayout.BeginHorizontal();
        //GUILayout.FlexibleSpace();

        GUI.enabled = false;
        target.Icon = (Sprite)EditorGUILayout.ObjectField(target.Icon, typeof(Sprite), false, GUILayout.Width(90), GUILayout.Height(90));
        GUI.enabled = true;

        //GUILayout.FlexibleSpace();
        //GUILayout.EndHorizontal();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }
}
