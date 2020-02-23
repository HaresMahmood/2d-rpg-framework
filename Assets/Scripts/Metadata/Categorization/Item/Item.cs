using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Categorizable/Item/Generic")]
public class Item : Categorizable
{
    #region Fields

    [SerializeField] private ItemCategory categorization = new ItemCategory();
    [SerializeField] private Sprite sprite;
    [SerializeField] private ItemEffect effect = new ItemEffect();
    [SerializeField] private int quantity;
    [SerializeField] private bool isFavorite;
    [SerializeField] private bool isNew;

    #endregion

    #region Properties

    public override Category Categorization 
    {
        get { return categorization; } 
    }

    public Sprite Sprite
    {
        get { return sprite; }
        private set { sprite = value; }
    }

    public virtual ItemEffect Effect
    {
        get { return effect; }
    }

    public int Quantity
    {
        get { return quantity; }
        set { quantity = value; }
    }

    public bool IsFavorite
    {
        get { return isFavorite; }
        set { isFavorite = value; }
    }

    public bool IsNew
    {
        get { return isNew; }
        set { isNew = value; }
    }

    #endregion

    #region Nested Classes

    [Serializable]
    public sealed class ItemCategory : Category
    {
        #region Variables

        [SerializeField] private Category value;

        #endregion

        #region Properties

        protected override string Value
        {
            get { return value.ToString(); }
        }

        #endregion

        #region Enums

        public enum Category
        {
            Key
        }

        public enum CategoryConstant
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
            return ((CategoryConstant)index).ToString();
        }

        public override int GetTotalCategories()
        {
            return Enum.GetNames(typeof(CategoryConstant)).Length;
        }

        #endregion
    }

    public class ItemEffect
    {
        #region Miscellaneous Methods

        public virtual string GetQuantity()
        {
            return "";
        }

        public override string ToString()
        {
            return "None";
        }

        #endregion
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
        //behavior[0].behaviorEvent.AddListener(delegate { FindObjectOfType<InventoryUserInterface>().Use(); });
        //behavior[1].behaviorEvent.AddListener(delegate { FindObjectOfType<InventoryUserInterface>().Give(); });

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
        //behavior[0].behaviorEvent.AddListener(delegate { FindObjectOfType<InventoryUserInterface>().Favorite(item); });
        //behavior[1].behaviorEvent.AddListener(delegate { FindObjectOfType<InventoryUserInterface>().Discard(item); });

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
