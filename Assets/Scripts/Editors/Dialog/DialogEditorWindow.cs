using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class DialogEditorWindow : EditorWindow
{
    public Character character;
    public string sentence;
    public BranchingDialog choices;

    public static Dialog dialog;

    //public List<SerializedObject> serializedList = new List<SerializedObject>();

    public bool hasBranchingDialog = false;

    public static void ShowWindow(Dialog _dialog)
    {
        EditorWindow window = EditorWindow.GetWindow(typeof(DialogEditorWindow));
        window.maxSize = new Vector2(400, 220);
        window.minSize = window.maxSize;

        dialog = _dialog;
    }

    private void OnGUI()
    {
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
        EditorGUILayout.EndVertical();

        GUILayout.Space(5);
        GUILine();
        GUILayout.Space(5);

        EditorGUILayout.BeginVertical();
        hasBranchingDialog = EditorGUILayout.Toggle("Branching dialog", hasBranchingDialog);
        if (hasBranchingDialog)
        {
            choices = (BranchingDialog)EditorGUILayout.ObjectField(choices, typeof(BranchingDialog), false);

            /*
            if (GUILayout.Button("Add dialog branch"))
            {
                serializedObj.Update();
                EditorGUILayout.PropertyField(serializedProperty, true);
                serializedObj.ApplyModifiedProperties();
            }
            */
        }
        EditorGUILayout.EndVertical();

        GUILayout.Space(5);
        GUILine();
        GUILayout.Space(5);

        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("Add"))
        {
            AddSentence();
            this.Close();
        }
        EditorGUILayout.EndVertical();
    }

    public void AddSentence()
    {
        Dialog.DialogInfo newSentence = new Dialog.DialogInfo();

        newSentence.character = character;
        newSentence.sentence = sentence;
        newSentence.choices = choices;

        dialog.dialog.Add(newSentence);
    }

    void GUILine(int height = 1) // TODO: Move to ExtensionMethods
    {
        Rect rect = EditorGUILayout.GetControlRect(false, height);
        rect.height = height;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
    }
}
