using UnityEngine;

/// <summary>
///
/// </summary>
[RequireComponent(typeof(CategorySubComponent))]
public class CategoryHoverButton : HoverButton
{
    #region Miscellaneous Methods

    public override void Select(bool isSelected) // TODO: Debug
    {
        IsSelected = isSelected;

        GetComponentInParent<CategoryComponent>().SelectComponent(GetComponent<CategorySubComponent>(), isSelected);
    }

    #endregion
}

