using System;
using System.Linq;
using System.Collections.Generic;
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

    public override void DeselectComponents(UserInterfaceSubComponent selectedComponent)
    {
        List<UserInterfaceSubComponent> components = this.components.Where(c => c != selectedComponent && c.GetComponent<HoverButton>().IsSelected).ToList();

        if (selectedCategory != this.components.IndexOf(selectedComponent))
        {
            selectedCategory = this.components.IndexOf(selectedComponent);

            selectedComponent.GetComponent<HoverButton>().Select(true);
            FadeText(selectedComponent.transform);

            OnCategoryChange?.Invoke(this, EventArgs.Empty);
        }

        foreach (UserInterfaceSubComponent component in components)
        {
            component.GetComponent<HoverButton>().Select(false);
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

    protected override void Awake()
    {
        base.Awake();

        foreach (CategorySubComponent component in components)
        {
            component.AnimationDuration = animationDuration;
        }
    }

    protected override void Start()
    {
        base.Start();

        GetComponent<ButtonPromptController>().SetInformation(GetComponent<ButtonList>().PromptGroups);
    }

    #endregion
}

