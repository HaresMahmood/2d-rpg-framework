using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public new string name;
    public int id;
    public string description;
    public Sprite sprite;
    public string effect;
    public Category category;
    public int amount;
    public ItemBehavior behavior;
    public bool isFavorite;
    public bool isNew;

    public enum Category
    {
        Key,
        Health,
        PokéBalls,
        Battle,
        TM,
        Berry,
        Other
    }
}