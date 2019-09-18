using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

[CustomEditor(typeof(Dialog)), CanEditMultipleObjects]
public class DialogEditor : Editor
{
    public Character character;
    public string sentence;
    public BranchingDialog branchingDialog;

    public Dialog dialog;

    bool hasBranchingDialog;
    //[SerializeField] public static bool expandProperties = true;

    private void OnEnable()
    {
        dialog = (Dialog)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();
        GUILayout.Space(5);

        EditorGUILayout.BeginVertical("Box");
        GUILayout.Label("Total sentences: " + dialog.dialogData.Count);
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndVertical();
        GUILayout.Space(10);

        foreach (Dialog.DialogData dialog in dialog.dialogData)
        {
            //expandProperties = EditorGUILayout.Foldout(expandProperties, "Sentence " + (this.dialog.dialogData.IndexOf(dialog) + 1));
            //if (expandProperties)
            //{
            //EditorGUI.indentLevel++;
            EditorGUILayout.BeginVertical("Box");
            GUILayout.Label("Sentence " + (this.dialog.dialogData.IndexOf(dialog) + 1) + ":");
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
                GUILayout.Space(5);

                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("Character");
                dialog.character = (Character)EditorGUILayout.ObjectField(dialog.character, typeof(Character), false);
                EditorGUILayout.EndVertical();

                GUILayout.Space(5);
                GUILine();
                GUILayout.Space(5);

                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("Sentence");
                dialog.sentence = EditorGUILayout.TextArea(dialog.sentence, GUILayout.MaxHeight(50));
            EditorStyles.textField.wordWrap = true;
            EditorGUILayout.EndVertical();

                GUILayout.Space(5);
                GUILine();
                GUILayout.Space(5);

                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("Branching dialog");
                dialog.branchingDialog = (BranchingDialog)EditorGUILayout.ObjectField(dialog.branchingDialog, typeof(BranchingDialog), false);
                EditorGUILayout.EndVertical();

                GUILayout.Space(5);
                GUILine();
                GUILayout.Space(5);
                EditorGUILayout.EndVertical();
            //}
        }

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add sentence"))
            DialogEditorWindow.ShowWindow(this.dialog);

        if (GUILayout.Button("Clear all sentences"))
            this.dialog.dialogData.Clear();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        //base.OnInspectorGUI();
    }

    void GUILine(int height = 1) // TODO: Move to ExtensionMethods
    {
        Rect rect = EditorGUILayout.GetControlRect(false, height);
        rect.height = height;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
    }
}