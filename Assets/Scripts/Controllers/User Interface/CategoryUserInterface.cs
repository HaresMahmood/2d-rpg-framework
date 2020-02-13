using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class CategoryUserInterface : UserInterface
{
    #region Variables

    public List<Categorizable> categoryObjects { get; private set; } = new List<Categorizable>();

    private List<CategorizableSlot> objectSlots;

    private CategoryPanel categoryPanel;

    #endregion

    #region Miscellaneous Methods

    protected IEnumerator UpdateCategoryObjectsList(List<Categorizable> objects, string value, float animationDuration = 0.15f, float animationDelay = 0.03f)
    {
        #if DEBUG
        if (GameManager.Debug())
        {
            Debug.Log($"[{gameObject.name.ToUpper()}:] Updating category objects.");
        }
        #endif

        categoryItems = inventory.items.Where(item => item.category.ToString().Equals(value)).ToList();

        if (categoryItems.Count > 0)
        {
            int max = categoryItems.Count > 28 ? 28 : categoryItems.Count;

            StartCoroutine(transform.Find("Middle/Grid/Item Grid").gameObject.FadeOpacity(1f, duration));
            StartCoroutine(emptyGrid.FadeOpacity(0f, duration));
            indicator.SetActive(true);

            yield return new WaitForSecondsRealtime(duration);

            for (int i = 0; i < max; i++)
            {
                itemGrid[i].GetComponent<ItemSlot>().UpdateInformation(categoryItems[i], duration);

                yield return new WaitForSecondsRealtime(delay);
            }

            if (max < categoryItems.Count)
            {
                for (int i = max; i < categoryItems.Count; i++)
                {
                    itemGrid[i].GetComponent<ItemSlot>().UpdateInformation(categoryItems[i]);
                }
            }
        }
        else
        {
            //StartCoroutine(UpdateIndicator());
            indicator.SetActive(false);
            StartCoroutine(emptyGrid.FadeOpacity(1f, duration));
            StartCoroutine(transform.Find("Middle/Grid/Item Grid").gameObject.FadeOpacity(0f, duration));
        }
    }

    public void UpdateSelectedCategory(int selectedCategory, int increment)
    {
        string value = ((Categorizable.Category)selectedCategory).ToString();

        categoryPanel.AnimateCategory(selectedCategory, increment);
        StartCoroutine(categoryPanel.UpdateCategoryName(selectedCategory, value));
        ResetCategoryObjects();
        //StartCoroutine(UpdateCategoryItems(inventory, selectedCategory, 0.15f, 0.03f));
        UpdateSelectedObject(0);
    }

    private void ResetCategoryObjects()
    {
        #if DEBUG
        if (GameManager.Debug())
        {
            Debug.Log($"[{gameObject.name.ToUpper()}:] Reseting category objects.");
        }
        #endif

        foreach (CategorizableSlot slot in objectSlots)
        {
            //slot.AnimateSlot(0f);
        }

        categoryObjects.Clear();
    }

    #endregion
}
