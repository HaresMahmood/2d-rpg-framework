using System;
using UnityEngine;

/// <summary>
///
/// </summary>
[CreateAssetMenu(fileName = "New Poké Ball", menuName = "Categorizable/Item/Poké Ball")]
public class PokeBall : Holdable
{
    #region Fields

    [SerializeField] private PokeBallCategory categorization = new PokeBallCategory();

    #endregion

    #region Properties

    public override Category Categorization
    {
        get { return categorization; }
    }

    #endregion

    #region Nested Classes

    public sealed class PokeBallCategory : Category
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
            Poké_Balls
        }

        #endregion
    }

    #endregion
}
