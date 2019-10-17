using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public new string name;
    public int id;
    public string description;
    public Sprite sprite;
    public Category category;
    public int amount;
    public ItemBehavior action;

    public bool isPickedUp;

    public enum Category
    {
        Key,
        Health,
        PokéBall,
        Battle,
        TM,
        Berry,
        Other
    }
}