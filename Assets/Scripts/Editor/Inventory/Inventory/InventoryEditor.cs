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
    private static bool showList = true;

    private int tab = 0;
    private List<string> categories = new List<string>();

    private static GUIStyle ToggleButtonStyleNormal = null;
    private static GUIStyle ToggleButtonStyleToggled = null;

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
        /*
        if (ToggleButtonStyleNormal == null)
        {
            ToggleButtonStyleNormal = "Button";
            ToggleButtonStyleToggled = new GUIStyle(ToggleButtonStyleNormal);
            ToggleButtonStyleToggled.normal.background = ToggleButtonStyleToggled.active.background;
        }

        tab = GUILayout.Toolbar(tab, categories.ToArray());
        DrawInspector(categories[tab]);
        */

        base.OnInspectorGUI();
    }
    /*
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
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();

        GUILayout.Label($"Displaying {activeCategorizables.Count}/{target.items.Count} Items.");

        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("L", showList ? ToggleButtonStyleToggled : ToggleButtonStyleNormal, GUILayout.Width(20), GUILayout.Height(20)))
        {
            showList = true;
        }

        if (GUILayout.Button("G", showList ? ToggleButtonStyleNormal : ToggleButtonStyleToggled, GUILayout.Width(20), GUILayout.Height(20)))
        {
            showList = false;
        }

        GUILayout.EndHorizontal();
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

            for (int i = 0; i < activeCategorizables.Count; i++)
            {
                GUILayout.BeginHorizontal();

                for (int j = 1; j < (showList ? 2 : 3); j++)
                {
                    if (i == activeCategorizables.Count - 1 && !showList)
                    {
                        break;
                    }

                    int counter = Mathf.Clamp((i - 1) + j, 0, activeCategorizables.Count - 1);

                    //Debug.Log($"i: {i}, j: {j}, c: {counter}");

                    Item item = activeCategorizables[counter];

                    GUILayout.BeginVertical("Box");
                    GUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField($"{counter + 1}.", GUILayout.Width(45));

                    EditorGUILayout.LabelField(new GUIContent("Item", "Dex number of this Pokémon.\n\n" +
                    "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(45));
                    Item newItem = (Item)EditorGUILayout.ObjectField(target.items[target.items.IndexOf(item)], typeof(Item), false);
                    target.items[target.items.FindIndex(it => it == item)] = newItem;
                    item = newItem;

                    EditorGUI.BeginDisabledGroup(counter == 0);

                    if (GUILayout.Button("↑", GUILayout.Width(18), GUILayout.Height(18)))
                    {
                        activeCategorizables.RemoveAt(counter);
                        activeCategorizables.Insert(counter - 1, item);
                    }

                    EditorGUI.EndDisabledGroup();
                    EditorGUI.BeginDisabledGroup(counter == activeCategorizables.Count - 1);

                    if (GUILayout.Button("↓", GUILayout.Width(18), GUILayout.Height(18)))
                    {
                        activeCategorizables.RemoveAt(counter);
                        activeCategorizables.Insert(counter + 1, item);
                    }

                    EditorGUI.EndDisabledGroup();

                    if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18)))
                    {
                        activeCategorizables.RemoveAt(counter);
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
                        /*
                        GUILayout.BeginHorizontal();

                        EditorGUILayout.LabelField(new GUIContent("Desc.", "Name of this Pokémon.\n\n" +
                        "- Must be unique for every Pokémon.\n- Number must not be larger than 3 digits."), GUILayout.Width(45));
                        GUI.enabled = false;
                        EditorGUILayout.SelectableLabel(item.Description, EditorStyles.textArea);
                        GUI.enabled = true;

                        GUILayout.EndHorizontal();
                    }

                    GUILayout.EndHorizontal();
                }

                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
        
    }
    */
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