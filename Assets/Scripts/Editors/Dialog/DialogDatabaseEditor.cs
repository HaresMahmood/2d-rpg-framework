using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Dialog))]
public class DialogEditor : Editor
{
    public Character character;
    public string sentence;
    public BranchingDialog choices;

    Dialog dialog;

    private void OnEnable()
    {
        dialog = (Dialog)target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Space(5);

        EditorGUILayout.BeginHorizontal("Box");
        GUILayout.Label("Total sentences: " + dialog.dialog.Count);
        EditorGUILayout.EndHorizontal();

        foreach (Dialog.DialogInfo dialog in dialog.dialog)
        {
            GUILayout.Space(10);

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
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Choices");
            dialog.choices = (BranchingDialog)EditorGUILayout.ObjectField(dialog.choices, typeof(BranchingDialog), false);
            EditorGUILayout.EndVertical();
        }

        GUILayout.Space(10);
        GUILine();
        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add sentence"))
            DialogEditorWindow.ShowWindow(dialog);
        if (GUILayout.Button("Clear all sentences"))
            dialog.dialog.Clear();
        EditorGUILayout.EndHorizontal();

        //base.OnInspectorGUI();
    }

    void GUILine(int height = 1) // TODO: Move to ExtensionMethods
    {
        Rect rect = EditorGUILayout.GetControlRect(false, height);
        rect.height = height;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
    }
}