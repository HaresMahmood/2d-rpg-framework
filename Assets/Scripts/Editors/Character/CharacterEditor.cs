using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Character)), CanEditMultipleObjects]
public class CharacterEditor : Editor
{
    private Character character;

    private Texture2D characterPortrait;
    private Rect portraitSection;
    private float portraitSize = 300f;

    private void OnEnable()
    {
        character = (Character)target;

        characterPortrait = character.portrait.texture;
        portraitSection.x = (0f + (Screen.width / 2f)) - (portraitSize / 2f);
        portraitSection.y = (10f);
        portraitSection.width = portraitSize * 0.7f;
        portraitSection.height = portraitSize;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();

        GUILayout.Space(5);

        EditorGUILayout.BeginVertical("Box");
        GUILayout.Label("Currently editing: " + character.name);
        EditorGUILayout.EndVertical();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField("Character portrait");
        EditorGUILayout.BeginHorizontal();
        GUI.DrawTexture(portraitSection, characterPortrait);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginVertical();
        GUILayout.Space(portraitSize + 10f);
        character.portrait = (Sprite)EditorGUILayout.ObjectField(character.portrait, typeof(Sprite), false);
        
        EditorUtility.SetDirty(target);
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndVertical();

        //base.OnInspectorGUI();
    }
}
