using System;
using System.Linq;

/// <summary>
///
/// </summary>
public class InventoryUserInterface : XUserInterface<Inventory>
{
    #region Event Methods

    private void CategoryComponent_OnCategoryChange(object sender, EventArgs e)
    {
        FindComponent("Items").SetInformation(information.items.Where(i => i.Categorization.ToString().Replace("_", " ").Equals(((CategoryComponent)FindComponent("Categories")).SelectedCategory)).ToList());
    }

    #endregion

    #region Unity Methods

    protected override void Awake()
    {
        base.Awake();

        ((CategoryComponent)FindComponent("Categories")).OnCategoryChange += CategoryComponent_OnCategoryChange;
    }

    protected override void Start()
    { }

    #endregion
}

