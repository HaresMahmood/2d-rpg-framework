using UnityEditor;
using UnityEngine;

public class DialogEditorWindow : EditorWindow
{
    #region Variables
    private static Dialog dialog;

    private Character character;
    private string sentence;
    private BranchingDialog branchingDialog;
    #endregion

    private static Vector2 windowSize = new Vector2(400, 200); // Default width and height respectively of Editor Window.
    private static bool hasBranchingDialog;

    public static void ShowWindow(Dialog _dialog)
    {
        DialogEditorWindow window = (DialogEditorWindow)EditorWindow.GetWindow(typeof(DialogEditorWindow), true, "New sentence");
        window.maxSize = windowSize;
        window.minSize = window.maxSize;

        dialog = _dialog;
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginVertical();

        GUILayout.Space(10);

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField(new GUIContent("Character", "Character who is conversing this sentence. " +
            "Can be left empty I.E. for system messages through the dialog box."));
        character = (Character)EditorGUILayout.ObjectField(character, typeof(Character), false);
        EditorGUILayout.EndVertical();

        ExtensionMethods.DrawUILine("#969696".ToColor());

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField(new GUIContent("Sentence", "Text displayed in dialog box. " +
            "Note that this is allowed to be multiple sentences long."));
        sentence = EditorGUILayout.TextArea(sentence, GUILayout.MaxHeight(50));
        EditorStyles.textField.wordWrap = true;
        EditorGUILayout.EndVertical();

        ExtensionMethods.DrawUILine("#969696".ToColor());

        EditorGUILayout.BeginVertical();
        hasBranchingDialog = EditorGUILayout.Toggle(new GUIContent("Branching dialog", "Whether or not this sentence contains a dialog " +
                "branch at the end of the sentence. Can be left empty"), hasBranchingDialog);
        if (hasBranchingDialog)
            branchingDialog = (BranchingDialog)EditorGUILayout.ObjectField(branchingDialog, typeof(BranchingDialog), false);
        EditorGUILayout.EndVertical();

        ExtensionMethods.DrawUILine("#969696".ToColor());

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
