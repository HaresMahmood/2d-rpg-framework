using System.Linq;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Mission)), CanEditMultipleObjects]
public class MissionEditor : Editor
{
    #region Variables

    private new Mission target;

    private static bool showBasicInfo = true;
    private static bool showOriginAndDestination = false;
    private static bool showGoals = false;
    private static bool showRewards = false;

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

        GUILayout.Label($"{target.Categorization.ToString()} - {target.Name} - {(target.IsFailed ? "Failed" : (target.Goals.Count == 0 ? "Add a goal via the \"Goals\" tab..." : $"{target.CompletionPercentage}% completed"))}");

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

        showOriginAndDestination = EditorGUILayout.Foldout(showOriginAndDestination, "Origin and Destination", foldoutStyle);
        GUILayout.Space(5);

        if (showOriginAndDestination)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(new GUIContent("Destination", "Character assigning this mission."), GUILayout.Width(95));
            target.OriginDestination.Destination = EditorGUILayout.TextField(target.OriginDestination.Destination);

            GUILayout.EndHorizontal();
            GUILayout.BeginVertical("Box");

            EditorGUILayout.LabelField("Auto-fields:");

            GUI.enabled = false;
            GUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(new GUIContent("Origin", "Character assigning this mission."), GUILayout.Width(95));
            target.OriginDestination.Origin = EditorGUILayout.TextField(target.OriginDestination.Origin);

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(new GUIContent("Assignee", "Character assigning this mission.\n\n" +
            "This field is assigned automatically."), GUILayout.Width(95));
            target.OriginDestination.Assignee = (Character)EditorGUILayout.ObjectField(target.OriginDestination.Assignee, typeof(Character), false);

            GUILayout.EndHorizontal();
            GUI.enabled = true;
            GUILayout.EndVertical();
            EditorGUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(2);
        ExtensionMethods.DrawUILine("#525252".ToColor());
        GUILayout.Space(2);

        GUILayout.BeginHorizontal();

        showGoals = EditorGUILayout.Foldout(showGoals, $"Goals ({target.Goals.Count})", foldoutStyle);

        EditorGUI.BeginDisabledGroup(!showGoals || target.Goals.Count >= 10);

        if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
        {
            target.Goals.Add(new Mission.MissionGoal());
        }

        EditorGUI.EndDisabledGroup();
        GUILayout.EndHorizontal();

        if (showGoals)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginVertical();
            GUILayout.BeginVertical();
            GUILayout.BeginVertical();

            for (int i = 0; i < target.Goals.Count; i++)
            {
                Mission.MissionGoal goal = target.Goals[i];

                GUILayout.BeginVertical("Box");
                GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField($"{i + 1}.", GUILayout.Width(45));

                EditorGUI.BeginDisabledGroup(goal.IsCompleted || goal.IsFailed || (target.Goals.Where(g => target.Goals.IndexOf(g) < target.Goals.IndexOf(goal) && g.IsFailed == true).Count() > 0));

                EditorGUILayout.LabelField(new GUIContent("Type", "Dex number of this Pokémon.\n\n" +
                "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
                goal.Type = (Mission.MissionGoal.GoalType)EditorGUILayout.EnumPopup(goal.Type);

                EditorGUI.EndDisabledGroup();

                EditorGUI.BeginDisabledGroup(i == 0);

                if (GUILayout.Button("↑", GUILayout.Width(18), GUILayout.Height(18)))
                {
                    target.Goals.RemoveAt(i);
                    target.Goals.Insert(i - 1, goal);
                }

                EditorGUI.EndDisabledGroup();
                EditorGUI.BeginDisabledGroup(i == target.Rewards.Count - 1);

                if (GUILayout.Button("↓", GUILayout.Width(18), GUILayout.Height(18)))
                {
                    target.Goals.RemoveAt(i);
                    target.Goals.Insert(i + 1, goal);
                }

                EditorGUI.EndDisabledGroup();

                if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
                {
                    target.Goals.RemoveAt(i);
                }

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Space(48);

                if (goal.Type == Mission.MissionGoal.GoalType.Talk || goal.Type == Mission.MissionGoal.GoalType.Deliver || goal.Type == Mission.MissionGoal.GoalType.Escort)
                {
                    EditorGUI.BeginDisabledGroup(goal.Character == null ? false : (goal.IsCompleted || goal.IsFailed || (target.Goals.Where(g => target.Goals.IndexOf(g) < target.Goals.IndexOf(goal) && g.IsFailed == true).Count() > 0)));

                    EditorGUILayout.LabelField(new GUIContent("Character", "Dex number of this Pokémon.\n\n" +
                    "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
                    goal.Character = (Character)EditorGUILayout.ObjectField(goal.Character, typeof(Character), false);

                    EditorGUI.EndDisabledGroup();
                }
                else if (goal.Type == Mission.MissionGoal.GoalType.Battle)
                {
                    EditorGUI.BeginDisabledGroup(goal.Pokemon == null ? false : (goal.IsCompleted || goal.IsFailed || (target.Goals.Where(g => target.Goals.IndexOf(g) < target.Goals.IndexOf(goal) && g.IsFailed == true).Count() > 0)));

                    EditorGUILayout.LabelField(new GUIContent("Pokémon", "Dex number of this Pokémon.\n\n" +
                    "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
                    goal.Pokemon = (Pokemon)EditorGUILayout.ObjectField(goal.Pokemon, typeof(Pokemon), false);

                    if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
                    {
                        goal.Amount = Mathf.Clamp(++goal.Amount, 0, 5);
                    }

                    goal.Amount = EditorGUILayout.IntField(goal.Amount, GUILayout.Width(18));

                    if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
                    {
                        goal.Amount = Mathf.Clamp(--goal.Amount, 0, 5);
                    }

                    EditorGUI.EndDisabledGroup();
                }
                else if (goal.Type == Mission.MissionGoal.GoalType.Gather)
                {
                    EditorGUI.BeginDisabledGroup(goal.Item == null ? false : (goal.IsCompleted || goal.IsFailed || (target.Goals.Where(g => target.Goals.IndexOf(g) < target.Goals.IndexOf(goal) && g.IsFailed == true).Count() > 0)));

                    EditorGUILayout.LabelField(new GUIContent("Item", "Dex number of this Pokémon.\n\n" +
                    "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
                    goal.Item = (Item)EditorGUILayout.ObjectField(goal.Item, typeof(Item), false);

                    if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
                    {
                        goal.Amount = Mathf.Clamp(++goal.Amount, 0, 5);
                    }

                    goal.Amount = EditorGUILayout.IntField(goal.Amount, GUILayout.Width(18));

                    if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
                    {
                        goal.Amount = Mathf.Clamp(--goal.Amount, 0, 5);
                    }

                    EditorGUI.EndDisabledGroup();
                }

                GUILayout.Space(10);

                EditorGUI.BeginDisabledGroup(goal.IsFailed || (target.Goals.Where(g => target.Goals.IndexOf(g) < target.Goals.IndexOf(goal) && g.IsFailed == true).Count() > 0));

                if (target.Goals.Where(g => target.Goals.IndexOf(g) < target.Goals.IndexOf(goal) && g.IsFailed == true).Count() > 0)
                {
                    goal.IsCompleted = false;
                }

                EditorGUILayout.LabelField(new GUIContent("✓", "Checked if goal is failed."), GUILayout.Width(15));
                goal.IsCompleted = EditorGUILayout.Toggle(goal.IsCompleted, GUILayout.Width(15));

                EditorGUI.EndDisabledGroup();
                EditorGUI.BeginDisabledGroup(goal.IsCompleted || (target.Goals.Where(g => target.Goals.IndexOf(g) < target.Goals.IndexOf(goal) && g.IsFailed == true).Count() > 0));

                if (target.Goals.Where(g => target.Goals.IndexOf(g) < target.Goals.IndexOf(goal) && g.IsFailed == true).Count() > 0)
                {
                    goal.IsFailed = true;
                }

                EditorGUILayout.LabelField(new GUIContent("x", "Checked if goal is completed."), GUILayout.Width(15));
                goal.IsFailed = EditorGUILayout.Toggle(goal.IsFailed, GUILayout.Width(15));

                EditorGUI.EndDisabledGroup();
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

        GUILayout.BeginHorizontal();

        showRewards = EditorGUILayout.Foldout(showRewards, $"Rewards ({target.Rewards.Count})", foldoutStyle);

        EditorGUI.BeginDisabledGroup(!showRewards || target.Rewards.Count >= 3);

        if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
        {
            target.Rewards.Add(new Mission.MissionReward());
        }

        EditorGUI.EndDisabledGroup();
        GUILayout.EndHorizontal();

        if (showRewards)
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
                reward.Type = (Mission.MissionReward.RewardType)EditorGUILayout.EnumPopup(reward.Type);

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
                GUILayout.BeginHorizontal();
                GUILayout.Space(48);

                if (reward.Type == Mission.MissionReward.RewardType.Item)
                {
                    EditorGUILayout.LabelField(new GUIContent("Item", "Dex number of this Pokémon.\n\n" +
                    "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
                    reward.Item = (Item)EditorGUILayout.ObjectField(reward.Item, typeof(Item), false);
                }

                EditorGUILayout.LabelField(new GUIContent("Amount", "Dex number of this Pokémon.\n\n" +
                "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
                reward.Amount = EditorGUILayout.IntSlider(reward.Amount, 1, (reward.Type == Mission.MissionReward.RewardType.Experience ? 2000 : (reward.Type == Mission.MissionReward.RewardType.Item ? 10 : 50000)));

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

        //base.OnInspectorGUI();
    }
}
