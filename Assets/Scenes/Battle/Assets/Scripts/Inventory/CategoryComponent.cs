using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


//////////////////////////////////////
//
// TODO: Basically a complete redo
// This is a proof of concept!
//
//////////////////////////////////////


/// <summary>
///
/// </summary>
public class CategoryComponent : MonoBehaviour
{
    #region Variables

    [Header("Setup")]
    [SerializeField] private List<string> categories = new List<string>();
    [SerializeField] private Scrollbar scrollbar;

    [Header("Settings")]
    [SerializeField, Range(0.01f, 2f)] private float animationTime = 0.2f;

    private List<CategoryHoverButton> components;

    private int selectedCategory = -1;

    #endregion

    #region Miscellaneous Methods

    public void SelectComponent(CategoryHoverButton component, bool isSelected)
    {
        component.IsSelected = isSelected;

        component.GetComponentInChildren<Image>().DOColor(isSelected ? GameManager.instance.accentColor : Color.white, animationTime);

        if (component.GetComponentsInChildren<Image>().Length > 0)
        {
            foreach (Image image in component.GetComponentsInChildren<Image>())
            {
                image.DOColor(isSelected ? GameManager.instance.accentColor : Color.white, animationTime);
            }
        }

        if (isSelected && selectedCategory != components.IndexOf(component))
        {
            selectedCategory = components.IndexOf(component);

            FadeText(component.transform);
            SetInformation();
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

    public void SetInformation()
    {
        List<Item> currentCategory = GetComponentInParent<InventoryComponent>().Inventory.items.Where(i => i.Categorization.ToString().Replace("_", " ").Equals(categories[selectedCategory])).ToList();

        GetComponentInParent<InventoryComponent>().SetInformation(currentCategory);
    }

    private void FadeText(Transform position)
    {
        Sequence sequence = DOTween.Sequence();

        transform.Find("Category Name").GetComponent<TextMeshProUGUI>().SetText(categories[selectedCategory]);

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

        scrollbar.value = 1;
    }

    #endregion
}

