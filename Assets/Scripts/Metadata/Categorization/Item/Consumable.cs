using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
[CreateAssetMenu(fileName = "New Consumable", menuName = "Categorizable/Item/Consumable")]
public class Consumable : Holdable
{
    #region Fields

    [SerializeField] private ConsumableCategory categorization = new ConsumableCategory();
    [SerializeField] private ConsumableEffect effect = new ConsumableEffect();

    #endregion

    #region Properties

    public override Category Categorization
    {
        get { return categorization; }
    }

    public override ItemEffect Effect
    {
        get { return effect; }
    }

    #endregion

    #region Nested Classes

    public sealed class ConsumableCategory : Category
    {
        #region Variables

        private Category value;

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
            Health,
            Berry
        }

        #endregion

        #region Miscellaneous Methods

        public override string GetCategoryFromIndex(int index)
        {
            return ((Category)index).ToString();
        }

        public override int GetTotalCategories()
        {
            return Enum.GetNames(typeof(Category)).Length;
        }

        #endregion
    }

    [Serializable]
    public class ConsumableEffect : ItemEffect
    {
        #region Variables

        [SerializeField] private Type type;
        [ConditionalField("type", false, Type.HP)] [SerializeField] private int quantity; // TODO: Using property for debug.
        [ConditionalField("type", false, Type.STS)] [SerializeField] private Pokemon.StatusAilment status; // TODO: Using property for debug.

        #endregion

        #region Enums

        private enum Type
        {
            HP,
            STS
        }

        /*
        private enum Status
        {
            
        }
        */

        #endregion

        #region Miscellaneous Methods

        public override string GetQuantity()
        {
            return quantity.ToString();
        }

        public override string ToString()
        {
            //string effect = type == 0 ? $"{amount.ToString()} {type.ToString()}" : status.ToString().Replace("None", "All"); // TODO: Using replace for debug.

            string effect = type == 0 ? type.ToString() : status.ToString().Replace("None", "All"); // TODO: Using replace for debug.

            return effect;
        }

        #endregion
    }

    #endregion

    #region Miscellaneous Methods

    protected override List<ItemBehavior> DefineBehavior(Item item)
    {
        List<ItemBehavior> behavior = new List<ItemBehavior>
        {
            new ItemBehavior("Use", InventoryMenuIcons.instance.Icons[3]),
            new ItemBehavior("Give", InventoryMenuIcons.instance.Icons[4])
        };
        behavior[0].behaviorEvent.AddListener(delegate { ((ItemInformationUserInterface)ItemInformationController.Instance.UserInterface).Use(item); });
        //behavior[1].behaviorEvent.AddListener(delegate { FindObjectOfType<InventoryUserInterface>().Give(); });

        behavior.AddRange(base.DefineBehavior(item));

        return behavior;
    }

    #endregion
}