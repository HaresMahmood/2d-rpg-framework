using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Dialog)), CanEditMultipleObjects]
public class DialogEditor : Editor
{
    #region Variables

    private Dialog dialog;

    private Character character;
    private string sentence;
    private BranchingDialog branchingDialog;

    private int tab = 0;
    private string[] languages = new string[] { "English", "Dutch", "German"};
   
    #endregion

    private void OnEnable()
    {
        dialog = (Dialog)target;
    }

    public override void OnInspectorGUI()
    {
        tab = GUILayout.Toolbar(tab, languages);
        switch (tab)
        {
            case 0:
                DrawInspector(dialog.dialogData);
                break;
            case 1:
                DrawInspector(dialog.dialogDataDutch);
                break;
            case 2:
                DrawInspector(dialog.dialogDataGerman);
                break;
        }

    }

    private void DrawInspector(List<Dialog.DialogData> languageData)
    {
        EditorGUILayout.BeginVertical();

        GUILayout.Space(5);

        EditorGUILayout.BeginVertical("Box");
        GUILayout.Label("Total sentences: " + languageData.Count);
        EditorGUILayout.EndVertical();

        ExtensionMethods.DrawUILine("#969696".ToColor(), 3);

        EditorGUILayout.EndVertical();

        foreach (Dialog.DialogData dialog in languageData)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical("Box");
            GUILayout.Label("Sentence " + (languageData.IndexOf(dialog) + 1) + ":");
            EditorGUILayout.EndVertical();

            if (GUILayout.Button("Remove", GUILayout.Width(70), GUILayout.Height(25)))
            {
                languageData.Remove(dialog);
                EditorUtility.SetDirty(target);
                return;
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(new GUIContent("Character", "Character who is conversing this sentence. " +
                "Can be left empty I.E. for system messages through the inventory box."));
            dialog.character = (Character)EditorGUILayout.ObjectField(dialog.character, typeof(Character), false);
            EditorUtility.SetDirty(target);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(new GUIContent("Sentence", "Text displayed in inventory box. " +
                "Note that this is allowed to be multiple sentences long."));
            dialog.sentence = EditorGUILayout.TextArea(dialog.sentence, GUILayout.MaxHeight(50));
            EditorStyles.textField.wordWrap = true;
            EditorUtility.SetDirty(target);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(new GUIContent("Branching inventory", "Whether or not this sentence contains a inventory " +
                "branch at the end of the sentence. Can be left empty"));
            dialog.branchingDialog = (BranchingDialog)EditorGUILayout.ObjectField(dialog.branchingDialog, typeof(BranchingDialog), false);
            EditorUtility.SetDirty(target);
            EditorGUILayout.EndVertical();

            GUILayout.Space(4);
            ExtensionMethods.DrawUILine("#969696".ToColor());
            GUILayout.Space(2);

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("Add sentences", "Adds an entry to the current inventory.")))
        {
            DialogEditorWindow.ShowWindow(languageData);
            EditorUtility.SetDirty(target);
        }

        if (GUILayout.Button(new GUIContent("Clear all sentences", "Clears all entries from the current inventory.")))
        {
            languageData.Clear();
            EditorUtility.SetDirty(target);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }
}