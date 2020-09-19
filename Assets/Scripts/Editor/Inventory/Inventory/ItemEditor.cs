using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
///
/// </summary>
[CustomEditor(typeof(Item)), CanEditMultipleObjects]
public class ItemEditor : Editor
{
    #region Variables

    private new Item target;

    private static bool showBasicInfo = true;
    private static bool showSprites = false;
    private static bool showInventoryInformation = true;

    #endregion

    private void OnEnable()
    {
        target = (Item)base.target;
    }

    public override void OnInspectorGUI()
    {
        GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout)
        {
            fontStyle = FontStyle.Bold,
            fontSize = 16
        };

        GUILayout.BeginHorizontal();
        GUILayout.BeginHorizontal("Box", GUILayout.Height(35));
        GUILayout.FlexibleSpace();

        if (target.Sprite != null)
        {
            Texture2D itemSprite = target.Sprite.texture;
            GUILayout.Label(itemSprite, GUILayout.Width(30), GUILayout.Height(30));
        }

        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();

        GUILayout.Label($"#{target.ID:000} - {(target.Name == "" ? "Enter a name via \"Basic Information\"" : target.Name)} - {target.Categorization.ToString()} Item");

        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndHorizontal();

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

            EditorGUILayout.LabelField(new GUIContent("Dex Entry", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Description = EditorGUILayout.TextArea(target.Description, GUILayout.MaxHeight(35));
            EditorStyles.textField.wordWrap = true;

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(new GUIContent("Category", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            ((Item.ItemCategory)target.Categorization).Value = (Item.ItemCategory.Category)EditorGUILayout.EnumPopup(((Item.ItemCategory)target.Categorization).Value);

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(2);
        ExtensionMethods.DrawUILine("#525252".ToColor());
        GUILayout.Space(2);

        showSprites = EditorGUILayout.Foldout(showSprites, "Sprites", foldoutStyle);
        GUILayout.Space(5);

        if (showSprites)
        {
            int width = Screen.width / 4;

            GUILayout.BeginHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(new GUIContent("Menu", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(35));

            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical();

            target.Sprite = (Sprite)EditorGUILayout.ObjectField(target.Sprite, typeof(Sprite), false, GUILayout.Width(width), GUILayout.Height(width));

            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(2);
        ExtensionMethods.DrawUILine("#525252".ToColor());
        GUILayout.Space(2);

        EditorUtility.SetDirty(target);

        base.OnInspectorGUI();
    }
}

