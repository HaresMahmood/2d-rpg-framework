using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item/Consumable")]
public class Consumable : Item
{
    #region Variables

    public new Category category { get; private set; }

    #endregion

    #region Enums

    public new enum Category
    {
        Health,
        Berry
    }

    #endregion
}
