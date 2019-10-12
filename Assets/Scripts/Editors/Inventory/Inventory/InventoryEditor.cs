using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

[CustomEditor(typeof(Inventory)), CanEditMultipleObjects]
public class InventoryEditor : Editor
{
    #region Variables

    private Inventory inventory;

    private bool sameItem = false;
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
        int counter = 0; foreach (Item item in inventory.items.ToArray())
            if (item.category.ToString().Equals(category)) counter++;
        GUILayout.Label("Displaying " + counter + "/" + inventory.items.Count + " items.");
        EditorGUILayout.EndVertical();

        ExtensionMethods.DrawUILine("#969696".ToColor(), 3);

        EditorGUILayout.EndVertical();

        counter = 0;  foreach (Item item in inventory.items.ToArray())
        {
            if (item.category.ToString().Equals(category))
            {
                counter++;
                EditorGUILayout.BeginHorizontal();
                if (item.sprite != null)
                {
                    Texture2D itemSprite = item.sprite.texture;
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(itemSprite, GUILayout.Width(30), GUILayout.Height(30));
                }
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical("Box");
                float boxWidth = 0;
                if (item.sprite != null)
                    boxWidth = Screen.width - 150;
                else
                    boxWidth = Screen.width - 112;
                GUILayout.Label(("Slot " + counter + ":"), GUILayout.Width(boxWidth));
                EditorGUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Remove", GUILayout.Width(70), GUILayout.Height(25)))
                {
                    item.amount = 0;
                    inventory.items.Remove(item);
                    EditorUtility.SetDirty(target);
                    return;
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space(2);

                EditorGUILayout.BeginVertical();

                EditorGUILayout.BeginVertical();

                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();
                inventory.items[inventory.items.IndexOf(item)] = (Item)EditorGUILayout.ObjectField(inventory.items[inventory.items.IndexOf(item)], typeof(Item), false, GUILayout.Width(Screen.width - 115));
                inventory.items = inventory.items.Distinct().ToList();
                EditorUtility.SetDirty(target);
                GUILayout.FlexibleSpace();
                EditorGUILayout.PrefixLabel(new GUIContent("X"));
                if (GUILayout.Button(new GUIContent("-", "Decrements the amount of this item in the inventory."), GUILayout.Height(18), GUILayout.Width(18)))
                    if (item.amount > 1) item.amount--; EditorUtility.SetDirty(item);
                item.amount = EditorGUILayout.IntField(item.amount, GUILayout.Width(30));
                if (GUILayout.Button(new GUIContent("+", "Increments the amount of this item in the inventory."), GUILayout.Height(18), GUILayout.Width(18)))
                    if (item.amount < 999) item.amount++; EditorUtility.SetDirty(item);
                EditorGUILayout.EndHorizontal();

                EditorUtility.SetDirty(target);
                EditorGUILayout.EndVertical();

                GUILayout.Space(3);
                ExtensionMethods.DrawUILine("#969696".ToColor());
                GUILayout.Space(2);

                EditorGUILayout.EndVertical();
            }
        }

        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("Add item", "Adds an item entry to this category.")))
            AddItem(category);

        EditorGUI.BeginDisabledGroup(counter == 0);
        if (GUILayout.Button(new GUIContent("Clear category", "Removes all items of from this category.")))
        {
            foreach (Item item in inventory.items.ToArray())
            {
                if (item.category.ToString().Equals(category))
                    inventory.items.Remove(item);
            }

            EditorUtility.SetDirty(target);
        }
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }

    public void AddItem(string category)
    {
        Item.Category.TryParse(category, out Item.Category itemCategory);
        inventory.items.Add
        (
            new Item
            {
                category = itemCategory
            }
        ); 

        EditorUtility.SetDirty(target);
    }
}