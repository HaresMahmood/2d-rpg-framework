using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BranchingDialog)), CanEditMultipleObjects]
public class BranchingDialogEditor : Editor
{
    #region Variables

    private new BranchingDialog target;

    private static bool showBranches = true;
    private static bool showList = true;

    private int tab = 0;

    #endregion

    private void OnEnable()
    {
        target = (BranchingDialog)base.target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout)
        {
            fontStyle = FontStyle.Bold,
            fontSize = 16
        };

        GUILayout.Space(10);

        GUILayout.BeginHorizontal("Box", GUILayout.Height(35));
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();

        GUILayout.Label($"Contains {target.Branches.Count} Branches.");

        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndHorizontal();


        GUILayout.Space(2);
        ExtensionMethods.DrawUILine("#525252".ToColor());
        GUILayout.Space(2);

        GUILayout.BeginHorizontal();

        showBranches = EditorGUILayout.Foldout(showBranches, "Branches", foldoutStyle);

        EditorGUI.BeginDisabledGroup(!showBranches && target.Branches.Count >= 5);

        if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
        {
            target.Branches.Add(new BranchingDialog.DialogBranch());
        }

        EditorGUI.EndDisabledGroup();
        GUILayout.EndHorizontal();

        if (showBranches)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginVertical();

            for (int i = 0; i < target.Branches.Count; i++)
            {
                //Debug.Log($"i: {i}, j: {j}, c: {counter}");

                BranchingDialog.DialogBranch branch = target.Branches[i];

                GUILayout.BeginVertical("Box");
                GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField($"{i + 1}.", GUILayout.Width(45));

                EditorGUILayout.LabelField(new GUIContent("Dialog", "Dex number of this Pokémon.\n\n" +
                "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(45));
                branch.NextDialog = (Dialog)EditorGUILayout.ObjectField(branch.NextDialog, typeof(Dialog), false);

                EditorGUI.BeginDisabledGroup(i == 0);

                if (GUILayout.Button("↑", GUILayout.Width(18), GUILayout.Height(18)))
                {
                    target.Branches.RemoveAt(i);
                    target.Branches.Insert(i - 1, branch);
                }

                EditorGUI.EndDisabledGroup();
                EditorGUI.BeginDisabledGroup(i == target.Branches.Count - 1);

                if (GUILayout.Button("↓", GUILayout.Width(18), GUILayout.Height(18)))
                {
                    target.Branches.RemoveAt(i);
                    target.Branches.Insert(i + 1, branch);
                }

                EditorGUI.EndDisabledGroup();

                if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
                {
                    target.Branches.RemoveAt(i);
                }

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(new GUIContent("Text", "Name of this Pokémon.\n\n" +
                "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(45));
                branch.Text = EditorGUILayout.TextField(branch.Text);

                GUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(new GUIContent("Button", "Name of this Pokémon.\n\n" +
                "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(45));
                GUI.enabled = false;
                EditorGUILayout.SelectableLabel(i == (target.Branches.Count - 1) ? "Back" : (i + 1).ToString(), EditorStyles.textField, GUILayout.Width(38), GUILayout.Height(EditorGUIUtility.singleLineHeight));
                GUI.enabled = true;

                GUILayout.EndHorizontal();
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(new GUIContent("Event", "Name of this Pokémon.\n\n" +
                "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(45));
                
                SerializedProperty branchProperty = serializedObject.FindProperty("branches").GetArrayElementAtIndex(i).FindPropertyRelative("branchEvent");
                EditorGUILayout.PropertyField(branchProperty, new GUIContent("Custom event"));

                serializedObject.ApplyModifiedProperties();

                GUILayout.EndHorizontal();
                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        //base.OnInspectorGUI();
    }

    /*
    #region Variables
    private BranchingDialog branchingDialog;

    private bool hasNextDialog = false;
    #endregion

    private void OnEnable()
    {
        branchingDialog = (BranchingDialog)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.BeginVertical();

        GUILayout.Space(5);

        EditorGUILayout.BeginVertical("Box");
        GUILayout.Label("Total branches: " + branchingDialog.dialogBranches.Count);
        EditorGUILayout.EndVertical();

        ExtensionMethods.DrawUILine("#969696".ToColor(), 3);

        EditorGUILayout.EndVertical();

        if (branchingDialog.dialogBranches != null)
        {
            foreach (BranchingDialog.DialogBranch dialogBranch in branchingDialog.dialogBranches)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.BeginVertical("Box");
                GUILayout.Label("Branch " + (this.branchingDialog.dialogBranches.IndexOf(dialogBranch) + 1) + ":");
                EditorGUILayout.EndVertical();

                if (GUILayout.Button("Remove", GUILayout.Width(70), GUILayout.Height(25)))
                {
                    this.branchingDialog.dialogBranches.Remove(dialogBranch);
                    EditorUtility.SetDirty(target);
                    return;
                }
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(5);

                EditorGUILayout.BeginVertical();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(new GUIContent("Option text", "Text displayed during inventory, representing " +
                    "this specific branch."));
                dialogBranch.branchOption = EditorGUILayout.TextField(dialogBranch.branchOption, GUILayout.MaxWidth(230));
                EditorUtility.SetDirty(branchingDialog);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(new GUIContent("Jump to inventory", "Whether or not this branch splits off to another inventory."));
                dialogBranch.nextDialog = (Dialog)EditorGUILayout.ObjectField(dialogBranch.nextDialog, typeof(Dialog), false, GUILayout.MaxWidth(230));
                EditorUtility.SetDirty(branchingDialog);
                EditorGUILayout.EndHorizontal();

                if (dialogBranch == branchingDialog.dialogBranches.Last())
                {
                    EditorGUILayout.BeginVertical();
                    dialogBranch.hasBackButton = EditorGUILayout.Toggle(new GUIContent("Back button", "Whether or not this option " +
                    "includes a 'Back'-button. Can only be assigned to the last element in the list."), dialogBranch.hasBackButton); //TODO: Edit tooltip!
                    EditorUtility.SetDirty(branchingDialog);
                    EditorGUILayout.EndVertical();
                }

                GUILayout.Space(5);

                EditorGUILayout.BeginVertical();
                SerializedProperty branchList = serializedObject.FindProperty("dialogBranches");
                SerializedProperty branchProperty = branchList.GetArrayElementAtIndex(branchingDialog.dialogBranches.IndexOf(dialogBranch)).FindPropertyRelative("branchEvent");
                //Debug.Log(branchProperty.name); // Debug
                EditorGUILayout.PropertyField(branchProperty, new GUIContent("Custom event"));
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(branchingDialog);
                EditorGUILayout.EndVertical();

                GUILayout.Space(10f);
                ExtensionMethods.DrawUILine("#969696".ToColor());
                GUILayout.Space(2);

                EditorGUILayout.EndVertical();
            }
        }

        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("Add branch", "Adds an entry to the current branching inventory.")))
            AddBranchingDialog();

        EditorGUI.BeginDisabledGroup(branchingDialog.dialogBranches.Count == 0);
        if (GUILayout.Button(new GUIContent("Clear all branches", "Clears all branch entries from current branching inventory.")))
        {
            this.branchingDialog.dialogBranches.Clear();
            EditorUtility.SetDirty(target);
        }
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }

    public void AddBranchingDialog()
    {
        BranchingDialog.DialogBranch newDialogBranch = new BranchingDialog.DialogBranch();
        branchingDialog.dialogBranches.Add(newDialogBranch);
        EditorUtility.SetDirty(branchingDialog);
    }
    */
}
