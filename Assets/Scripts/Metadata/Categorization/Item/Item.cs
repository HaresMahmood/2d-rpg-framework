using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        set { Debug.Log(value);  quantity = value; }
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

    public List<ItemBehavior> Behavior
    {
        get 
        { 
            return DefineBehavior(this);
        }
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

    public class ItemBehavior
    {
        public string buttonName;
        public Sprite iconSprite;
        public UnityEvent behaviorEvent = new UnityEvent();

        public ItemBehavior(string buttonName, Sprite iconSprite)
        {
            this.buttonName = buttonName;
            this.iconSprite = iconSprite;
        }
    }

    #endregion

    #region Miscellaneous Methods

    protected virtual List<ItemBehavior> DefineBehavior(Item item) // TODO: Think of beteter way to handle icons
    {
        List<ItemBehavior> behavior = new List<ItemBehavior>
        {
            new ItemBehavior("Favorite", InventoryMenuIcons.instance.Icons[0]),
            new ItemBehavior("Discard", InventoryMenuIcons.instance.Icons[1]),
            new ItemBehavior("Cancel", InventoryMenuIcons.instance.Icons[2])
        };
        behavior[0].behaviorEvent.AddListener(delegate { ((ItemInformationUserInterface)ItemInformationController.Instance.UserInterface).Favorite(item); });
        behavior[1].behaviorEvent.AddListener(delegate { ((ItemInformationUserInterface)ItemInformationController.Instance.UserInterface).Discard(item); });
        behavior[2].behaviorEvent.AddListener(delegate { ((ItemInformationUserInterface)ItemInformationController.Instance.UserInterface).Cancel(); });

        return behavior;
    }

    #endregion
}
