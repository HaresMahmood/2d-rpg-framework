﻿using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Pokemon)), CanEditMultipleObjects]
public class PokemonEditor : Editor
{
    #region Variables

    private new Pokemon target;

    GUIStyle foldoutStyle;

    private float margin = 15f;

    private static bool showBasicInfo = true;
    private static bool showProgression = true;
    private static bool showStats = true;

    #endregion

    private void OnEnable()
    {
        target = (Pokemon)base.target;
    }

    public override void OnInspectorGUI()
    {
        foldoutStyle = new GUIStyle(EditorStyles.foldout)
        {
            fontStyle = FontStyle.Bold,
            fontSize = 16
        };

        GUILayout.BeginHorizontal();
        GUILayout.BeginHorizontal("Box", GUILayout.Height(35));
        GUILayout.FlexibleSpace();

        if (target.Sprites.MenuSprite != null)
        {
            Texture2D itemSprite = target.Sprites.MenuSprite.texture;
            GUILayout.Label(itemSprite, GUILayout.Width(30), GUILayout.Height(30));
        }

        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();

        GUILayout.Label($"#{target.ID:000} - {target.Name} - The {target.Category} Pokémon -");

        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();

        GUILayout.BeginHorizontal("Box", GUILayout.Height(29));
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();

        GUILayout.Label(target.PrimaryType.ToString());

        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        if (!target.SecondaryType.Equals(Pokemon.Typing.None))
        {
            GUILayout.BeginHorizontal("Box", GUILayout.Height(29));
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            GUILayout.Label(target.SecondaryType.ToString());

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndHorizontal();

        GUILayout.Space(2);
        ExtensionMethods.DrawUILine("#525252".ToColor());
        GUILayout.Space(2);

        showBasicInfo = EditorGUILayout.Foldout(showBasicInfo, "Basic Information", foldoutStyle);

        if (showBasicInfo)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(margin);
            GUILayout.BeginVertical();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("#", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.ID = int.Parse(EditorGUILayout.TextField(target.ID.ToString("000")));

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Name", "Name of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Name = EditorGUILayout.TextField(target.Name);

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Category", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Category = EditorGUILayout.TextField(target.Category);

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Typing", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.PrimaryType = (Pokemon.Typing)EditorGUILayout.EnumPopup(target.PrimaryType);
            target.SecondaryType = (Pokemon.Typing)EditorGUILayout.EnumPopup(target.SecondaryType);

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Dex Entry", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.DexEntry = EditorGUILayout.TextArea(target.DexEntry, GUILayout.MaxHeight(35));
            EditorStyles.textField.wordWrap = true;

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(2);
        ExtensionMethods.DrawUILine("#525252".ToColor());
        GUILayout.Space(2);

        showProgression = EditorGUILayout.Foldout(showProgression, "Progression", foldoutStyle);

        if (showProgression)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(margin);
            GUILayout.BeginVertical();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Level", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Experience.Level = EditorGUILayout.IntSlider(target.Experience.Level, 1, 100);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
            {
                Mathf.Clamp(++target.Experience.Level, 1, 100);
            }

            if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
            {
                Mathf.Clamp(--target.Experience.Level, 1, 100);
            }

            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Experience", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Experience.Value = EditorGUILayout.IntSlider(target.Experience.Value, 0, target.Experience.Remaining);

            if (target.Experience.Value == target.Experience.Remaining && target.Experience.Level < 100)
            {
                if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
                {
                    Mathf.Clamp(++target.Experience.Level, 1, 100);
                }
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.BeginHorizontal();

            GUILayout.Space(1);
            EditorGUILayout.LabelField(new GUIContent("Group", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(40));
            target.Experience.Group = (Pokemon.PokemonExperience.ExperienceGroup)EditorGUILayout.EnumPopup(target.Experience.Group);

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Total", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(40));
            EditorGUILayout.SelectableLabel(target.Experience.Total.ToString(), EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Remaining", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(65));
            EditorGUILayout.SelectableLabel((target.Experience.Level < 100 && target.Experience.Level > 0) ? target.Experience.Remaining.ToString() : "-", EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));

            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Met At Level", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.MetAt = EditorGUILayout.IntSlider(target.MetAt, 1, target.Experience.Level);

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Location", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            EditorGUILayout.TextField("");

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(2);
        ExtensionMethods.DrawUILine("#525252".ToColor());
        GUILayout.Space(2);

        /*
        showBasicInfo = EditorGUILayout.Foldout(showBasicInfo, "Abilities", foldoutStyle);

        if (showBasicInfo)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(margin);
            GUILayout.BeginVertical();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Ability", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            //target.Level = EditorGUILayout.IntSlider(target.Level, 0, 100);

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Description", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            EditorGUILayout.TextArea(target.DexEntry, GUILayout.MaxHeight(35));

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Nature", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Nature.Value = (Pokemon.PokemonNature.Nature)EditorGUILayout.EnumPopup(target.Nature.Value);

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(2);
        ExtensionMethods.DrawUILine("#525252".ToColor());
        GUILayout.Space(2);

        showBasicInfo = EditorGUILayout.Foldout(showBasicInfo, "Held Item", foldoutStyle);

        if (showBasicInfo)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(margin);
            GUILayout.BeginVertical();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Item", "Item this Pokémon is holding.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.HeldItem = (Holdable)EditorGUILayout.ObjectField(target.HeldItem, typeof(Holdable), false);

            GUILayout.EndHorizontal();

            if (target.HeldItem != null)
            {
                GUILayout.FlexibleSpace();
                GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(new GUIContent("Effect", "Category of this Pokémon.\n\n" +
                "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(50));
                EditorGUILayout.SelectableLabel(target.HeldItem.Effect.ToString(), EditorStyles.textField, GUILayout.Width(65), GUILayout.Height(EditorGUIUtility.singleLineHeight));

                GUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
            }

            GUILayout.EndHorizontal();

            if (target.HeldItem != null)
            {
                GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(new GUIContent("Description"), GUILayout.Width(95));
                EditorGUILayout.SelectableLabel(target.HeldItem.Description, EditorStyles.textArea, GUILayout.MaxHeight(35));

                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(2);
        ExtensionMethods.DrawUILine("#525252".ToColor());
        GUILayout.Space(2);
        */

        EditorUtility.SetDirty(target);

        //base.OnInspectorGUI();
    }
}
