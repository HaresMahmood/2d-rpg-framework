using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class DialogEditorWindow : EditorWindow
{
    public Character character;
    public string sentence;
    public BranchingDialog branchingDialog;

    public static Dialog dialog;

    public bool hasBranchingDialog;

    //Vector2 scrollPos;

    public static void ShowWindow(Dialog _dialog)
    {
        DialogEditorWindow window = (DialogEditorWindow)EditorWindow.GetWindow(typeof(DialogEditorWindow), true, "Add new sentence");
        window.maxSize = new Vector2(400, 230);
        window.minSize = window.maxSize;

        dialog = _dialog;
    }

    private void OnGUI()
    {
        //scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, , GUILayout.MaxHeight(500), GUILayout.ExpandHeight(true));
        EditorGUILayout.BeginVertical();


        EditorGUILayout.BeginVertical();
        GUILayout.Space(10);

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Character");
        character = (Character)EditorGUILayout.ObjectField(character, typeof(Character) , false);
        EditorGUILayout.EndVertical();

        GUILayout.Space(5);
        GUILine();
        GUILayout.Space(5);

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Sentence");
        sentence = EditorGUILayout.TextArea(sentence, GUILayout.MaxHeight(50));
        EditorStyles.textField.wordWrap = true;
        EditorGUILayout.EndVertical();

        GUILayout.Space(5);
        GUILine();
        GUILayout.Space(5);

        EditorGUILayout.BeginVertical();
        hasBranchingDialog = EditorGUILayout.Toggle("Branching dialog", hasBranchingDialog);
        if (hasBranchingDialog)
            branchingDialog = (BranchingDialog)EditorGUILayout.ObjectField(branchingDialog, typeof(BranchingDialog), false);
        EditorGUILayout.EndVertical();

        GUILayout.Space(5);
        GUILine();
        GUILayout.Space(5);

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add"))
        {
            AddSentence();
            this.Close();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        //GUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    public void AddSentence()
    {
        Dialog.DialogData newSentence = new Dialog.DialogData();

        newSentence.character = character;
        newSentence.sentence = sentence;
        newSentence.branchingDialog = branchingDialog;

        dialog.dialogData.Add(newSentence);
    }

    void GUILine(int height = 1) // TODO: Move to ExtensionMethods
    {
        Rect rect = EditorGUILayout.GetControlRect(false, height);
        rect.height = height;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
    }
}
