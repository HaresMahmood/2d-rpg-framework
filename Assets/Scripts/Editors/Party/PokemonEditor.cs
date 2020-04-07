using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Pokemon)), CanEditMultipleObjects]
public class PokemonEditor : Editor
{
    #region Variables

    private new Pokemon target;

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

        GUILayout.Space(2);
        EditorGUILayout.LabelField("Basic Information", EditorStyles.boldLabel);
        GUILayout.Space(5);

        GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(new GUIContent("#", "Dex number of this Pokémon.\n\n" +
                "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
                target.ID = int.Parse(EditorGUILayout.TextField(target.ID.ToString("000")));

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(new GUIContent("Name", "Name of this Pokémon.\n\n" +
                "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
                target.Name = (EditorGUILayout.TextField(target.Name));

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(new GUIContent("Category", "Category of this Pokémon.\n\n" +
                "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
                target.Category = EditorGUILayout.TextField(target.Category);

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(new GUIContent("Typing", "Category of this Pokémon.\n\n" +
                "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
                target.PrimaryType = (Pokemon.Typing)EditorGUILayout.EnumPopup(target.PrimaryType);
                target.SecondaryType = (Pokemon.Typing)EditorGUILayout.EnumPopup(target.SecondaryType);

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(new GUIContent("Dex Entry", "Category of this Pokémon.\n\n" +
                "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
                target.DexEntry = EditorGUILayout.TextArea(target.DexEntry, GUILayout.MaxHeight(35));
                EditorStyles.textField.wordWrap = true;

            GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        GUILayout.Space(2);
        ExtensionMethods.DrawUILine("#525252".ToColor());
        GUILayout.Space(2);

        EditorGUILayout.LabelField("Progression", EditorStyles.boldLabel);

        GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(new GUIContent("Level", "Category of this Pokémon.\n\n" +
                "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
                target.Level = EditorGUILayout.IntSlider(target.Level, 0, 100);

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(new GUIContent("Experience", "Category of this Pokémon.\n\n" +
                "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
                target.Experience = EditorGUILayout.Slider(target.Experience, 0f, 100f); // TODO: https://bulbapedia.bulbagarden.net/wiki/Experience

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(new GUIContent("Met At Level", "Category of this Pokémon.\n\n" +
                "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
                target.MetAt = EditorGUILayout.IntSlider(target.MetAt, 0, target.Level);

            GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        GUILayout.Space(2);
        ExtensionMethods.DrawUILine("#525252".ToColor());
        GUILayout.Space(2);

        GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(new GUIContent("Ability", "Category of this Pokémon.\n\n" +
                "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
                target.Level = EditorGUILayout.IntSlider(target.Level, 0, 100);

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(new GUIContent("Experience", "Category of this Pokémon.\n\n" +
                "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
                target.Experience = EditorGUILayout.Slider(target.Experience, 0f, 100f); // TODO: https://bulbapedia.bulbagarden.net/wiki/Experience

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(new GUIContent("Nature", "Category of this Pokémon.\n\n" +
                "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
                target.Nature.Value = (Pokemon.PokemonNature.Nature)EditorGUILayout.EnumPopup(target.Nature.Value);

            GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        GUILayout.Space(2);
        ExtensionMethods.DrawUILine("#525252".ToColor());
        GUILayout.Space(2);

        base.OnInspectorGUI();
    }
}
