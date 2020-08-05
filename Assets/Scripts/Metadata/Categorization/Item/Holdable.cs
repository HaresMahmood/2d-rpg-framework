using System;
using UnityEngine;

/// <summary>
///
/// </summary>
[CreateAssetMenu(fileName = "New Holdable", menuName = "Categorizable/Item/Holdable")]
public class Holdable : Item
{
    #region Fields

    [SerializeField] private HoldableCategory categorization = new HoldableCategory();

    #endregion

    #region Properties

    public override Category Categorization
    {
        get { return categorization; }
    }

    #endregion

    #region Nested Classes

    public sealed class HoldableCategory : Category
    {
        #region Variables

        private Category value;

        #endregion

        #region Properties

        protected override string StringValue
        {
            get { return value.ToString(); }
        }

        #endregion

        #region Enums

        public enum Category
        {
            Battle,
            Other
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
