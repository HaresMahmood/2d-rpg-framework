using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[CustomEditor(typeof(BranchingDialog)), CanEditMultipleObjects]
public class BranchingDialogEditor : Editor
{
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
                EditorGUILayout.LabelField("Option text");
                dialogBranch.branchOption = EditorGUILayout.TextField(dialogBranch.branchOption, GUILayout.MaxWidth(230));
                EditorUtility.SetDirty(branchingDialog);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Jump to dialog");
                dialogBranch.nextDialog = (Dialog)EditorGUILayout.ObjectField(dialogBranch.nextDialog, typeof(Dialog), false, GUILayout.MaxWidth(230));
                EditorUtility.SetDirty(branchingDialog);
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(5);
                ExtensionMethods.DrawUILine("#969696".ToColor());
                GUILayout.Space(5);

                EditorGUILayout.BeginVertical();
                SerializedProperty branchList = serializedObject.FindProperty("dialogBranches");
                SerializedProperty branchProperty = branchList.GetArrayElementAtIndex(branchingDialog.dialogBranches.IndexOf(dialogBranch)).FindPropertyRelative("branchEvent");
                //Debug.Log(branchProperty.name); // Debug
                EditorGUILayout.PropertyField(branchProperty);
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(branchingDialog);
                EditorGUILayout.EndVertical();

                GUILayout.Space(5);
                ExtensionMethods.DrawUILine("#969696".ToColor());
                GUILayout.Space(5);

                EditorGUILayout.EndVertical();
            }
        }

        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add branch"))
            AddBranchingDialog();

        if (GUILayout.Button("Clear all branches"))
        {
            this.branchingDialog.dialogBranches.Clear();
            EditorUtility.SetDirty(target);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }

    public void AddBranchingDialog()
    {
        BranchingDialog.DialogBranch newDialogBranch = new BranchingDialog.DialogBranch();
        branchingDialog.dialogBranches.Add(newDialogBranch);
        EditorUtility.SetDirty(branchingDialog);
    }
}
