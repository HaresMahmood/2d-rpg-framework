using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Pokemon)), CanEditMultipleObjects]
public class PokemonEditor : Editor
{
    #region Variables

    private new Pokemon target;

    public event EventHandler OnEdit = delegate { };

    private bool isEditing;

    #endregion

    private void OnEnable()
    {
        target = (Pokemon)base.target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal();
            GUILayout.BeginHorizontal("Box", GUILayout.Height(35));
                GUILayout.FlexibleSpace();

                        if (target.Sprites.MenuSprite != null)
                        {
                            Texture2D itemSprite = target.Sprites.MenuSprite.texture;
                            GUILayout.Label(itemSprite, GUILayout.Width(30), GUILayout.Height(30));
                        }

                        GUILayout.BeginVertical();
                            GUILayout.FlexibleSpace();

                                GUILayout.Label($"#{target.ID:000} - {target.Name} - The {target.Category} Pokémon -");

                            GUILayout.FlexibleSpace();
                        GUILayout.EndVertical();

                    GUILayout.BeginHorizontal();
                        GUILayout.BeginHorizontal("Box", GUILayout.Height(29));
                            GUILayout.BeginVertical();
                                GUILayout.FlexibleSpace();

                                    GUILayout.Label(target.PrimaryType.ToString());

                                GUILayout.FlexibleSpace();
                            GUILayout.EndVertical();
                        GUILayout.EndHorizontal();

                        if (!target.SecondaryType.Equals(Pokemon.Typing.None))
                        {
                            GUILayout.BeginHorizontal("Box", GUILayout.Height(29));
                                GUILayout.BeginVertical();
                                    GUILayout.FlexibleSpace();

                                        GUILayout.Label(target.SecondaryType.ToString());

                                    GUILayout.FlexibleSpace();
                                GUILayout.EndVertical();
                            GUILayout.EndHorizontal();
                        }

                    GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Edit", GUILayout.Width(60), GUILayout.Height(37)))
            {
                //OnEdit?.Invoke(target, EventArgs.Empty);
                isEditing = !isEditing;
            }

        GUILayout.EndHorizontal();

        ExtensionMethods.DrawUILine("#525252".ToColor());

        if (isEditing)
        {
            GUILayout.BeginHorizontal();
               GUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField(new GUIContent("#", "Dex number of this Pokémon.\n\n" +
                    "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(10));
                    target.ID = int.Parse(EditorGUILayout.TextField(target.ID.ToString("000"), GUILayout.Width(30)));

                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField(new GUIContent("Name", "Name of this Pokémon.\n\n" +
                    "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(35));
                    target.Name = (EditorGUILayout.TextField(target.Name, GUILayout.Width(70)));

                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField(new GUIContent("Cat.", "Category of this Pokémon.\n\n" +
                    "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(25));
                    target.Category = EditorGUILayout.TextField(target.Category, GUILayout.Width(85));

                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();

                    target.PrimaryType = (Pokemon.Typing)EditorGUILayout.EnumPopup(target.PrimaryType, GUILayout.Width(60));

                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();

                    target.SecondaryType = (Pokemon.Typing)EditorGUILayout.EnumPopup(target.SecondaryType, GUILayout.Width(60));

                GUILayout.EndHorizontal();

                if (GUILayout.Button("OK", GUILayout.Width(40), GUILayout.Height(20)))
                {
                    //OnEdit?.Invoke(target, EventArgs.Empty);
                    isEditing = false;
                    EditorUtility.SetDirty(target);
                }
  
            GUILayout.EndHorizontal();

            ExtensionMethods.DrawUILine("#525252".ToColor());
        }

        base.OnInspectorGUI();
    }
}
