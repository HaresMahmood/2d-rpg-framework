using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    #region Fields

    [SerializeField] private int id; // PropertyBackingField, RevealProperty("ID")

    #endregion

    #region Properties

    public string Name;
    public int ID
    {
        get { return id; }
        private set { id = value; }
    }

    #endregion

    #region Variables

    public string description;
    public Sprite sprite;
    public string effect;
    public Category category;
    public int amount;
    public bool isFavorite;
    public bool isNew;

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

    #region Miscellaneous Methods

    private List<ItemBehavior> HealthBehavior()
    {
        List<ItemBehavior> behavior = new List<ItemBehavior>
        {
            new ItemBehavior("Use"),
            new ItemBehavior("Give")
        };
        behavior[0].behaviorEvent.AddListener(delegate { FindObjectOfType<InventoryUserInterface>().Use(); });
        behavior[1].behaviorEvent.AddListener(delegate { FindObjectOfType<InventoryUserInterface>().Give(); });

        return behavior;
    }

    private List<ItemBehavior> GenericBehavior(Item item)
    {
        List<ItemBehavior> behavior = new List<ItemBehavior>
        {
            new ItemBehavior("Favorite"),
            new ItemBehavior("Discard"),
            new ItemBehavior("Cancel")
        };
        behavior[0].behaviorEvent.AddListener(delegate { FindObjectOfType<InventoryUserInterface>().Favorite(item); });
        behavior[1].behaviorEvent.AddListener(delegate { FindObjectOfType<InventoryUserInterface>().Discard(item); });

        return behavior;
    }

    public List<ItemBehavior> GenerateButtons()
    {
        List<ItemBehavior> behavior = new List<ItemBehavior>();

        switch (category)
        {
            default: { break; }
            case (Category.Health):
                {
                    behavior = HealthBehavior();
                    break;
                }
            case (Category.Berry):
                {
                    behavior = HealthBehavior();
                    break;
                }
        }

        behavior.AddRange(GenericBehavior(this));

        return behavior;
    }

    #endregion
}