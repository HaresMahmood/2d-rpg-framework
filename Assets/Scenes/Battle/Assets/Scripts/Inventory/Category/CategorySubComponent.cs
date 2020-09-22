using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
///
/// </summary>
public class CategorySubComponent : UserInterfaceSubComponent
{
    #region Properties

    public float AnimationDuration { private get; set; }

    #endregion

    #region Variables

    private Image[] icons;

    #endregion

    #region Miscellaneous Methods

    public void FadeColor(bool isSelected, float animationDuration)
    {
        foreach (Image icon in icons)
        {
            icon.DOColor(isSelected ? GameManager.instance.accentColor : Color.white, animationDuration);
        }
    }

    public void FadeOpacity(float opacity, float animationDuration)
    {
        GetComponent<CanvasGroup>().DOFade(opacity, animationDuration);
    }

    public void Animate()
    {
        UIAnimation animation = GetComponent(typeof(UIAnimation)) as UIAnimation;

        animation.Play();
    }

    public override void Select(bool isSelected)
    {
        FadeColor(isSelected, AnimationDuration);
    }

    public override void SetInspectorValues()
    {
        icons = GetComponentsInChildren<Image>();
    }

    #endregion
}

