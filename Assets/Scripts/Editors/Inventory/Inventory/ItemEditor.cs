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

        showBasicInfo = EditorGUILayout.Foldout(showBasicInfo, "Basic Information", foldoutStyle);
        GUILayout.Space(5);

        /*
        if (showBasicInfo)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(new GUIContent("Species", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Species = (Pokemon)EditorGUILayout.ObjectField(target.Species, typeof(Pokemon), false);

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(new GUIContent("Nickname", "Name of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));

            GUILayout.BeginVertical();

            showNoName = GUILayout.Toggle(showNoName, "None");


            if (!showNoName)
            {
                GUILayout.BeginHorizontal();

                target.Nickname = EditorGUILayout.TextField(target.Nickname);

                GUILayout.EndHorizontal();
            }
            else
            {
                target.Nickname = "";
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(new GUIContent("HP", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Stats.HP = EditorGUILayout.IntSlider(target.Stats.HP, 0, target.Stats.Stats[Pokemon.Stat.HP]);

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(new GUIContent("Gender", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Gender.Value = (PartyMember.MemberGender.Gender)EditorGUILayout.EnumPopup(target.Gender.Value);
            EditorStyles.textField.wordWrap = true;

            if (target.Gender.Value != PartyMember.MemberGender.Gender.None)
            {
                if (GUILayout.Button("Random", GUILayout.Width(100), GUILayout.Height(18)))
                {
                    target.Gender.Value = target.Gender.AssignRandom(target.Species);
                }
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(new GUIContent("Held Item", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.HeldItem = (Holdable)EditorGUILayout.ObjectField(target.HeldItem, typeof(Holdable), false);

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(new GUIContent("Poké Ball", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.PokeBall = (PokeBall)EditorGUILayout.ObjectField(target.PokeBall, typeof(PokeBall), false);

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
        */

        GUILayout.Space(2);
        ExtensionMethods.DrawUILine("#525252".ToColor());
        GUILayout.Space(2);

        EditorUtility.SetDirty(target);

        base.OnInspectorGUI();
    }
}

