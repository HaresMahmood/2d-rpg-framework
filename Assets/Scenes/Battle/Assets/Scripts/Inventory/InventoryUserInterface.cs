using System;
using System.Linq;
using UnityEngine;

/// <summary>
///
/// </summary>
public class InventoryUserInterface : XUserInterface<Inventory>
{
    #region Variables

    [Header("Settings")]
    [SerializeField, Range(0.01f, 0.5f)] private float animationDuration;

    #endregion

    #region Event Methods

    private void CategoryComponent_OnCategoryChange(object sender, EventArgs e)
    {
        FindComponent("Items").SetInformation(information.items.Where(i => i.Categorization.ToString().Replace("_", " ").Equals(((CategoryComponent)FindComponent("Categories")).SelectedCategory)).ToList());
    }

    private void InventoryUserInterface_OnValueChange(object sender, Item e)
    {
        ((InventoryDetailsComponent)FindComponent("Details")).Animate(e, animationDuration);
    }

    #endregion

    #region Unity Methods

    protected override void Awake()
    {
        base.Awake();

        ((CategoryComponent)FindComponent("Categories")).OnCategoryChange += CategoryComponent_OnCategoryChange;
        ((InventoryGridComponent)FindComponent("Items")).OnValueChange += InventoryUserInterface_OnValueChange;
    }

    protected override void Start()
    { }

    #endregion
}

