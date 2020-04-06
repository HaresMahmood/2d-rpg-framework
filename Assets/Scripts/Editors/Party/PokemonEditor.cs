using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Pokemon)), CanEditMultipleObjects]
public class PokemonEditor : Editor
{
    #region Variables

    private Pokemon pokemon;

    #endregion

    private void OnEnable()
    {
        pokemon = (Pokemon)target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal("Box", GUILayout.Height(35));
            GUILayout.FlexibleSpace();

                    if (pokemon.Sprites.MenuSprite != null)
                    {
                        Texture2D itemSprite = pokemon.Sprites.MenuSprite.texture;
                        GUILayout.Label(itemSprite, GUILayout.Width(30), GUILayout.Height(30));
                    }

                    GUILayout.BeginVertical();
                        GUILayout.FlexibleSpace();

                            GUILayout.Label($"{pokemon.Name} - The {pokemon.Category} Pokémon");

                        GUILayout.FlexibleSpace();
                    GUILayout.EndVertical();

                GUILayout.BeginHorizontal();
                    GUILayout.BeginHorizontal("Box", GUILayout.Height(29));
                        GUILayout.BeginVertical();
                            GUILayout.FlexibleSpace();

                                GUILayout.Label(pokemon.PrimaryType.ToString());

                            GUILayout.FlexibleSpace();
                        GUILayout.EndVertical();
                    GUILayout.EndHorizontal();

                    if (!pokemon.SecondaryType.Equals(Pokemon.Typing.None))
                    {
                        GUILayout.BeginHorizontal("Box", GUILayout.Height(29));
                            GUILayout.BeginVertical();
                                GUILayout.FlexibleSpace();

                                    GUILayout.Label(pokemon.SecondaryType.ToString());

                                GUILayout.FlexibleSpace();
                            GUILayout.EndVertical();
                        GUILayout.EndHorizontal();
                    }

                GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        GUILayout.EndHorizontal();


        base.OnInspectorGUI();
    }
}
