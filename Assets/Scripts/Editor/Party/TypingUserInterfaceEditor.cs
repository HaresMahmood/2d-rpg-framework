using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TypingUserInterface)), CanEditMultipleObjects]
public class TypingUserInterfaceEditor : Editor
{
    private new TypingUserInterface target;

    private void OnEnable()
    {
        target = (TypingUserInterface)base.target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginVertical("Box");
        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField(new GUIContent("Typing", "Dex number of this Pokémon.\n\n" +
        "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
        target.Value = (Typing.Type)EditorGUILayout.EnumPopup(target.Value);

        if (GUILayout.Button("Apply", GUILayout.Height(18)))
        {
            target.UpdateUserInterface(target.Type, target.Icon);
        }

        GUILayout.EndHorizontal();

        if (target.Value != Typing.Type.None)
        {
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Sprite", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));

            GUI.enabled = false;
            target.Icon = (Sprite)EditorGUILayout.ObjectField(target.Icon, typeof(Sprite), false, GUILayout.Width(90), GUILayout.Height(90));
            GUI.enabled = true;

            GUILayout.Space(10);

            EditorGUILayout.LabelField(new GUIContent("Color", "Dex number of this Pokémon.\n\n" +
            "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));

            GUI.enabled = false;
            target.Type.Color = EditorGUILayout.ColorField(target.Type.Color, GUILayout.Height(90));
            GUI.enabled = true;

            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();

        //base.OnInspectorGUI();
    }
}
