using UnityEngine;
using DG.Tweening;
using TMPro;

/// <summary>
///
/// </summary>
public class CategoryComponent : UserInterfaceComponent
{
    #region Variables

    [Header("Settings")]
    [SerializeField, Range(0.01f, 0.5f)] private float animationDuration;

    private int selectedCategory = -1;

    #endregion

    #region Miscellaneous Methods

    public void SelectComponent(CategorySubComponent component, bool isSelected)
    {
        component.Fade(isSelected, animationDuration);

        if (isSelected && selectedCategory != components.IndexOf(component))
        {
            selectedCategory = components.IndexOf(component);

            FadeText(component.transform);
        }
    }

    public void SelectComponent(int increment)
    {
        DeselectComponents(components[ExtensionMethods.IncrementInt(selectedCategory, 0, components.Count, increment)]);
    }

    private void FadeText(Transform position)
    {
        transform.Find("Category Name").GetComponent<TextMeshProUGUI>().SetText(position.name);

        transform.Find("Category Name").GetComponent<RectTransform>().DOLocalMoveX(position.GetComponent<RectTransform>().localPosition.x, animationDuration);
    }

    #endregion
}

