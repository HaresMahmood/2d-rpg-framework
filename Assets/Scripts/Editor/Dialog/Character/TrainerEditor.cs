using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
///
/// </summary>
[CustomEditor(typeof(Trainer)), CanEditMultipleObjects]
public class TrainerEditor : CharacterEditor
{
    #region Variables

    private new Trainer target;

    private static bool showBattleData = false;

    #endregion

    #region Miscellaneous Methods

    private void OnEnable()
    {
        target = (Trainer)base.target;
    }

    public override void OnInspectorGUI()
    {
        DrawInspector(target);

        EditorUtility.SetDirty(target);
    }

    public override void DrawInspector(Character target)
    {
        GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout)
        {
            fontStyle = FontStyle.Bold,
            fontSize = 16
        };

        base.DrawInspector(target);

        showBattleData = EditorGUILayout.Foldout(showBattleData, "Battle Data", foldoutStyle);
        GUILayout.Space(5);

        if (showBattleData)
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
        GUILayout.Space(2);
    }

    #endregion
}

