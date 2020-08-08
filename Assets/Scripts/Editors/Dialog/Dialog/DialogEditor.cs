using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Dialog)), CanEditMultipleObjects]
public class DialogEditor : Editor
{
    #region Variables

    private new Dialog target;

    private static bool showSentences = true;
    private static bool showEdit = false;

    private int tab = 0;

    private List<string> languages = new List<string>();

    #endregion

    private void OnEnable()
    {
        target = (Dialog)base.target;

        for (int i = 0; i < target.Data.Count; i++)
        {
            languages.Add(target.Data[i].Language == "" ? "Unknown Language" : target.Data[i].Language);
        }
    }

    public override void OnInspectorGUI()
    {
        tab = GUILayout.Toolbar(tab, languages.ToArray());

        DrawInspector(target.Data[tab].LanguageData);

        //base.OnInspectorGUI();
    }

    private void DrawInspector(List<Dialog.DialogData> dialogData)
    {
        serializedObject.Update();

        GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout)
        {
            fontStyle = FontStyle.Bold,
            fontSize = 16
        };

        GUILayout.Space(10);

        GUILayout.BeginVertical("Box", GUILayout.Height(35));
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();

        GUILayout.Label($"Displaying {dialogData.Count} Sentences in {(target.Data[tab].Language == "" ? "Unknown Language" : target.Data[tab].Language)}");

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Edit", GUILayout.Width(45), GUILayout.Height(20)))
        {
            showEdit = !showEdit;

            languages[tab] = target.Data[tab].Language == "" ? "Unknown Language" : target.Data[tab].Language;
        }

        GUILayout.EndHorizontal();

        if (showEdit)
        {
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Language", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
            target.Data[tab].Language = EditorGUILayout.TextField(target.Data[tab].Language);

            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Add Language"))
            {
                target.Data.Add(new Dialog.DialogLanguageData());
                languages.Add("Unknown Language");

                tab = target.Data.Count - 1;
            }

            if (GUILayout.Button("Remove Language"))
            {
                target.Data.RemoveAt(tab);
                languages.RemoveAt(tab);

                tab = 0;
            }

            if (GUILayout.Button("Save"))
            {
                showEdit = false;

                languages[tab] = target.Data[tab].Language == "" ? "Unknown Language" : target.Data[tab].Language;
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();

        /*
        GUILayout.BeginHorizontal("Box", GUILayout.Height(35));
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();

        GUILayout.Label($"Displaying {dialogData.Count} Sentences in {(target.Data[tab].Language == "" ? "Unknown Language" : target.Data[tab].Language)}");

        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Edit", GUILayout.Width(45), GUILayout.Height(20)))
        {
            showEdit = !showEdit;
        }

        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        */

        GUILayout.Space(2);
        ExtensionMethods.DrawUILine("#525252".ToColor());
        GUILayout.Space(2);

        GUILayout.BeginHorizontal();

        showSentences = EditorGUILayout.Foldout(showSentences, "Sentences", true, foldoutStyle);

        EditorGUI.BeginDisabledGroup(!showSentences);

        if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
        {
            dialogData.Add(new Dialog.DialogData());
        }

        EditorGUI.EndDisabledGroup();
        GUILayout.EndHorizontal();

        if (showSentences)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginVertical();

            DrawItem(dialogData);

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.Space(2);
            ExtensionMethods.DrawUILine("#525252".ToColor());
            GUILayout.Space(2);
        }
    }

