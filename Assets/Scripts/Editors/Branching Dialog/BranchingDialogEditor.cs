using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

[CustomEditor(typeof(BranchingDialog)), CanEditMultipleObjects]
public class BranchingDialogEditor : Editor
{
    private BranchingDialog branchingDialog;

    private bool hasNextDialog = false;

    private void OnEnable()
    {
        branchingDialog = (BranchingDialog)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();

        GUILayout.Space(5);

        EditorGUILayout.BeginVertical("Box");
        GUILayout.Label("Total sentences: " + branchingDialog.dialogBranches.Count);
        EditorGUILayout.EndVertical();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.EndVertical();

        if (branchingDialog.dialogBranches != null)
        {
            foreach (BranchingDialog.DialogBranch dialogBranch in branchingDialog.dialogBranches)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.BeginVertical("Box");
                GUILayout.Label("Sentence " + (this.branchingDialog.dialogBranches.IndexOf(dialogBranch) + 1) + ":");
                EditorGUILayout.EndVertical();

                if (GUILayout.Button("Remove", GUILayout.Width(70), GUILayout.Height(25)))
                {
                    this.branchingDialog.dialogBranches.Remove(dialogBranch);
                    EditorUtility.SetDirty(target);
                    return;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("Branch option");
                dialogBranch.branchOption = EditorGUILayout.TextField(dialogBranch.branchOption);
                EditorGUILayout.EndVertical();

                GUILayout.Space(5);
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                GUILayout.Space(5);

                /*
                EditorGUILayout.BeginVertical();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("branchEvent"));
                EditorGUILayout.EndVertical();
                */

                GUILayout.Space(5);
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                GUILayout.Space(5);

                EditorGUILayout.BeginHorizontal();
                hasNextDialog = EditorGUILayout.Toggle("Jump to dialog", hasNextDialog);
                EditorGUILayout.EndHorizontal();

                if (hasNextDialog)
                {
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.LabelField("Next dialog");
                    dialogBranch.nextDialog = (Dialog)EditorGUILayout.ObjectField(dialogBranch.nextDialog, typeof(Dialog), false);
                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.EndVertical();

                if (branchingDialog.dialogBranches != null)
                    branchingDialog.dialogBranches.Clear();

                GUILayout.Space(5);
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                GUILayout.Space(5);
                EditorGUILayout.EndVertical();

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
            }
        }

        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add sentence"))
        {
            AddBranchingDialog();
        }

        if (GUILayout.Button("Clear all sentences"))
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
