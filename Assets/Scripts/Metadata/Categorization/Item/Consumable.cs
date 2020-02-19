﻿using System;
using UnityEngine;

/// <summary>
///
/// </summary>
[CreateAssetMenu(fileName = "New Consumable", menuName = "Categorizable/Item/Consumable")]
public class Consumable : Item
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
        [ConditionalField("type", false, Type.HP)] [SerializeField] private int amount; // TODO: Using property for debug.
        [ConditionalField("type", false, Type.Status)] [SerializeField] private Pokemon.Status status; // TODO: Using property for debug.

        #endregion

        #region Enums

        private enum Type
        {
            HP,
            Status
        }

        /*
        private enum Status
        {
            
        }
        */

        #endregion

        #region Miscellaneous Methods

        public override string ToString()
        {
            string effect = type == 0 ? $"{amount.ToString()} {type.ToString()}" : status.ToString().Replace("None", "All"); // TODO: Using replace for debug.

            return effect;
        }

        #endregion
    }

    #endregion
}