using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Categorizable/Item")]
public class Item : Categorizable
{
    #region Fields

    [SerializeField] private ItemCategory categorization;
    [SerializeField] private Sprite sprite;
    [SerializeField] private ItemEffect effect;
    [SerializeField] private int quantity;
    [SerializeField] private bool isFavorite;
    [SerializeField] private bool isNew;

    #endregion

    #region Properties

    public Sprite Sprite
    {
        get { return sprite; }
        private set { sprite = value; }
    }

    public ItemEffect Effect
    {
        get { return effect; }
        private set { effect = value; }
    }

    public int Quantity
    {
        get { return quantity; }
        private set { quantity = value; }
    }

    public bool IsFavorite
    {
        get { return isFavorite; }
        private set { isFavorite = value; }
    }

    public bool IsNew
    {
        get { return isNew; }
        private set { isNew = value; }
    }

    #endregion

    #region Subclasses

    [System.Serializable]
    public sealed class ItemCategory : Category
    {
        #region Variables

        [SerializeField] private Category value;

        #endregion

        #region Properties

        public override string Value
        {
            get { return value.ToString(); }
        }

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

        public override string GetCategoryFromIndex(int index)
        {
            return ((Category)index).ToString();
        }

        #endregion
    }

    public class ItemEffect
    {

    }

    #endregion

    #region Miscellaneous Methods

    private List<ItemBehavior> HealthBehavior()
    {
        List<ItemBehavior> behavior = new List<ItemBehavior>
        {
            new ItemBehavior("Use", InventoryMenuIcons.instance.Icons[3]),
            new ItemBehavior("Give", InventoryMenuIcons.instance.Icons[4])
        };
        behavior[0].behaviorEvent.AddListener(delegate { FindObjectOfType<InventoryUserInterface>().Use(); });
        behavior[1].behaviorEvent.AddListener(delegate { FindObjectOfType<InventoryUserInterface>().Give(); });

        return behavior;
    }

    private List<ItemBehavior> GenericBehavior(Item item)
    {
        List<ItemBehavior> behavior = new List<ItemBehavior>
        {
            new ItemBehavior("Favorite", InventoryMenuIcons.instance.Icons[0]),
            new ItemBehavior("Discard", InventoryMenuIcons.instance.Icons[1]),
            new ItemBehavior("Cancel", InventoryMenuIcons.instance.Icons[2])
        };
        behavior[0].behaviorEvent.AddListener(delegate { FindObjectOfType<InventoryUserInterface>().Favorite(item); });
        behavior[1].behaviorEvent.AddListener(delegate { FindObjectOfType<InventoryUserInterface>().Discard(item); });

        return behavior;
    }

    public List<ItemBehavior> GenerateButtons()
    {
        List<ItemBehavior> behavior = new List<ItemBehavior>();

        switch (categorization)
        {
            /*
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
            */
        }

        behavior.AddRange(GenericBehavior(this));

        return behavior;
    }

    #endregion
}
