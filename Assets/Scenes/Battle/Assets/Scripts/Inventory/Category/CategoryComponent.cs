using System;
using UnityEngine;
using DG.Tweening;
using TMPro;

/// <summary>
///
/// </summary>
public class CategoryComponent : UserInterfaceComponent
{
    #region Properties

    public string SelectedCategory
    {
        get
        {
            return components[selectedCategory].name;
        }
    }

    #endregion

    #region Variables

    [Header("Settings")]
    [SerializeField, Range(0.01f, 0.5f)] private float animationDuration;

    private int selectedCategory = -1;

    #endregion

    #region Events

    public event EventHandler OnCategoryChange;

    #endregion

    #region Miscellaneous Methods

    public void SelectComponent(CategorySubComponent selectedComponent, bool isSelected)
    {
        selectedComponent.Fade(isSelected, animationDuration);

        if (isSelected && selectedCategory != components.IndexOf(selectedComponent))
        {
            selectedCategory = components.IndexOf(selectedComponent);

            selectedComponent.Animate();
            FadeText(selectedComponent.transform);

            OnCategoryChange?.Invoke(this, EventArgs.Empty);
        }
    }

    public void SelectComponent(int increment)
    {
        DeselectComponents(components[ExtensionMethods.IncrementInt(selectedCategory, 0, components.Count, increment)]);
    }

    private void FadeText(Transform position)
    {
        if (transform.Find("Category Icons/Category Name").transform.localPosition.x != position.localPosition.x)
        {
            transform.Find("Category Icons/Category Name").GetComponent<TextMeshProUGUI>().SetText(position.name);

            transform.Find("Category Icons/Category Name").GetComponent<RectTransform>().DOLocalMoveX(position.GetComponent<RectTransform>().localPosition.x, animationDuration).OnComplete(() => FadeText(position));
        }
    }

    #endregion

    #region Unity Methods

    protected override void Start()
    {
        base.Start();

        GetComponent<ButtonPromptController>().SetInformation(GetComponent<ButtonList>().PromptGroups);
    }

    #endregion
}

