using System;
using UnityEngine;

/// <summary>
///
/// </summary>
[CreateAssetMenu(fileName = "New Usable", menuName = "Categorizable/Item/Usable")]
public class Usable : Item
{
    #region Fields

    [SerializeField] private UsableCategory categorization = new UsableCategory();

    #endregion

    #region Properties

    public override Category Categorization
    {
        get { return categorization; }
    }

    #endregion

    #region Nested Classes

    public sealed class UsableCategory : Category
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
            TM
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

    #endregion
}
