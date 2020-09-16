using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
///
/// </summary>
public class CategorySubComponent : UserInterfaceSubComponent
{
    #region Variables

    private Image[] icons;

    #endregion

    #region Miscellaneous Methods

    public void Fade(bool isSelected, float animationDuration)
    {
        foreach (Image icon in icons)
        {
            icon.DOColor(isSelected ? GameManager.instance.accentColor : Color.white, animationDuration);
        }
    }

    public override void SetInspectorValues()
    {
        icons = GetComponentsInChildren<Image>();
    }

    #endregion
}