    private void DrawItem(List<Dialog.DialogData> dialogData, bool drawButtons = true)
    {
        GUIStyle miniFoldoutStyle = new GUIStyle(EditorStyles.foldout)
        {
            fixedWidth = 0.1f
        };

        for (int i = 0; i < dialogData.Count; i++)
        {
            //Debug.Log($"i: {i}, j: {j}, c: {counter}");

            Dialog.DialogData sentence = dialogData[i];

            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField($"{i + 1}.", GUILayout.Width(45));

            EditorGUILayout.LabelField(new GUIContent("Char.", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(45));
            sentence.Character = (Character)EditorGUILayout.ObjectField(sentence.Character, typeof(Dialog), false);

            if (drawButtons)
            {
                EditorGUI.BeginDisabledGroup(i == 0);

                if (GUILayout.Button("↑", GUILayout.Width(18), GUILayout.Height(18)))
                {
                    dialogData.RemoveAt(i);
                    dialogData.Insert(i - 1, sentence);
                }

                EditorGUI.EndDisabledGroup();
                EditorGUI.BeginDisabledGroup(i == dialogData.Count - 1);

                if (GUILayout.Button("↓", GUILayout.Width(18), GUILayout.Height(18)))
                {
                    dialogData.RemoveAt(i);
                    dialogData.Insert(i + 1, sentence);
                }

                EditorGUI.EndDisabledGroup();

                if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
                {
                    dialogData.RemoveAt(i);
                }

                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Text", "Name of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(45));
            sentence.Text = EditorGUILayout.TextField(sentence.Text, GUILayout.MaxHeight(35));
            EditorStyles.textField.wordWrap = true;

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            //EditorGUILayout.LabelField(new GUIContent("Branch", "Name of this Pokémon.\n\n" +
            //"- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(45));
            //GUILayout.Space(12.5f);
            sentence.ShowBranch = EditorGUILayout.Foldout(sentence.ShowBranch && sentence.Branch != null, "Branch", true, miniFoldoutStyle);
            sentence.Branch = (BranchingDialog)EditorGUILayout.ObjectField(sentence.Branch, typeof(BranchingDialog), false);

            GUILayout.EndHorizontal();

            if (sentence.ShowBranch && sentence.Branch != null)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(45f);
                GUILayout.BeginVertical();

                for (int j = 0; j < sentence.Branch.Branches.Count; j++)
                {
                    BranchingDialog.DialogBranch branch = sentence.Branch.Branches[j];

                    GUILayout.BeginVertical("Box");
                    GUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField($"{j + 1}.", GUILayout.Width(45));

                    EditorGUILayout.LabelField(new GUIContent("Dialog", "Dex number of this Pokémon.\n\n" +
                    "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(45));
                    branch.NextDialog = (Dialog)EditorGUILayout.ObjectField(branch.NextDialog, typeof(Dialog), false);

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField(new GUIContent("Text", "Name of this Pokémon.\n\n" +
                    "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(45));
                    branch.Text = EditorGUILayout.TextField(branch.Text);

                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                }

                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }

            GUILayout.EndHorizontal();
        }
    }

    /*
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

        if (dialog.dialogDataDutch.Count == 0)
            dialog.dialogDataDutch = dialog.dialogData.ToList();
        else if (dialog.dialogDataGerman.Count == 0)
            dialog.dialogDataGerman = dialog.dialogData.ToList();
    }

    public override void OnInspectorGUI()
    {
        tab = GUILayout.Toolbar(tab, languages);
        switch (tab)
        {
            case 0:
                {
                    DrawInspector(dialog.dialogData);
                    break;
                }
            case 1:
                {
                    DrawInspector(dialog.dialogDataDutch);
                    break;
                }
            case 2:
                {
                    DrawInspector(dialog.dialogDataGerman);
                    break;
                }
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
                "Can be left empty I.E. for system messages through the dialog box."));
            dialog.character = (Character)EditorGUILayout.ObjectField(dialog.character, typeof(Character), false);
            EditorUtility.SetDirty(target);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(new GUIContent("Sentence", "Text displayed in dialog box. " +
                "Note that this is allowed to be multiple sentences long."));
            dialog.sentence = EditorGUILayout.TextArea(dialog.sentence, GUILayout.MaxHeight(50));
            EditorStyles.textField.wordWrap = true;
            EditorUtility.SetDirty(target);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(new GUIContent("Branching dialog", "Whether or not this sentence contains a dialog " +
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
        if (GUILayout.Button(new GUIContent("Add sentence", "Adds an entry to the current dialog.")))
        {
            DialogEditorWindow.ShowWindow(languageData);
            EditorUtility.SetDirty(target);
        }

        EditorGUI.BeginDisabledGroup(languageData.Count == 0);
        if (GUILayout.Button(new GUIContent("Clear all sentences", "Clears all entries from the current dialog.")))
        {
            languageData.Clear();
            EditorUtility.SetDirty(target);
        }
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }
    */
    }