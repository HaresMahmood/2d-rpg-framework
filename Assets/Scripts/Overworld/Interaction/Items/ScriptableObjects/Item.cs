using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public new string name;
    public int id;
    public string description;
    public Sprite sprite;
    public Category category;
    public int amount;

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

    internal object Clone()
    {
        throw new NotImplementedException();
    }
}