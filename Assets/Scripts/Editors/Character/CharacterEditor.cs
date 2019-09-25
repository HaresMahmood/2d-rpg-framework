using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Character)), CanEditMultipleObjects]
public class CharacterEditor : Editor
{
    #region Variables
    private Character character;

    private string characterID;

    private Rect characterSection, portraitSection;
    private Texture2D characterPortrait;
    private float portraitSize = 250f, offset = 10f, margin = 20f;
    #endregion

    private void OnEnable()
    {
        character = (Character)target;

        if (character.id < 10)
            characterID = "00" + character.id.ToString();
        else if (character.id > 10 && character.id < 100)
            characterID = "0" + character.id.ToString();
        else
            characterID = character.id.ToString();

        characterSection.x = 0;
        characterSection.height = portraitSize + margin;

        portraitSection.width = portraitSize * 0.65f;
        portraitSection.height = portraitSize;

    }

    public override void OnInspectorGUI()
    {
        EditorGUIUtility.labelWidth = Screen.width / 4;
        EditorGUIUtility.fieldWidth = Screen.width / 4;

        characterSection.width = Screen.width;
        characterSection.y = 0;

        portraitSection.x = (characterSection.x + (characterSection.width / 2)) - (portraitSize / 3);
        portraitSection.y = characterSection.y + margin;


        if (character.portrait != null)
        {
            characterPortrait = character.portrait.texture;

            int height = Screen.height / 2;

            EditorGUILayout.BeginVertical();
            GUI.Box(characterSection, characterPortrait);
            GUILayout.Space(characterSection.height);
            EditorGUILayout.EndVertical();

            ExtensionMethods.DrawUILine("#969696".ToColor());
        }

        GUILayout.Space(2);

        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("ID");
        char chr = Event.current.character;
        if (chr < '0' || chr > '9')
            Event.current.character = '\0';
        characterID = EditorGUILayout.TextField(characterID);
        if (!characterID.Equals(""))
            character.id = Int32.Parse(characterID);
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



        //base.OnInspectorGUI();
    }
}
