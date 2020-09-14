using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


//////////////////////////////////////
// TODO: Basically a complete redo
// This is a proof of concept!
//////////////////////////////////////


/// <summary>
///
/// </summary>
public class CategoryComponent : MonoBehaviour
{
    #region Properties

    public int SelectedCategory { get; set; }

    #endregion

    #region Variables

    [Header("Setup")]
    [SerializeField] private List<string> categories = new List<string>();

    [Header("Settings")]
    [SerializeField, Range(0.01f, 2f)] private float animationTime = 0.2f;

    private List<CategoryHoverButton> components;

    #endregion

    #region Miscellaneous Methods

    public void SelectComponent(CategoryHoverButton component, bool isSelected)
    {
        component.IsSelected = isSelected;
        SelectedCategory = components.IndexOf(component);

        component.GetComponentInChildren<Image>().DOColor(isSelected ? GameManager.instance.accentColor : Color.white, animationTime); // Tint all images
        //component.GetComponentInChildren<Image>().GetComponent<RectTransform>().DOAnchorPosY(component.transform.position.y + (isSelected ? 3 : -3), animationTime);

        if (isSelected)
        {
            FadeText(component.transform);
        }
    }

    public void DeselectComponents(CategoryHoverButton selectedComponent)
    {
        List<CategoryHoverButton> components = this.components.Where(c => c != selectedComponent && c.IsSelected).ToList();

        SelectComponent(selectedComponent, true);

        foreach (CategoryHoverButton component in components)
        {
            SelectComponent(component, false);
        }
    }

    public void SetInformation(List<Item> inventory)
    {
        List<Item> currentCategory = inventory.Where(i => i.Categorization.ToString().Equals(categories[SelectedCategory])).ToList();
    }

    private void FadeText(Transform position)
    {
        Sequence sequence = DOTween.Sequence();

        transform.Find("Category Name").GetComponent<TextMeshProUGUI>().SetText(categories[SelectedCategory]);

        sequence.Append(transform.Find("Category Name").GetComponent<RectTransform>().DOLocalMoveX(position.GetComponent<RectTransform>().localPosition.x, animationTime));

        /*
        sequence.Join(transform.Find("Category Name").GetComponent<CanvasGroup>().DOFade(0f, animationTime / 2));
        sequence.Append(transform.Find("Category Name").GetComponent<CanvasGroup>().DOFade(1f, animationTime / 2));
        */
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        components = GetComponentsInChildren<CategoryHoverButton>().ToList();
    }

    private void Start()
    {
        SelectComponent(components[0], true);
    }

    #endregion
}

