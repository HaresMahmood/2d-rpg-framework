using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PartyMember)), CanEditMultipleObjects]
public class PartyMemberEditor : Editor
{
    #region Variables

    private new PartyMember target;

    private static bool showDexInfo = true;
    private static bool showNoName = true;
    private static bool showProgression = false;
    private static bool showAbility = true;
    private static bool showStats = false;

    #endregion

    private void OnEnable()
    {
        target = (PartyMember)base.target;
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

        if (target.Species != null)
        {
            if (target.Species.Sprites.MenuSprite != null)
            {
                Texture2D itemSprite = target.Species.Sprites.MenuSprite.texture;
                GUILayout.Label(itemSprite, GUILayout.Width(30), GUILayout.Height(30));
            }

            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            GUILayout.Label($"#{target.Species.ID:000} - {(target.Nickname != "" ? target.Nickname : target.Species.Name)} - The {target.Species.Category} Pokémon -");

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal("Box", GUILayout.Height(29));
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            GUILayout.Label(target.Species.PrimaryType.ToString());

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            if (!target.Species.SecondaryType.Equals(Pokemon.Typing.None))
            {
                GUILayout.BeginHorizontal("Box", GUILayout.Height(29));
                GUILayout.BeginVertical();
                GUILayout.FlexibleSpace();

                GUILayout.Label(target.Species.SecondaryType.ToString());

                GUILayout.FlexibleSpace();
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
        }
        else
        {
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            GUILayout.Label("Add a species via the \"Species\" option..");

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndHorizontal();

        showDexInfo = EditorGUILayout.Foldout(showDexInfo, "Basic Information", foldoutStyle);
        GUILayout.Space(5);

        if (showDexInfo)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginVertical();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Species", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Species = (Pokemon)EditorGUILayout.ObjectField(target.Species, typeof(Pokemon), false);

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();

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
            GUILayout.EndVertical();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Gender", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));

            GUILayout.BeginHorizontal();

            target.Gender.Value = (PartyMember.MemberGender.Gender)EditorGUILayout.EnumPopup(target.Gender.Value);
            EditorStyles.textField.wordWrap = true;

            if (GUILayout.Button("Random", GUILayout.Height(18)))
            {
                target.Gender.Value = target.Gender.AssignGender(target.Species);
            }

            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(2);
        ExtensionMethods.DrawUILine("#525252".ToColor());
        GUILayout.Space(2);

        showAbility = EditorGUILayout.Foldout(showAbility, "Ability", foldoutStyle);
        GUILayout.Space(5);

        if (showAbility)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginVertical();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();

            /*
            EditorGUILayout.LabelField(new GUIContent("Leveling Group", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Progression.Group = (Pokemon.PokemonProgression.LevelingGroup)EditorGUILayout.EnumPopup(target.Progression.Group);
            */

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(2);
        ExtensionMethods.DrawUILine("#525252".ToColor());
        GUILayout.Space(2);

        showProgression = EditorGUILayout.Foldout(showProgression, "Progression", foldoutStyle);
        GUILayout.Space(5);

        if (showProgression)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginVertical();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Leveling Group", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            //target.Progression.Group = (Pokemon.PokemonProgression.LevelingGroup)EditorGUILayout.EnumPopup(target.Progression.Group);

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(2);
        ExtensionMethods.DrawUILine("#525252".ToColor());
        GUILayout.Space(2);

        showStats = EditorGUILayout.Foldout(showStats, "Base Stats", foldoutStyle);
        GUILayout.Space(5);

        if (showStats)
        {
            /*
            GUILayout.BeginHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginVertical();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Hit Points", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Stats.BaseStats[Pokemon.Stat.HP] = Mathf.Max(0, EditorGUILayout.IntField(target.Stats.BaseStats[Pokemon.Stat.HP]));

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Attack", "Dex number of this Pokémon.\n\n" +
           "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Stats.BaseStats[Pokemon.Stat.Attack] = Mathf.Max(0, EditorGUILayout.IntField(target.Stats.BaseStats[Pokemon.Stat.Attack]));

            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Defence", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Stats.BaseStats[Pokemon.Stat.Defence] = Mathf.Max(0, EditorGUILayout.IntField(target.Stats.BaseStats[Pokemon.Stat.Defence]));

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Sp. Attack", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Stats.BaseStats[Pokemon.Stat.SpAttack] = Mathf.Max(0, EditorGUILayout.IntField(target.Stats.BaseStats[Pokemon.Stat.SpAttack]));

            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Sp. Defence", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Stats.BaseStats[Pokemon.Stat.SpDefence] = Mathf.Max(0, EditorGUILayout.IntField(target.Stats.BaseStats[Pokemon.Stat.SpDefence]));

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Speed", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Stats.BaseStats[Pokemon.Stat.Speed] = Mathf.Max(0, EditorGUILayout.IntField(target.Stats.BaseStats[Pokemon.Stat.Speed]));

            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Total", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            EditorGUILayout.SelectableLabel(target.Stats.BaseStats.Sum(stat => stat.Value).ToString(), EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Happiness", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Stats.BaseHappiness = EditorGUILayout.IntSlider(target.Stats.BaseHappiness, 0, 255);

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            */
        }

        GUILayout.Space(2);
        ExtensionMethods.DrawUILine("#525252".ToColor());
        GUILayout.Space(2);

        EditorUtility.SetDirty(target);

        //base.OnInspectorGUI();
    }

    /*
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
    target.Progression.Level = EditorGUILayout.IntSlider(target.Progression.Level, 1, 100);

    GUILayout.BeginHorizontal();

    if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
    {
        Mathf.Clamp(++target.Progression.Level, 1, 100);
    }

    if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
    {
        Mathf.Clamp(--target.Progression.Level, 1, 100);
    }

    GUILayout.EndHorizontal();
    GUILayout.EndHorizontal();
    GUILayout.EndVertical();
    GUILayout.BeginVertical("Box");
    GUILayout.BeginHorizontal();

    EditorGUILayout.LabelField(new GUIContent("Experience", "Category of this Pokémon.\n\n" +
    "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
    target.Progression.Value = EditorGUILayout.IntSlider(target.Progression.Value, 0, target.Progression.Remaining);

    if (target.Progression.Value == target.Progression.Remaining && target.Progression.Level < 100)
    {
        if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
        {
            Mathf.Clamp(++target.Progression.Level, 1, 100);
        }
    }

    GUILayout.EndHorizontal();

    GUILayout.BeginHorizontal();
    GUILayout.BeginHorizontal();

    GUILayout.Space(1);
    EditorGUILayout.LabelField(new GUIContent("Group", "Category of this Pokémon.\n\n" +
    "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(40));
    target.Progression.Group = (Pokemon.PokemonProgression.LevelingGroup)EditorGUILayout.EnumPopup(target.Progression.Group);

    GUILayout.EndHorizontal();
    GUILayout.BeginHorizontal();

    EditorGUILayout.LabelField(new GUIContent("Total", "Category of this Pokémon.\n\n" +
    "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(40));
    EditorGUILayout.SelectableLabel(target.Progression.Total.ToString(), EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));

    GUILayout.EndHorizontal();
    GUILayout.BeginHorizontal();

    EditorGUILayout.LabelField(new GUIContent("Remaining", "Category of this Pokémon.\n\n" +
    "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(65));
    EditorGUILayout.SelectableLabel((target.Progression.Level < 100 && target.Progression.Level > 0) ? target.Progression.Remaining.ToString() : "-", EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));

    GUILayout.EndHorizontal();
    GUILayout.EndHorizontal();
    GUILayout.EndVertical();
    GUILayout.BeginVertical("Box");
    GUILayout.BeginHorizontal();

    EditorGUILayout.LabelField(new GUIContent("Met At Level", "Category of this Pokémon.\n\n" +
    "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
    target.MetAt = EditorGUILayout.IntSlider(target.MetAt, 1, target.Progression.Level);

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
}
