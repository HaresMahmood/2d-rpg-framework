using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GUI;

[CustomEditor(typeof(Inventory)), CanEditMultipleObjects]
public class InventoryEditor : Editor
{
    #region Variables

    private Inventory inventory;

    private int tab = 0;
    private string[] categories = new string[] { "Key", "Health", "PokéBall", "Battle", "TM", "Berry", "Other" };

    #endregion

    private void OnEnable()
    {
        inventory = (Inventory)target;
    }

    public override void OnInspectorGUI()
    {
        tab = GUILayout.Toolbar(tab, categories);
        DrawInspector(categories[tab]);

        //base.OnInspectorGUI();
    }

    private void DrawInspector(string category)
    {
        GUILayout.ExpandWidth(false);
        EditorGUIUtility.labelWidth = 15f;
        EditorGUIUtility.fieldWidth = Screen.width - 75;

        EditorGUILayout.BeginVertical();

        GUILayout.Space(5);

        EditorGUILayout.BeginVertical("Box");
        GUILayout.Label("Total items: " + inventory.items.Count);
        EditorGUILayout.EndVertical();

        ExtensionMethods.DrawUILine("#969696".ToColor(), 3);

        EditorGUILayout.EndVertical();

        foreach (Item item in inventory.items.ToArray())
        {
            if (item.category.ToString().Equals(category))
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.BeginVertical("Box");
                GUILayout.Label("Item " + (inventory.items.IndexOf(item) + 1) + ":");
                EditorGUILayout.EndVertical();

                if (GUILayout.Button("Remove", GUILayout.Width(70), GUILayout.Height(25)))
                {
                    inventory.items.Remove(item);
                    EditorUtility.SetDirty(target);
                    return;
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space(2);

                EditorGUILayout.BeginVertical();

                EditorGUILayout.BeginVertical();
                //EditorGUILayout.LabelField(new GUIContent("Item", "Character who is conversing this sentence. " +
                //    "Can be left empty I.E. for system messages through the inventory box."));

                EditorGUILayout.BeginHorizontal();
                inventory.items[inventory.items.IndexOf(item)] = (Item)EditorGUILayout.ObjectField(inventory.items[inventory.items.IndexOf(item)], typeof(Item), false);
                GUILayout.FlexibleSpace();
                EditorGUILayout.PrefixLabel(new GUIContent("X"));
                item.id = EditorGUILayout.IntField(item.id, GUILayout.Width(30));
                EditorGUILayout.EndHorizontal();


                EditorUtility.SetDirty(target);
                EditorGUILayout.EndVertical();

                GUILayout.Space(4);
                ExtensionMethods.DrawUILine("#969696".ToColor());
                GUILayout.Space(2);

                EditorGUILayout.EndVertical();
            }
        }

        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("Add item", "Adds an item entry to this category.")))
        { }

        if (GUILayout.Button(new GUIContent("Remove all items.", "Removes all items of from this category.")))
        {
            foreach (Item item in inventory.items.ToArray())
            {
                if (item.category.ToString().Equals(category))
                {
                    inventory.items.Remove(item);
                }
            }

            EditorUtility.SetDirty(target);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }
}