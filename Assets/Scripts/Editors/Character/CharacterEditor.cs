using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Character)), CanEditMultipleObjects]
public class CharacterEditor : Editor
{
    private Character character;

    private Rect characterSection, portraitSection;

    private Texture2D characterPortrait;

    private float portraitSize = 150f, offset = 10f, margin = 20f;
    

    private void OnEnable()
    {
        character = (Character)target;

        characterSection.x = 0;
        characterSection.height = 100;

        portraitSection.width = portraitSize * 0.65f;
        portraitSection.height = portraitSize;
    }

    public override void OnInspectorGUI()
    {

        EditorGUIUtility.labelWidth = Screen.width / 4;
        EditorGUIUtility.fieldWidth = Screen.width / 4;

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("ID");
        character.id = EditorGUILayout.IntField(character.id);
        EditorUtility.SetDirty(target);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(2);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Name");
        character.name = EditorGUILayout.TextField(character.name);
        EditorUtility.SetDirty(target);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(2);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Gender");
        character.gender = (Character.Gender)EditorGUILayout.EnumPopup(character.gender);
        EditorUtility.SetDirty(target);
        EditorGUILayout.EndHorizontal();


        GUILayout.Space(2);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Portrait");
        character.portrait = (Sprite)EditorGUILayout.ObjectField(character.portrait, typeof(Sprite), false);
        EditorUtility.SetDirty(target);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        GUILayout.Space(characterSection.height + offset);

        characterSection.width = Screen.width;
        characterSection.y = Screen.height - characterSection.height;

        portraitSection.x = (characterSection.x + (characterSection.width / 2)) - (portraitSize / 3);
        portraitSection.y = characterSection.y + 30;

        if (character.portrait != null)
        {
            characterPortrait = character.portrait.texture;

            EditorGUILayout.BeginVertical();
            GUI.Box(characterSection, characterPortrait);
            EditorGUI.DrawRect(characterSection, Color.grey);
            EditorGUI.DrawPreviewTexture(portraitSection, characterPortrait);
            EditorGUILayout.EndVertical();
            GUILayout.Space(characterSection.height + offset);
        }

        //base.OnInspectorGUI();
    }
}
