using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Pokemon)), CanEditMultipleObjects]
public class PokemonEditor : Editor
{
    #region Variables

    private new Pokemon target;

    private static bool showDexInfo = true;
    private static bool showMeasurements = false;
    private static bool showProgression = false;
    private static bool showAbility = false;
    private static bool showStats = false;
    private static bool showYield = false;
    private static bool showSprites = false;

    private float female;
    private int feet, inches;
    private double pounds;

    #endregion

    private void OnEnable()
    {
        target = (Pokemon)base.target;
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

        showDexInfo = EditorGUILayout.Foldout(showDexInfo, "Dex Information", foldoutStyle);
        GUILayout.Space(5);

        if (showDexInfo)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(new GUIContent("#", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.ID = int.Parse(EditorGUILayout.TextField(target.ID.ToString("000")));

            GUILayout.EndHorizontal();
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
            GUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(new GUIContent("Typing", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.PrimaryType = (Pokemon.Typing)EditorGUILayout.EnumPopup(target.PrimaryType);
            target.SecondaryType = (Pokemon.Typing)EditorGUILayout.EnumPopup(target.SecondaryType);

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(new GUIContent("Dex Entry", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.DexEntry = EditorGUILayout.TextArea(target.DexEntry, GUILayout.MaxHeight(35));
            EditorStyles.textField.wordWrap = true;

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(new GUIContent("Gender Ratio", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Male", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(55));

            GUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();

            target.GenderRatio = EditorGUILayout.Slider(target.GenderRatio, 0f, 100f);
            EditorGUILayout.LabelField("%", GUILayout.Width(20));

            if (EditorGUI.EndChangeCheck())
            {
                female = 100f - target.GenderRatio;
            }

            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Female", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(55));

            GUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();

            female = EditorGUILayout.Slider(female, 0f, 100f);
            EditorGUILayout.LabelField("%", GUILayout.Width(20));

            if (EditorGUI.EndChangeCheck())
            {
                target.GenderRatio = 100f - female;
            }
            else
            {
                female = female == 100f - target.GenderRatio ? female : 100f - target.GenderRatio;
            }

            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(new GUIContent("Catch Rate", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.CatchRate = EditorGUILayout.IntSlider(target.CatchRate, 1, 255);

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(2);
        ExtensionMethods.DrawUILine("#525252".ToColor());
        GUILayout.Space(2);

        showMeasurements = EditorGUILayout.Foldout(showMeasurements, "Measurements", foldoutStyle);
        GUILayout.Space(5);

        if (showMeasurements)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(new GUIContent("Height", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));

            GUILayout.BeginHorizontal();
            GUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();

            target.Measurements.Height = EditorGUILayout.DoubleField(Math.Round(target.Measurements.Height, 1));
            EditorGUILayout.LabelField("m", GUILayout.Width(55));

            if (EditorGUI.EndChangeCheck())
            {
                (feet, inches) = ToFeet(target.Measurements.Height);
            }

            GUILayout.EndHorizontal();

            EditorGUILayout.LabelField(new GUIContent("=", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(55));

            GUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            GUILayout.BeginHorizontal();

            feet = EditorGUILayout.IntField(feet);
            EditorGUILayout.LabelField("'", GUILayout.Width(10));

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            inches = EditorGUILayout.IntField(inches);
            EditorGUILayout.LabelField("\"", GUILayout.Width(10));

            GUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                target.Measurements.Height = toMeters(feet, inches);
            }
            else
            {
                (feet, inches) = (feet, inches) == ToFeet(Math.Round(target.Measurements.Height, 1)) ? (feet, inches) : ToFeet(target.Measurements.Height);
            }

            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(new GUIContent("Weight", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));

            GUILayout.BeginHorizontal();
            GUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();

            target.Measurements.Weight = EditorGUILayout.DoubleField(Math.Round(target.Measurements.Weight, 1));
            EditorGUILayout.LabelField("kg", GUILayout.Width(55));

            if (EditorGUI.EndChangeCheck())
            {
                pounds = ToPounds(target.Measurements.Weight);
            }

            GUILayout.EndHorizontal();

            EditorGUILayout.LabelField(new GUIContent("=", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(55));

            GUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            GUILayout.BeginHorizontal();

            pounds = EditorGUILayout.DoubleField(pounds);
            EditorGUILayout.LabelField("lbs", GUILayout.Width(20));

            GUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                target.Measurements.Weight = ToKilograms(pounds);
            }
            else
            {
                pounds = pounds == ToPounds(target.Measurements.Weight) ? pounds : ToPounds(target.Measurements.Weight);
            }

            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
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
            GUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(new GUIContent("Leveling Group", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Progression.Group = (Pokemon.PokemonProgression.LevelingGroup)EditorGUILayout.EnumPopup(target.Progression.Group);

            GUILayout.EndHorizontal();
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
            GUI.enabled = false;
            EditorGUILayout.SelectableLabel(target.Stats.BaseStats.Sum(stat => stat.Value).ToString(), EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
            GUI.enabled = true;

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
        }

        GUILayout.Space(2);
        ExtensionMethods.DrawUILine("#525252".ToColor());
        GUILayout.Space(2);

        showYield = EditorGUILayout.Foldout(showYield, "Base Yield", foldoutStyle);
        GUILayout.Space(5);

        if (showYield)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(new GUIContent("Experience", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Yield.Experience = EditorGUILayout.IntSlider(target.Yield.Experience, 0, 750);

            GUILayout.EndHorizontal();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("EV", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("HP", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(55));

            if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Yield.EV[Pokemon.Stat.HP] = Mathf.Clamp(++target.Yield.EV[Pokemon.Stat.HP], 0, 3);
            }

            target.Yield.EV[Pokemon.Stat.HP] = EditorGUILayout.IntField(target.Yield.EV[Pokemon.Stat.HP], GUILayout.Width(18));

            if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Yield.EV[Pokemon.Stat.HP] = Mathf.Clamp(--target.Yield.EV[Pokemon.Stat.HP], 0, 3);
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("ATK", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(55));

            if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Yield.EV[Pokemon.Stat.Attack] = Mathf.Clamp(++target.Yield.EV[Pokemon.Stat.Attack], 0, 3);
            }

            target.Yield.EV[Pokemon.Stat.Attack] = EditorGUILayout.IntField(target.Yield.EV[Pokemon.Stat.Attack], GUILayout.Width(18));

            if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Yield.EV[Pokemon.Stat.Attack] = Mathf.Clamp(--target.Yield.EV[Pokemon.Stat.Attack], 0, 3);
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("DEF", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(55));

            if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Yield.EV[Pokemon.Stat.Defence] = Mathf.Clamp(++target.Yield.EV[Pokemon.Stat.Defence], 0, 3);
            }

            target.Yield.EV[Pokemon.Stat.Defence] = EditorGUILayout.IntField(target.Yield.EV[Pokemon.Stat.Defence], GUILayout.Width(18));

            if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Yield.EV[Pokemon.Stat.Defence] = Mathf.Clamp(--target.Yield.EV[Pokemon.Stat.Defence], 0, 3);
            }

            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("SP. ATK", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(55));

            if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Yield.EV[Pokemon.Stat.SpAttack] = Mathf.Clamp(++target.Yield.EV[Pokemon.Stat.SpAttack], 0, 3);
            }

            target.Yield.EV[Pokemon.Stat.SpAttack] = EditorGUILayout.IntField(target.Yield.EV[Pokemon.Stat.SpAttack], GUILayout.Width(18));

            if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Yield.EV[Pokemon.Stat.SpAttack] = Mathf.Clamp(--target.Yield.EV[Pokemon.Stat.SpAttack], 0, 3);
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("SP. DEF", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(55));

            if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Yield.EV[Pokemon.Stat.SpDefence] = Mathf.Clamp(++target.Yield.EV[Pokemon.Stat.SpDefence], 0, 3);
            }

            target.Yield.EV[Pokemon.Stat.SpDefence] = EditorGUILayout.IntField(target.Yield.EV[Pokemon.Stat.SpDefence], GUILayout.Width(18));

            if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Yield.EV[Pokemon.Stat.SpDefence] = Mathf.Clamp(--target.Yield.EV[Pokemon.Stat.SpDefence], 0, 3);
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("SPD", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(55));

            if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Yield.EV[Pokemon.Stat.Speed] = Mathf.Clamp(++target.Yield.EV[Pokemon.Stat.Speed], 0, 3);
            }

            target.Yield.EV[Pokemon.Stat.Speed] = EditorGUILayout.IntField(target.Yield.EV[Pokemon.Stat.Speed], GUILayout.Width(18));

            if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Yield.EV[Pokemon.Stat.Speed] = Mathf.Clamp(--target.Yield.EV[Pokemon.Stat.Speed], 0, 3);
            }

            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Total", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            GUI.enabled = false;
            EditorGUILayout.SelectableLabel(target.Yield.EV.Sum(stat => stat.Value).ToString(), EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
            GUI.enabled = true;

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
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
            int width = Screen.width / 2 - 30;

            GUILayout.BeginHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(new GUIContent("Menu", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Sprites.MenuSprite = (Sprite)EditorGUILayout.ObjectField(target.Sprites.MenuSprite, typeof(Sprite), false);

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal("Box");
            GUILayout.BeginVertical();

            EditorGUILayout.LabelField(new GUIContent("Front", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Sprites.FrontSprite = (Sprite)EditorGUILayout.ObjectField(target.Sprites.FrontSprite, typeof(Sprite), false, GUILayout.Width(width), GUILayout.Height(width));

            GUILayout.EndVertical();
            GUILayout.BeginVertical();

            EditorGUILayout.LabelField(new GUIContent("Back", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Sprites.BackSprite = (Sprite)EditorGUILayout.ObjectField(target.Sprites.BackSprite, typeof(Sprite), false, GUILayout.Width(width), GUILayout.Height(width));

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(2);
        ExtensionMethods.DrawUILine("#525252".ToColor());
        GUILayout.Space(2);

        EditorUtility.SetDirty(target);

        //base.OnInspectorGUI();
    }

    #region Miscellaneaous Methods

    private (int, int) ToFeet(double value)
    {
        double conversion = value * 39.37f;
        int feet = (int)(conversion / 12);
        int inches = (int)(conversion % 12);

        return (feet, inches);
    }

    private double toMeters(int feet, int inches)
    {
        inches = inches + (feet * 12);
        double meters = inches / 39.37f;

        return meters;
    }

    private double ToPounds(double value)
    {
        double pounds = Math.Round(value * 2.205f, 1);

        return pounds;
    }

    private double ToKilograms(double value)
    {
        double kilograms = value / 2.205f;

        return kilograms;
    }

    #endregion

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
