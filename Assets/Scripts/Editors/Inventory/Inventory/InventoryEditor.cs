using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

[CustomEditor(typeof(Inventory)), CanEditMultipleObjects]
public class InventoryEditor : Editor
{
    #region Variables

    private new Inventory target;

    private static bool showItem = true;

    private bool sameItem = false;
    private int tab = 0;
    private List<string> categories = new List<string>();

    #endregion

    private void OnEnable()
    {
        target = (Inventory)base.target;

        for (int i = 0; i < target.items[0].Categorization.GetTotalCategories(); i++)
        {
            categories.Add(target.items[0].Categorization.GetCategoryFromIndex(i));
        }
    }

    public override void OnInspectorGUI()
    {
        tab = GUILayout.Toolbar(tab, categories.ToArray());
        DrawInspector(categories[tab]);

        //base.OnInspectorGUI();
    }

    private void DrawInspector(string category)
    {
        GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout)
        {
            fontStyle = FontStyle.Bold,
            fontSize = 16
        };

        List<Item> activeCategorizables = target.items.Where(categorizable => categorizable.Categorization.ToString().Equals(category)).ToList();

        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.BeginHorizontal("Box", GUILayout.Height(35));
        GUILayout.FlexibleSpace();

        //if (target.Sprite != null)
        //{
        //    Texture2D itemSprite = target.Sprite.texture;
        //    GUILayout.Label(itemSprite, GUILayout.Width(30), GUILayout.Height(30));
        //}

        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();

        GUILayout.Label($"Displaying {activeCategorizables.Count}/{target.items.Count} Items.");

        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndHorizontal();

        GUILayout.Space(2);
        ExtensionMethods.DrawUILine("#525252".ToColor());
        GUILayout.Space(2);

        GUILayout.BeginHorizontal();

        showItem = EditorGUILayout.Foldout(showItem, "Item", foldoutStyle);

        EditorGUI.BeginDisabledGroup(!showItem);

        if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
        {
            //target.Goals.Add(new Mission.MissionGoal());
        }

        EditorGUI.EndDisabledGroup();
        GUILayout.EndHorizontal();

        if (showItem)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginVertical();
            GUILayout.BeginVertical();
            GUILayout.BeginVertical();

            for (int i = 0; i < activeCategorizables.Count; i++)
            {
                Item item = activeCategorizables[i];

                GUILayout.BeginVertical("Box");
                GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField($"{i + 1}.", GUILayout.Width(45));

                EditorGUILayout.LabelField(new GUIContent("Item", "Dex number of this Pokémon.\n\n" +
                "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(95));
                Item newItem = (Item)EditorGUILayout.ObjectField(target.items[target.items.IndexOf(item)], typeof(Item), false);
                target.items[target.items.FindIndex(it => it == item)] = newItem;
                item = newItem;

                EditorGUI.BeginDisabledGroup(i == 0);

                if (GUILayout.Button("↑", GUILayout.Width(18), GUILayout.Height(18)))
                {
                    activeCategorizables.RemoveAt(i);
                    activeCategorizables.Insert(i - 1, item);
                }

                EditorGUI.EndDisabledGroup();
                EditorGUI.BeginDisabledGroup(i == activeCategorizables.Count - 1);

                if (GUILayout.Button("↓", GUILayout.Width(18), GUILayout.Height(18)))
                {
                    activeCategorizables.RemoveAt(i);
                    activeCategorizables.Insert(i + 1, item);
                }

                EditorGUI.EndDisabledGroup();

                if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
                {
                    activeCategorizables.RemoveAt(i);
                }

                GUILayout.EndHorizontal();

                if (item != null)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField(new GUIContent("Qty.", "Name of this Pokémon.\n\n" +
                    "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(45));

                    if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18)))
                    {
                        item.Quantity = Mathf.Clamp(++item.Quantity, 1, 999);
                    }

                    item.Quantity = EditorGUILayout.IntField(item.Quantity, GUILayout.Width(36));

                    if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
                    {
                        item.Quantity = Mathf.Clamp(--item.Quantity, 1, 999);
                    }

                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField(new GUIContent("Fav.", "Name of this Pokémon.\n\n" +
                    "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(35));
                    item.IsFavorite = GUILayout.Toggle(item.IsFavorite, GUIContent.none);

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField(new GUIContent("New", "Name of this Pokémon.\n\n" +
                    "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(35));
                    item.IsNew = GUILayout.Toggle(item.IsNew, GUIContent.none);

                    GUILayout.EndHorizontal();
                    GUILayout.EndHorizontal();
                    GUILayout.EndHorizontal();
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField(new GUIContent("Desc.", "Name of this Pokémon.\n\n" +
                    "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(45));
                    GUI.enabled = false;
                    EditorGUILayout.SelectableLabel(item.Description, EditorStyles.textArea);
                    GUI.enabled = true;

                    GUILayout.EndHorizontal();
                }

                GUILayout.EndVertical();
            }

            GUILayout.EndVertical();
            GUILayout.EndVertical();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
    }

    public void AddItem(string category)
    {
        /*
        Item.ItemCategory.Category.TryParse(category, out Item.ItemCategory.Category itemCategory);
        inventory.items.Add
        (
            new Item
            {
                Categorization. = itemCategory
            }
        ); 
        */

        EditorUtility.SetDirty(base.target);
    }
}