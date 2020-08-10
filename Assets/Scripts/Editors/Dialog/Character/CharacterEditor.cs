﻿using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Character)), CanEditMultipleObjects]
public class CharacterEditor : Editor
{
    #region Variables

    private new Character target;

    private static bool showBasicInfo = true;

    #endregion

    private void OnEnable()
    {
        target = (Character)base.target;
    }

    public override void OnInspectorGUI()
    {
        GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout)
        {
            fontStyle = FontStyle.Bold,
            fontSize = 16
        };

        GUILayout.BeginVertical("Box", GUILayout.Height(35));
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        GUILayout.Label($"#{target.ID:000} - {(target.Name == "" ? "Give this character a name via Basic Information" : target.Name)}");

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();

        GUILayout.Space(2);
        ExtensionMethods.DrawUILine("#525252".ToColor());
        GUILayout.Space(2);

        showBasicInfo = EditorGUILayout.Foldout(showBasicInfo, "Basic Information", foldoutStyle);
        GUILayout.Space(5);

        if (showBasicInfo)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(new GUIContent("ID", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.ID = int.Parse(EditorGUILayout.TextField(target.ID.ToString("000")));

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(new GUIContent("Name", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Name = EditorGUILayout.TextField(target.Name);

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(new GUIContent("Gender", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Gender = (Character.CharacterGender)EditorGUILayout.EnumPopup(target.Gender);
            EditorStyles.textField.wordWrap = true;

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(2);
        ExtensionMethods.DrawUILine("#525252".ToColor());

        EditorUtility.SetDirty(target);

        //base.OnInspectorGUI();
    }

    /*
    #region Variables
    private Character character;

    private string characterID = "000";

    private Rect characterSection, portraitSection;
    private Texture2D characterPortrait;
    private float portraitSize = 250f, offset = 10f, margin = 20f;

    private string[] characterGuids;
    private List<Character> characterObjs = new List<Character>();

    private bool uniqueID = true;

    #endregion

    private void OnEnable()
    {
        character = (Character)target;

        SetID();
        SetName();

        characterSection.x = 0;
        characterSection.height = portraitSize + margin;

        portraitSection.width = portraitSize * 0.65f;
        portraitSection.height = portraitSize;

        characterGuids = AssetDatabase.FindAssets("t:Character", null);
        foreach (string characterGuid in characterGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(characterGuid);
            if (!string.Equals(AssetDatabase.GetAssetPath(target.GetInstanceID()), assetPath))
            {
                Character character = (Character)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Character));
                characterObjs.Add(character);
            }
        }
    }

    public override void OnInspectorGUI()
    {
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

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Preview");
            EditorGUILayout.EndVertical();

            GUILayout.Space(characterSection.height - margin + 2);
            EditorGUILayout.EndVertical();

            ExtensionMethods.DrawUILine("#969696".ToColor());
        }

        GUILayout.Space(2);

        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("ID number", "Number that defines this character. " +
            "Must be unique for every character. used internally by engine. Must be only 3 digits."));

        char chr = Event.current.character;
        if (chr < '0' || chr > '9')
            Event.current.character = '\0';
        EditorGUI.BeginChangeCheck();
        characterID = EditorGUILayout.TextField(characterID);

        if (!characterID.Equals(""))
            character.id = Int32.Parse(characterID);

        foreach (Character character in characterObjs)
        {
            if (character.id == this.character.id)
                uniqueID = false;
            else
                uniqueID = true;
        }
        if (EditorGUI.EndChangeCheck() && !uniqueID && EditorUtility.DisplayDialog("ID: " + characterID + " has " +
        "already been taken.",
        "Please fill in a unique ID for this character.",
        "Okay"))
            characterID = "000";

        SetID();
        EditorUtility.SetDirty(target);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(2);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Name", "How this character is refered to during during " +
            "and, if applicable, battles."));
        character.name = EditorGUILayout.TextField(character.name);
        EditorUtility.SetDirty(target);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(2);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Gender", "Gender with which character is addressed. " +
            "Select 'Mixed' for I.E. a character representing a pair of triners"));
        character.gender = (Character.CharacterGender)EditorGUILayout.EnumPopup(character.gender);
        EditorUtility.SetDirty(target);
        EditorGUILayout.EndHorizontal();


        GUILayout.Space(2);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Portrait", "Sprite shown during inventory."));
        character.portrait = (Sprite)EditorGUILayout.ObjectField(character.portrait, typeof(Sprite), false);
        if (character.id == 0 && character.portrait != null)
            SetFilenameID(character.portrait);
        EditorUtility.SetDirty(target);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }

    private string SetID()
    {
        if (character.id < 10)
            characterID = "00" + character.id.ToString();
        else if (character.id > 10 && character.id < 100)
            characterID = "0" + character.id.ToString();
        else
            characterID = character.id.ToString();

        return characterID;
    }

    private void SetName()
    {
        string assetPath = AssetDatabase.GetAssetPath(target.GetInstanceID());
        character.name = Path.GetFileNameWithoutExtension(assetPath);
        EditorUtility.SetDirty(target);
    }

    private void SetFilenameID(Sprite portrait)
    {
        string id = portrait.name.Substring(3, 3);
        characterID = id;
        character.id = Int32.Parse(id);
        EditorUtility.SetDirty(target);
    }
    */
}
