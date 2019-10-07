using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Inventory)), CanEditMultipleObjects]
public class InventoryEditor : Editor
{
    #region Variables

    private Inventory inventory;

    private string[] itemGuids;

    private List<Item> keyItem = new List<Item>();
    private List<Item> healthItem = new List<Item>();
    private List<Item> pokeballItem = new List<Item>();
    private List<Item> battleItem = new List<Item>();
    private List<Item> tmItems = new List<Item>();
    private List<Item> berryItems = new List<Item>();
    private List<Item> otherItems = new List<Item>();

    private int tab = 0;
    private string[] categories = new string[] { "Key", "Heatlh", "PokéBall", "Battle", "TM", "Berry", "Other" };

    #endregion

    private void OnEnable()
    {
        inventory = (Inventory)target;

        itemGuids = AssetDatabase.FindAssets("t:Item", null);
        foreach (string itemGuid in itemGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(itemGuid);
            Item item = (Item)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Item));
            string category = item.category.ToString();
            switch (category)
            {
                case "Key":
                    keyItem.Add(item);
                    break;
                case "Heatlh":
                    healthItem.Add(item);
                    break;
                case "PokéBall":
                    pokeballItem.Add(item);
                    break;
                case "Battle":
                    battleItem.Add(item);
                    break;
                case "TM":
                    tmItems.Add(item);
                    break;
                case "Berry":
                    berryItems.Add(item);
                    break;
                case "Other":
                    otherItems.Add(item);
                    break;
            }
        }

        Debug.Log(healthItem.Count);
    }

    public override void OnInspectorGUI()
    {
        tab = GUILayout.Toolbar(tab, categories);
        for (int i = 0; i < tab; i++)
            DrawInspector(categories[tab]);

        //base.OnInspectorGUI();
    }

    private void DrawInspector(string category)
    {
        List<Item> items = new List<Item>();
        switch (category)
        {
            case "Key":
                items = keyItem;
                break;
            case "Heatlh":
                items = healthItem;
                break;
            case "PokéBall":
                items = pokeballItem;
                break;
            case "Battle":
                items = battleItem;
                break;
            case "TM":
                items = tmItems;
                break;
            case "Berry":
                items = berryItems;
                break;
            case "Other":
                items = otherItems;
                break;
        }

        EditorGUILayout.BeginVertical();

        GUILayout.Space(5);

        EditorGUILayout.BeginVertical("Box");
        GUILayout.Label("Items in category: " + items.Count);
        EditorGUILayout.EndVertical();

        ExtensionMethods.DrawUILine("#969696".ToColor(), 3);

        EditorGUILayout.EndVertical();

        foreach (Item item in items)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical("Box");
            GUILayout.Label("Item " + (this.inventory.items.IndexOf(item) + 1) + ":");
            EditorGUILayout.EndVertical();

            if (GUILayout.Button("Remove", GUILayout.Width(70), GUILayout.Height(25)))
            {
                this.inventory.items.Remove(item);
                EditorUtility.SetDirty(target);
                return;
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(new GUIContent("Item", "Character who is conversing this sentence. " +
                "Can be left empty I.E. for system messages through the inventory box."));
            Item newItem = null;
            newItem = (Item)EditorGUILayout.ObjectField(newItem, typeof(Item), false);
            this.inventory.items.Select(x => x.Equals(item) ? newItem : x);
            EditorUtility.SetDirty(target);
            EditorGUILayout.EndVertical();

            GUILayout.Space(4);
            ExtensionMethods.DrawUILine("#969696".ToColor());
            GUILayout.Space(2);

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("Add sentences", "Adds an item entry to this category.")))


        if (GUILayout.Button(new GUIContent("Remove all items.", "Removes all items of from this category.")))
        {
            //this.inventory.dialogData.Clear();
            EditorUtility.SetDirty(target);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }
}