using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class DialogEditorWindow : EditorWindow
{
    private static Dialog dialog;

    private Character character;
    private string sentence;
    private BranchingDialog branchingDialog;

    private bool hasBranchingDialog;

    public static void ShowWindow(Dialog _dialog)
    {
        DialogEditorWindow window = (DialogEditorWindow)EditorWindow.GetWindow(typeof(DialogEditorWindow), true, "New sentence");
        window.maxSize = new Vector2(400, 230); // Width and height respectively of Editor Window.
        window.minSize = window.maxSize;

        dialog = _dialog;
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginVertical();

        GUILayout.Space(10);

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Character");
        character = (Character)EditorGUILayout.ObjectField(character, typeof(Character) , false);
        EditorGUILayout.EndVertical();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Sentence");
        sentence = EditorGUILayout.TextArea(sentence, GUILayout.MaxHeight(50));
        EditorStyles.textField.wordWrap = true;
        EditorGUILayout.EndVertical();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.BeginVertical();
        hasBranchingDialog = EditorGUILayout.Toggle("Branching dialog", hasBranchingDialog);
        if (hasBranchingDialog)
            branchingDialog = (BranchingDialog)EditorGUILayout.ObjectField(branchingDialog, typeof(BranchingDialog), false);
        EditorGUILayout.EndVertical();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add"))
        {
            if (string.IsNullOrEmpty(sentence) && EditorUtility.DisplayDialog("Leave sentence empty?",
            "Are you sure you want to leave the dialog sentence empty?",
            "Yes", "No"))
            {
                AddSentence();
                this.Close();
            }
            else
            {
                AddSentence();
                this.Close();
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndVertical();
    }

    public void AddSentence()
    {
        Dialog.DialogData newSentence = new Dialog.DialogData
        {
            character = character,
            sentence = sentence,
            branchingDialog = branchingDialog
        };
        dialog.dialogData.Add(newSentence);

        EditorUtility.SetDirty(dialog);
    }
}
