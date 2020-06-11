using System;
using System.Linq;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Mission)), CanEditMultipleObjects]
public class MissionEditor : Editor
{
    #region Variables

    private new Mission target;

    private static bool showBasicInfo = true;
    private static bool showLocation = false;
    private static bool showReward = true;

    #endregion

    private void OnEnable()
    {
        target = (Mission)base.target;
    }

    public override void OnInspectorGUI()
    {
        GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout)
        {
            fontStyle = FontStyle.Bold,
            fontSize = 16
        };

        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical("Box", GUILayout.Height(35));
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        GUILayout.Label($"{target.Categorization.ToString()} - {target.Name} - {(target.IsCompleted ? "Completed" : (target.IsFailed ? "Failed" : ("% completed")))}");

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        showBasicInfo = EditorGUILayout.Foldout(showBasicInfo, "Basic Information", foldoutStyle);
        GUILayout.Space(5);

        if (showBasicInfo)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(new GUIContent("ID", "Name of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.ID = EditorGUILayout.IntField(target.ID);

            if (GUILayout.Button("Auto", GUILayout.Width(100), GUILayout.Height(18)))
            {
                
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(new GUIContent("Name", "Name of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Name = EditorGUILayout.TextField(target.Name);

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(new GUIContent("Category", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            ((Mission.MissionCategory)target.Categorization).EnumValue = (Mission.MissionCategory.Category)EditorGUILayout.EnumPopup(((Mission.MissionCategory)target.Categorization).EnumValue);

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(new GUIContent("Description", "Category of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Description = EditorGUILayout.TextArea(target.Description, GUILayout.MaxHeight(35));
            EditorStyles.textField.wordWrap = true;

            GUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(2);
        ExtensionMethods.DrawUILine("#525252".ToColor());
        GUILayout.Space(2);

        showLocation = EditorGUILayout.Foldout(showLocation, "Location", foldoutStyle);
        GUILayout.Space(5);

        if (showLocation)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(new GUIContent("Status Ailment", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            //target.Ailment.Value = (PartyMember.StatusAilment.Ailment)EditorGUILayout.EnumPopup(target.Ailment.Value);

            GUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(2);
        ExtensionMethods.DrawUILine("#525252".ToColor());
        GUILayout.Space(2);

        GUILayout.BeginHorizontal();

        showReward = EditorGUILayout.Foldout(showReward, $"Rewards ({target.Rewards.Count})", foldoutStyle);

        EditorGUI.BeginDisabledGroup(!showReward || target.Rewards.Count >= 3);

        if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
        {
            target.Rewards.Add(new Mission.MissionReward());
        }

        EditorGUI.EndDisabledGroup();
        GUILayout.EndHorizontal();

        if (showReward)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginVertical();
            GUILayout.BeginVertical();
            GUILayout.BeginVertical();

            for (int i = 0; i < target.Rewards.Count; i++)
            {
                Mission.MissionReward reward = target.Rewards[i];

                GUILayout.BeginVertical("Box");
                GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField($"{i + 1}.", GUILayout.Width(45));
                EditorGUILayout.LabelField(new GUIContent("Type", "Dex number of this Pokémon.\n\n" +
                "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
                reward.Type = (Mission.MissionReward.MissionType)EditorGUILayout.EnumPopup(reward.Type);

                EditorGUI.BeginDisabledGroup(i == 0);

                if (GUILayout.Button("↑", GUILayout.Width(18), GUILayout.Height(18)))
                {
                    target.Rewards.RemoveAt(i);
                    target.Rewards.Insert(i - 1, reward);
                }

                EditorGUI.EndDisabledGroup();
                EditorGUI.BeginDisabledGroup(i == target.Rewards.Count - 1);

                if (GUILayout.Button("↓", GUILayout.Width(18), GUILayout.Height(18)))
                {
                    target.Rewards.RemoveAt(i);
                    target.Rewards.Insert(i + 1, reward);
                }

                EditorGUI.EndDisabledGroup();

                if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
                {
                    target.Rewards.RemoveAt(i);
                }

                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
            }

            GUILayout.EndVertical();

            GUILayout.EndVertical();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(2);
        ExtensionMethods.DrawUILine("#525252".ToColor());
        GUILayout.Space(2);

        EditorUtility.SetDirty(target);

        base.OnInspectorGUI();
    }
}
