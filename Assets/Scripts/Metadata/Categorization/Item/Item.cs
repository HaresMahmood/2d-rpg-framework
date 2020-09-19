using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Item", menuName = "Categorizable/Item")]
public class Item : Categorizable
{
    #region Fields

    [SerializeField] private Sprite sprite;
    [SerializeField] private ItemCategory categorization = new ItemCategory();
    [SerializeField] private ItemTags tags = new ItemTags();
    [SerializeField] private ItemEffect effect = new ItemEffect();

    #endregion

    #region Properties

    public Sprite Sprite
    {
        get { return sprite; }
        set { sprite = value; }
    }

    public override Category Categorization 
    {
        get { return categorization; } 
    }

    public virtual ItemTags Tags
    {
        get { return tags; }
    }

    public virtual ItemEffect Effect
    {
        get { return effect; }
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
    public class ItemCategory : Category
    {
        #region Variables

        [SerializeField] private Category value;

        #endregion

        #region Properties

        public Category Value
        {
            get { return value; }
            set { this.value = (Category)Mathf.Clamp((int)value, 0, Enum.GetNames(typeof(Category)).Length - 1); }
        }

        public override string ToString()
        {
            return value.ToString();
        }

        #endregion

        #region Enums

        public enum Category
        {
            Key,
            Poké_Balls,
            Health,
            Berry,
            Battle,
            TM,
            Other
        }

        #endregion
    }

    [Serializable]
    public class ItemTags
    {
        #region Fields

        [SerializeField] private int quantity;
        [SerializeField] private bool isFavorite;
        [SerializeField] private bool isNew;

        #endregion

        #region Properties

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
    }

    [Serializable]
    public class ItemEffect
    {

        // TODO: Debug until ItemEditor is updated

        #region Variables

        [Header("Health & Berries")]
        [SerializeField] private Type type;
        [SerializeField] private PartyMember.StatusAilment.Ailment status;

        [Header("Battle")]
        [SerializeField] private Pokemon.Stat stat;

        [Header("Value")]
        [SerializeField] private float amount;

        #endregion

        #region Enums

        private enum Type
        {
            HP,
            STS
        }

        #endregion

        #region Miscellaneous Methods

        public string GetValue(ItemCategory.Category category)
        {
            string pokeballs = $"{amount}x Max Catch Rate";
            string health = $"Heals {(type == Type.HP ? $"{amount} {Type.HP}" : status.ToString())}";
            string battle = $"Raises {stat} by {amount} Level";


            return category == ItemCategory.Category.Poké_Balls ? pokeballs : ((category == ItemCategory.Category.Health || category == ItemCategory.Category.Berry) ? health : battle);
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


        return null;
    }

    #endregion
}
