using System;
using System.Linq;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PartyMember)), CanEditMultipleObjects]
public class PartyMemberEditor : Editor
{
    #region Variables
     
    private new PartyMember target;

    private static bool showDexInfo = true;
    private static bool showStatusAilment = false;
    private static bool showNoName = true;
    private static bool showProgression = false;
    private static bool showAbility = false;
    private static bool showStats = false;
    private static bool showMoves = false;
    private static bool showActiveMoves = false;
    private static bool showLearnedMoves = false;

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

            if (!target.Species.SecondaryType.Equals(Typing.Type.None))
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

            GUILayout.Label("Add a species via Basic Information/\"Species\"");

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
            GUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(new GUIContent("Species", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Species = (Pokemon)EditorGUILayout.ObjectField(target.Species, typeof(Pokemon), false);

            GUILayout.EndHorizontal();

            if (target.Species != null)
            {
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
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(2);
        ExtensionMethods.DrawUILine("#525252".ToColor());
        GUILayout.Space(2);

        showStatusAilment = EditorGUILayout.Foldout(showStatusAilment, "Status Ailment", foldoutStyle);
        GUILayout.Space(5);

        if (showStatusAilment)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(new GUIContent("Status Ailment", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Ailment.Value = (PartyMember.StatusAilment.Ailment)EditorGUILayout.EnumPopup(target.Ailment.Value);

            GUILayout.EndHorizontal();
            GUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Pokérus", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Pokerus.Status = (PartyMember.MemberPokerus.InfectionStatus)EditorGUILayout.EnumPopup(target.Pokerus.Status);

            EditorGUILayout.EndHorizontal();

            if (target.Pokerus.Status == PartyMember.MemberPokerus.InfectionStatus.Infected)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(new GUIContent($"Strain ({Convert.ToChar((target.Pokerus.Strain % 4 + 1) + 64)})", "Dex number of this Pokémon.\n\n" +
                "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
                EditorGUI.BeginChangeCheck();

                int strain = EditorGUILayout.IntSlider(target.Pokerus.Strain, 0, 15);

                if (EditorGUI.EndChangeCheck())
                {
                    target.Pokerus.Strain = strain;
                }

                if (GUILayout.Button("Random", GUILayout.Width(100), GUILayout.Height(18)))
                {
                    target.Pokerus.Strain = UnityEngine.Random.Range(0, 15);
                }

                if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
                {
                    target.Pokerus.Strain = Mathf.Clamp(++target.Pokerus.Strain, 0, 15);
                }

                if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
                {
                    target.Pokerus.Strain = Mathf.Clamp(--target.Pokerus.Strain, 0, 15);
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(new GUIContent("Days Left", "Dex number of this Pokémon.\n\n" +
                "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
                target.Pokerus.Days = EditorGUILayout.IntField(target.Pokerus.Days);

                if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
                {
                    target.Pokerus.Days = Mathf.Clamp(++target.Pokerus.Days, 0, target.Pokerus.Strain % 4 + 1);
                }

                if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
                {
                    target.Pokerus.Days = Mathf.Clamp(--target.Pokerus.Days, 0, target.Pokerus.Strain % 4 + 1);
                }

                EditorGUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            EditorGUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(2);
        ExtensionMethods.DrawUILine("#525252".ToColor());
        GUILayout.Space(2);

        showAbility = EditorGUILayout.Foldout(showAbility, "Ability", foldoutStyle);
        GUILayout.Space(5);

        if (showAbility && target.Species != null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal("Box");

            /*
            EditorGUILayout.LabelField(new GUIContent("Leveling Group", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Progression.Group = (Pokemon.PokemonProgression.LevelingGroup)EditorGUILayout.EnumPopup(target.Progression.Group);
            */

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(2);
        ExtensionMethods.DrawUILine("#525252".ToColor());
        GUILayout.Space(2);

        showProgression = EditorGUILayout.Foldout(showProgression, "Progression", foldoutStyle);
        GUILayout.Space(5);

        if (showProgression && target.Species != null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(new GUIContent("Level", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Progression.Level = EditorGUILayout.IntSlider(target.Progression.Level, 1, 100);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Progression.Level = Mathf.Clamp(++target.Progression.Level, 1, 100);
            }

            if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Progression.Level = Mathf.Clamp(--target.Progression.Level, 1, 100);
            }

            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Experience", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Progression.Value = EditorGUILayout.IntSlider(target.Progression.Value, 0, target.Progression.GetRemaining(target.Species));

            EditorGUI.BeginDisabledGroup(target.Progression.Value != target.Progression.GetRemaining(target.Species) || target.Progression.Level >= 100);

            if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Progression.Level = Mathf.Clamp(++target.Progression.Level, 1, 100);
                target.Progression.Value = 0;
            }

            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.BeginHorizontal();

            GUILayout.Space(1);
            EditorGUILayout.LabelField(new GUIContent("Group", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(40));
            GUI.enabled = false;
            target.Species.Progression.Group = (Pokemon.PokemonProgression.LevelingGroup)EditorGUILayout.EnumPopup(target.Species.Progression.Group);
            GUI.enabled = true;

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Total", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(40));
            GUI.enabled = false;
            EditorGUILayout.SelectableLabel(target.Progression.GetTotal(target.Species).ToString(), EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
            GUI.enabled = true;

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Remaining", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(65));
            GUI.enabled = false;
            EditorGUILayout.SelectableLabel((target.Progression.Level < 100 && target.Progression.Level > 0) ? target.Progression.GetRemaining(target.Species).ToString() : "-", EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
            GUI.enabled = true;

            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Met At Level", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.MetAt.Level = EditorGUILayout.IntSlider(target.MetAt.Level, 1, target.Progression.Level);

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

        showStats = EditorGUILayout.Foldout(showStats, "Stats", foldoutStyle);
        GUILayout.Space(5);

        if (showStats && target.Species != null)
        { 
            GUILayout.BeginHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginVertical();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Stats", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Hit Points", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Stats.Stats[Pokemon.Stat.HP] = target.Stats.CalculateStat(target.Species.Stats.BaseStats[Pokemon.Stat.HP], target.Stats.IVs[Pokemon.Stat.HP], target.Stats.EVs[Pokemon.Stat.HP], target.Progression.Level, target.Nature.ModifiedStat[Pokemon.Stat.HP], true);
            GUI.enabled = false;
            EditorGUILayout.SelectableLabel(target.Stats.Stats[Pokemon.Stat.HP].ToString(), EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
            GUI.enabled = true;

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Attack", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Stats.Stats[Pokemon.Stat.Attack] = target.Stats.CalculateStat(target.Species.Stats.BaseStats[Pokemon.Stat.Attack], target.Stats.IVs[Pokemon.Stat.Attack], target.Stats.EVs[Pokemon.Stat.Attack], target.Progression.Level, target.Nature.ModifiedStat[Pokemon.Stat.Attack], false);
            GUI.enabled = false;
            EditorGUILayout.SelectableLabel(target.Stats.Stats[Pokemon.Stat.Attack].ToString(), EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
            GUI.enabled = true;

            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Defence", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Stats.Stats[Pokemon.Stat.Defence] = target.Stats.CalculateStat(target.Species.Stats.BaseStats[Pokemon.Stat.Defence], target.Stats.IVs[Pokemon.Stat.Defence], target.Stats.EVs[Pokemon.Stat.Defence], target.Progression.Level, target.Nature.ModifiedStat[Pokemon.Stat.Defence], false);
            GUI.enabled = false;
            EditorGUILayout.SelectableLabel(target.Stats.Stats[Pokemon.Stat.Defence].ToString(), EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
            GUI.enabled = true;

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Sp. Attack", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Stats.Stats[Pokemon.Stat.SpAttack] = target.Stats.CalculateStat(target.Species.Stats.BaseStats[Pokemon.Stat.SpAttack], target.Stats.IVs[Pokemon.Stat.SpAttack], target.Stats.EVs[Pokemon.Stat.SpAttack], target.Progression.Level, target.Nature.ModifiedStat[Pokemon.Stat.SpAttack], false);
            GUI.enabled = false;
            EditorGUILayout.SelectableLabel(target.Stats.Stats[Pokemon.Stat.SpAttack].ToString(), EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
            GUI.enabled = true;

            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Sp. Defence", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Stats.Stats[Pokemon.Stat.SpDefence] = target.Stats.CalculateStat(target.Species.Stats.BaseStats[Pokemon.Stat.SpDefence], target.Stats.IVs[Pokemon.Stat.SpDefence], target.Stats.EVs[Pokemon.Stat.SpDefence], target.Progression.Level, target.Nature.ModifiedStat[Pokemon.Stat.SpDefence], false);
            GUI.enabled = false;
            EditorGUILayout.SelectableLabel(target.Stats.Stats[Pokemon.Stat.SpDefence].ToString(), EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
            GUI.enabled = true;

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Speed", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Stats.Stats[Pokemon.Stat.Speed] = target.Stats.CalculateStat(target.Species.Stats.BaseStats[Pokemon.Stat.Speed], target.Stats.IVs[Pokemon.Stat.Speed], target.Stats.EVs[Pokemon.Stat.Speed], target.Progression.Level, target.Nature.ModifiedStat[Pokemon.Stat.Speed], false);
            GUI.enabled = false;
            EditorGUILayout.SelectableLabel(target.Stats.Stats[Pokemon.Stat.Speed].ToString(), EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
            GUI.enabled = true;

            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Total", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            GUI.enabled = false;
            EditorGUILayout.SelectableLabel(target.Stats.Stats.Sum(stat => stat.Value).ToString(), EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
            GUI.enabled = true;

            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("EVs", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("HP", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(55));

            if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Stats.EVs[Pokemon.Stat.HP] = Mathf.Clamp(++target.Stats.EVs[Pokemon.Stat.HP], 0, 255);
            }

            target.Stats.EVs[Pokemon.Stat.HP] = EditorGUILayout.IntField(target.Stats.EVs[Pokemon.Stat.HP], GUILayout.Width(36));

            if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Stats.EVs[Pokemon.Stat.HP] = Mathf.Clamp(--target.Stats.EVs[Pokemon.Stat.HP], 0, 255);
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("ATK", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(55));

            if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Stats.EVs[Pokemon.Stat.Attack] = Mathf.Clamp(++target.Stats.EVs[Pokemon.Stat.Attack], 0, 255);
            }

            target.Stats.EVs[Pokemon.Stat.Attack] = EditorGUILayout.IntField(target.Stats.EVs[Pokemon.Stat.Attack], GUILayout.Width(36));

            if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Stats.EVs[Pokemon.Stat.Attack] = Mathf.Clamp(--target.Stats.EVs[Pokemon.Stat.Attack], 0, 255);
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("DEF", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(55));

            if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Stats.EVs[Pokemon.Stat.Defence] = Mathf.Clamp(++target.Stats.EVs[Pokemon.Stat.Defence], 0, 255);
            }

            target.Stats.EVs[Pokemon.Stat.Defence] = EditorGUILayout.IntField(target.Stats.EVs[Pokemon.Stat.Defence], GUILayout.Width(36));

            if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Stats.EVs[Pokemon.Stat.Defence] = Mathf.Clamp(--target.Stats.EVs[Pokemon.Stat.Defence], 0, 255);
            }

            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("SP. ATK", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(55));

            if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Stats.EVs[Pokemon.Stat.SpAttack] = Mathf.Clamp(++target.Stats.EVs[Pokemon.Stat.SpAttack], 0, 255);
            }

            target.Stats.EVs[Pokemon.Stat.SpAttack] = EditorGUILayout.IntField(target.Stats.EVs[Pokemon.Stat.SpAttack], GUILayout.Width(36));

            if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Stats.EVs[Pokemon.Stat.SpAttack] = Mathf.Clamp(--target.Stats.EVs[Pokemon.Stat.SpAttack], 0, 255);
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("SP. DEF", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(55));

            if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Stats.EVs[Pokemon.Stat.SpDefence] = Mathf.Clamp(++target.Stats.EVs[Pokemon.Stat.SpDefence], 0, 255);
            }

            target.Stats.EVs[Pokemon.Stat.SpDefence] = EditorGUILayout.IntField(target.Stats.EVs[Pokemon.Stat.SpDefence], GUILayout.Width(36));

            if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Stats.EVs[Pokemon.Stat.SpDefence] = Mathf.Clamp(--target.Stats.EVs[Pokemon.Stat.SpDefence], 0, 255);
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("SPD", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(55));

            if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Stats.EVs[Pokemon.Stat.Speed] = Mathf.Clamp(++target.Stats.EVs[Pokemon.Stat.Speed], 0, 255);
            }

            target.Stats.EVs[Pokemon.Stat.Speed] = EditorGUILayout.IntField(target.Stats.EVs[Pokemon.Stat.Speed], GUILayout.Width(36));

            if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Stats.EVs[Pokemon.Stat.Speed] = Mathf.Clamp(--target.Stats.EVs[Pokemon.Stat.Speed], 0, 255);
            }

            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Total", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            GUI.enabled = false;
            EditorGUILayout.SelectableLabel(target.Stats.EVs.Sum(stat => stat.Value).ToString(), EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
            GUI.enabled = true;

            if (GUILayout.Button("Reset", GUILayout.Width(100), GUILayout.Height(18)))
            {
                target.Stats.ResetEVs();
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("IVs", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("HP", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(55));

            if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Stats.IVs[Pokemon.Stat.HP] = Mathf.Clamp(++target.Stats.IVs[Pokemon.Stat.HP], 0, 31);
            }

            target.Stats.IVs[Pokemon.Stat.HP] = EditorGUILayout.IntField(target.Stats.IVs[Pokemon.Stat.HP], GUILayout.Width(36));

            if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Stats.IVs[Pokemon.Stat.HP] = Mathf.Clamp(--target.Stats.IVs[Pokemon.Stat.HP], 0, 31);
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("ATK", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(55));

            if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Stats.IVs[Pokemon.Stat.Attack] = Mathf.Clamp(++target.Stats.IVs[Pokemon.Stat.Attack], 0, 31);
            }

            target.Stats.IVs[Pokemon.Stat.Attack] = EditorGUILayout.IntField(target.Stats.IVs[Pokemon.Stat.Attack], GUILayout.Width(36));

            if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Stats.IVs[Pokemon.Stat.Attack] = Mathf.Clamp(--target.Stats.IVs[Pokemon.Stat.Attack], 0, 31);
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("DEF", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(55));

            if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Stats.IVs[Pokemon.Stat.Defence] = Mathf.Clamp(++target.Stats.IVs[Pokemon.Stat.Defence], 0, 31);
            }

            target.Stats.IVs[Pokemon.Stat.Defence] = EditorGUILayout.IntField(target.Stats.IVs[Pokemon.Stat.Defence], GUILayout.Width(36));

            if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Stats.IVs[Pokemon.Stat.Defence] = Mathf.Clamp(--target.Stats.IVs[Pokemon.Stat.Defence], 0, 31);
            }

            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("SP. ATK", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(55));

            if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Stats.IVs[Pokemon.Stat.SpAttack] = Mathf.Clamp(++target.Stats.IVs[Pokemon.Stat.SpAttack], 0, 31);
            }

            target.Stats.IVs[Pokemon.Stat.SpAttack] = EditorGUILayout.IntField(target.Stats.IVs[Pokemon.Stat.SpAttack], GUILayout.Width(36));

            if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Stats.IVs[Pokemon.Stat.SpAttack] = Mathf.Clamp(--target.Stats.IVs[Pokemon.Stat.SpAttack], 0, 31);
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("SP. DEF", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(55));

            if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Stats.IVs[Pokemon.Stat.SpDefence] = Mathf.Clamp(++target.Stats.IVs[Pokemon.Stat.SpDefence], 0, 31);
            }

            target.Stats.IVs[Pokemon.Stat.SpDefence] = EditorGUILayout.IntField(target.Stats.IVs[Pokemon.Stat.SpDefence], GUILayout.Width(36));

            if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Stats.IVs[Pokemon.Stat.SpDefence] = Mathf.Clamp(--target.Stats.IVs[Pokemon.Stat.SpDefence], 0, 31);
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("SPD", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(55));

            if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Stats.IVs[Pokemon.Stat.Speed] = Mathf.Clamp(++target.Stats.IVs[Pokemon.Stat.Speed], 0, 31);
            }

            target.Stats.IVs[Pokemon.Stat.Speed] = EditorGUILayout.IntField(target.Stats.IVs[Pokemon.Stat.Speed], GUILayout.Width(36));

            if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.Stats.IVs[Pokemon.Stat.Speed] = Mathf.Clamp(--target.Stats.IVs[Pokemon.Stat.Speed], 0, 31);
            }

            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Total", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            GUI.enabled = false;
            EditorGUILayout.SelectableLabel(target.Stats.IVs.Sum(stat => stat.Value).ToString(), EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
            GUI.enabled = true;

            if (GUILayout.Button("Random", GUILayout.Width(100), GUILayout.Height(18)))
            {
                target.Stats.AssignRandomIVs();
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Happiness", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));


            target.Stats.Happiness = EditorGUILayout.IntSlider(target.Stats.Happiness, 0, 255);

            if (GUILayout.Button("Reset", GUILayout.Width(100), GUILayout.Height(18)))
            {
                target.Stats.Happiness = target.Species.Stats.BaseHappiness;
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Nature", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Nature.Value = (PartyMember.MemberNature.Nature)EditorGUILayout.EnumPopup(target.Nature.Value);

            if (GUILayout.Button("Random", GUILayout.Width(100), GUILayout.Height(18)))
            {
                target.Nature.Value = target.Nature.AssignRandom();
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

        }

        GUILayout.Space(2);
        ExtensionMethods.DrawUILine("#525252".ToColor());
        GUILayout.Space(2);

        showMoves = EditorGUILayout.Foldout(showMoves, "Moves", foldoutStyle);
        GUILayout.Space(5);

        if (showMoves && target.Species != null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginVertical();
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();

            showActiveMoves = EditorGUILayout.Foldout(showActiveMoves, $"Active Moves ({target.ActiveMoves.Count})");

            EditorGUI.BeginDisabledGroup(!showActiveMoves || target.ActiveMoves.Count >= 4);

            if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.ActiveMoves.Add(new PartyMember.MemberMove());
            }

            EditorGUI.EndDisabledGroup();

            GUILayout.EndHorizontal();

            if (showActiveMoves)
            {
                GUILayout.BeginVertical();

                for (int i = 0; i < target.ActiveMoves.Count; i++)
                {
                    PartyMember.MemberMove move = target.ActiveMoves[i];

                    GUILayout.BeginVertical("Box");
                    GUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField($"{i + 1}.", GUILayout.Width(45));
                    move.Value = (Move)EditorGUILayout.ObjectField(move.Value, typeof(Move), false);

                    EditorGUI.BeginDisabledGroup(i == 0);

                    if (GUILayout.Button("↑", GUILayout.Width(18), GUILayout.Height(18)))
                    {
                        target.ActiveMoves.RemoveAt(i);
                        target.ActiveMoves.Insert(i - 1, move);
                    }

                    EditorGUI.EndDisabledGroup();
                    EditorGUI.BeginDisabledGroup(i == target.ActiveMoves.Count - 1);

                    if (GUILayout.Button("↓", GUILayout.Width(18), GUILayout.Height(18)))
                    {
                        target.ActiveMoves.RemoveAt(i);
                        target.ActiveMoves.Insert(i + 1, move);
                    }

                    EditorGUI.EndDisabledGroup();

                    if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
                    {
                        target.ActiveMoves.RemoveAt(i);
                    }

                    GUILayout.EndHorizontal();

                    if (move.Value != null)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.BeginHorizontal();

                        EditorGUILayout.LabelField(new GUIContent("PP", "Name of this Pokémon.\n\n" +
                        "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(45));              
                        move.PP = EditorGUILayout.IntField(move.PP);
                        EditorGUILayout.LabelField("/", GUILayout.Width(10));
                        GUI.enabled = false;
                        EditorGUILayout.SelectableLabel(move.Value.pp.ToString(), EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
                        GUI.enabled = true;

                        GUILayout.BeginHorizontal();

                        EditorGUILayout.LabelField(new GUIContent("ACC.", "Name of this Pokémon.\n\n" +
                        "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(45));
                        GUI.enabled = false;
                        EditorGUILayout.SelectableLabel(move.Value.accuracy.ToString(), EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
                        GUI.enabled = true;

                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();

                        EditorGUILayout.LabelField(new GUIContent("POW.", "Name of this Pokémon.\n\n" +
                        "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(45));
                        GUI.enabled = false;
                        EditorGUILayout.SelectableLabel(move.Value.power.ToString(), EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
                        GUI.enabled = true;

                        GUILayout.EndHorizontal();
                        GUILayout.EndHorizontal();
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();

                        EditorGUILayout.LabelField(new GUIContent("Desc.", "Name of this Pokémon.\n\n" +
                        "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(45));
                        GUI.enabled = false;
                        EditorGUILayout.SelectableLabel(move.Value.description, EditorStyles.textArea);
                        GUI.enabled = true;

                        GUILayout.EndHorizontal();
                    }

                    GUILayout.EndVertical();
                }

                GUILayout.EndVertical();
            }

            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();

            showLearnedMoves = EditorGUILayout.Foldout(showLearnedMoves, $"Learned Moves ({target.LearnedMoves.Count})");

            EditorGUI.BeginDisabledGroup(!showLearnedMoves || target.LearnedMoves.Count >= 4);

            if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
            {
                target.LearnedMoves.Add(new PartyMember.MemberMove());
            }

            EditorGUI.EndDisabledGroup();

            GUILayout.EndHorizontal();

            if (showLearnedMoves)
            {
                GUILayout.BeginVertical();

                for (int i = 0; i < target.LearnedMoves.Count; i++)
                {
                    PartyMember.MemberMove move = target.LearnedMoves[i];

                    GUILayout.BeginVertical("Box");
                    GUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField($"{i + 1}.", GUILayout.Width(45));
                    move.Value = (Move)EditorGUILayout.ObjectField(move.Value, typeof(Move), false);

                    EditorGUI.BeginDisabledGroup(i == 0);

                    if (GUILayout.Button("↑", GUILayout.Width(18), GUILayout.Height(18)))
                    {
                        target.LearnedMoves.RemoveAt(i);
                        target.LearnedMoves.Insert(i - 1, move);
                    }

                    EditorGUI.EndDisabledGroup();
                    EditorGUI.BeginDisabledGroup(i == target.ActiveMoves.Count - 1);

                    if (GUILayout.Button("↓", GUILayout.Width(18), GUILayout.Height(18)))
                    {
                        target.LearnedMoves.RemoveAt(i);
                        target.LearnedMoves.Insert(i + 1, move);
                    }

                    EditorGUI.EndDisabledGroup();

                    if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
                    {
                        target.LearnedMoves.RemoveAt(i);
                    }

                    GUILayout.EndHorizontal();

                    if (move.Value != null)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.BeginHorizontal();

                        EditorGUILayout.LabelField(new GUIContent("PP", "Name of this Pokémon.\n\n" +
                        "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(45));
                        move.PP = EditorGUILayout.IntField(move.PP);
                        EditorGUILayout.LabelField("/", GUILayout.Width(10));
                        GUI.enabled = false;
                        EditorGUILayout.SelectableLabel(move.Value.pp.ToString(), EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
                        GUI.enabled = true;

                        GUILayout.BeginHorizontal();

                        EditorGUILayout.LabelField(new GUIContent("ACC.", "Name of this Pokémon.\n\n" +
                        "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(45));
                        GUI.enabled = false;
                        EditorGUILayout.SelectableLabel(move.Value.accuracy.ToString(), EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
                        GUI.enabled = true;

                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();

                        EditorGUILayout.LabelField(new GUIContent("POW.", "Name of this Pokémon.\n\n" +
                        "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(45));
                        GUI.enabled = false;
                        EditorGUILayout.SelectableLabel(move.Value.power.ToString(), EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
                        GUI.enabled = true;

                        GUILayout.EndHorizontal();
                        GUILayout.EndHorizontal();
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();

                        EditorGUILayout.LabelField(new GUIContent("Desc.", "Name of this Pokémon.\n\n" +
                        "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(45));
                        GUI.enabled = false;
                        EditorGUILayout.SelectableLabel(move.Value.description, EditorStyles.textArea);
                        GUI.enabled = true;

                        GUILayout.EndHorizontal();
                    }

                    GUILayout.EndVertical();
                }

                GUILayout.EndVertical();
            }

            GUILayout.EndVertical();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(2);
        ExtensionMethods.DrawUILine("#525252".ToColor());
        GUILayout.Space(2);

        EditorUtility.SetDirty(target);

        //base.OnInspectorGUI();
    }

    /*
showProgression = EditorGUILayout.Foldout(showProgression, "Progression", foldoutStyle);

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
