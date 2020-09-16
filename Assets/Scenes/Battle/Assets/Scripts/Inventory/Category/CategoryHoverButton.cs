using UnityEngine;

/// <summary>
///
/// </summary>
[RequireComponent(typeof(CategorySubComponent))]
public class CategoryHoverButton : HoverButton
{
    #region Properties

    public override bool IsSelected 
    { 
        get;
        protected set; 
    }

    #endregion

    #region Miscellaneous Methods

    public override void Select(bool isSelected)
    {
        IsSelected = isSelected;

        GetComponentInParent<CategoryComponent>().SelectComponent(GetComponent<CategorySubComponent>(), isSelected);
    }

    #endregion
}

