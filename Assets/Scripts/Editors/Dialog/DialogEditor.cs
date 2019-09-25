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
    #endregion

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

        ExtensionMethods.DrawUILine("#969696".ToColor(), 3);

        EditorGUILayout.EndVertical();

        foreach (Dialog.DialogData dialog in dialog.dialogData)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical("Box");
            GUILayout.Label("Sentence " + (this.dialog.dialogData.IndexOf(dialog) + 1) + ":");
            EditorGUILayout.EndVertical();

            if (GUILayout.Button("Remove", GUILayout.Width(70), GUILayout.Height(25)))
            {
                this.dialog.dialogData.Remove(dialog);
                EditorUtility.SetDirty(target);
                return;
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Character");
            dialog.character = (Character)EditorGUILayout.ObjectField(dialog.character, typeof(Character), false);
            EditorUtility.SetDirty(target);
            EditorGUILayout.EndVertical();

            ExtensionMethods.DrawUILine("#969696".ToColor());

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Sentence");
            dialog.sentence = EditorGUILayout.TextArea(dialog.sentence, GUILayout.MaxHeight(50));
            EditorStyles.textField.wordWrap = true;
            EditorUtility.SetDirty(target);
            EditorGUILayout.EndVertical();

            ExtensionMethods.DrawUILine("#969696".ToColor());

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Branching dialog");
            dialog.branchingDialog = (BranchingDialog)EditorGUILayout.ObjectField(dialog.branchingDialog, typeof(BranchingDialog), false);
            EditorUtility.SetDirty(target);
            EditorGUILayout.EndVertical();

            ExtensionMethods.DrawUILine("#969696".ToColor());

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add sentence"))
            DialogEditorWindow.ShowWindow(this.dialog);

        if (GUILayout.Button("Clear all sentences"))
        {
            this.dialog.dialogData.Clear();
            EditorUtility.SetDirty(target);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }
}