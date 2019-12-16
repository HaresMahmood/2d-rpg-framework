using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item/Generic")]
public class Item : ScriptableObject
{
    #region Fields

    public string Name;
    public int ID;

    #endregion

    #region Variables

    public string description;
    public Sprite sprite;
    public string effect;
    public Category category;
    public int amount;
    public bool isFavorite;
    public bool isNew;

    public ItemBehavior behavior;

    #endregion

    #region Enums

    public enum Category
    {
        Key,
        Health,
        Poké_Balls,
        Battle,
        TM,
        Berry,
        Other
    }

    #endregion
}