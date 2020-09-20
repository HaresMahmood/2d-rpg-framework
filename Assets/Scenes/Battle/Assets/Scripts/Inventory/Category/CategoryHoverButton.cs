using UnityEngine;

/// <summary>
///
/// </summary>
public class CategoryHoverButton : HoverButton
{
    #region Miscellaneous Methods

    public override void Select(bool isSelected)
    {
        IsSelected = isSelected;

        GetComponentInParent<CategoryComponent>().SelectComponent(GetComponent<CategorySubComponent>(), isSelected);
    }

    #endregion
}

