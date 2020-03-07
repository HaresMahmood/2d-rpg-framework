using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public abstract class CategoryUserInterface : UserInterface
{
    #region Constants

    public override int MaxObjects => activeCategorizables.Count;

    #endregion

    #region Variables

    protected List<Categorizable> activeCategorizables = new List<Categorizable>();

    protected List<CategorizableSlot> categorizableSlots;

    protected CategoryPanel categoryPanel;

    protected GameObject categorizablePanel;

    protected GameObject emptyPanel;

    protected CategorizableInformationUserInterface informationPanel;

    protected int selectedValue;

    private int selectedCategory = -1;

    #endregion

    #region Miscellaneous Methods

    public void UpdateSelectedCategory(List<Categorizable> categorizables, int selectedCategory, int selectedValue, int increment, int maxViewableObjects)
    {
        if (this.selectedCategory != selectedCategory)
        {
            string value = categorizables[0].Categorization.GetCategoryFromIndex(selectedCategory);

            this.selectedCategory = selectedCategory;

            categoryPanel.AnimateCategory(selectedCategory, increment);
            StartCoroutine(categoryPanel.UpdateCategoryName(selectedCategory, value));
            ResetCategoryObjects();
            UpdateCategoryObjectsList(categorizables, value, maxViewableObjects);
        }
        else
        {
            ActiveSlot(selectedValue, -1);
        }

        UpdateSelectedObject(selectedValue);
    }

    public override void UpdateSelectedObject(int selectedValue, int increment = -1)
    {
        Categorizable selectedCategorizable = activeCategorizables.Count > 0 ? activeCategorizables[selectedValue] : null;

        StartCoroutine(UpdateSelector(categorizableSlots[selectedValue].transform));

        //informationPanel.AnimatePanel(selectedCategorizable);

        this.selectedValue = selectedValue;
    }

    protected virtual void UpdateCategoryObjectsList(List<Categorizable> categorizables, string value, int maxViewableObjects, float animationDuration = 0.15f, float animationDelay = 0.02f)
    {
        #if DEBUG
        if (GameManager.Debug())
        {
            Debug.Log($"[{gameObject.name.ToUpper()}:] Updating category objects.");
        }
        #endif

        activeCategorizables = categorizables.Where(categorizable => categorizable.Categorization.ToString().Equals(value)).ToList();

        if (activeCategorizables.Count > 0)
        {
            int max = activeCategorizables.Count > maxViewableObjects ? maxViewableObjects : activeCategorizables.Count;

            if (activeCategorizables.Count < maxViewableObjects)
            {
                UpdateScrollbar();
            }
            else
            {
                UpdateScrollbar(MaxObjects);
            }

            ToggleEmptyPanel(true);

            StartCoroutine(ActiveSlots(0, max, animationDuration, animationDelay));

            if (activeCategorizables.Count > maxViewableObjects)
            {
                StartCoroutine(ActiveSlots(max, activeCategorizables.Count));
            }
        }
        else
        {
            UpdateScrollbar();
            ToggleEmptyPanel(false);
        }
    }

    protected virtual void ToggleEmptyPanel(bool isActive, float animationDuration = 0.15f)
    {
        float opacity = isActive ? 0f : 1f;
        Transform selectedObject = isActive ? categorizableSlots[0].transform : null;

        selector.SetActive(isActive);
        StartCoroutine(emptyPanel.FadeOpacity(opacity, animationDuration));

        opacity = isActive ? 1f : 0f;
        StartCoroutine(categorizablePanel.FadeOpacity(opacity, animationDuration));
    }

    protected IEnumerator ActiveSlots(int min, int max, float animationDuration = -1, float animationDelay = -1)
    {
        for (int i = min; i < max; i++)
        {
            if (i < activeCategorizables.Count)
            {
                ActiveSlot(i, animationDuration);

                if (animationDelay >= 0)
                {
                    yield return new WaitForSecondsRealtime(animationDelay);
                }
            }
        }
    }

    protected virtual void ActiveSlot(int index, float animationDuration)
    {
        categorizableSlots[index].UpdateInformation(activeCategorizables[index], animationDuration);
    }

    /*
    protected IEnumerator AnimateArrows(int increment)
    {
        arrowAnimator.SetBool("isActive", true);
        arrowAnimator.SetFloat("Blend", increment);

        yield return null; float waitTime = arrowAnimator.GetAnimationTime();
        yield return new WaitForSecondsRealtime(waitTime);

        //yield return new WaitForSecondsRealtime(0.1f);

        arrowAnimator.SetFloat("Blend", 0);
        arrowAnimator.SetBool("isActive", false);
    }
    */

    private void ResetCategoryObjects()
    {
        #if DEBUG
        if (GameManager.Debug())
        {
            Debug.Log($"[{gameObject.name.ToUpper()}:] Reseting category objects.");
        }
        #endif

        foreach (CategorizableSlot slot in categorizableSlots)
        {
            slot.AnimateSlot(0f);
        }

        activeCategorizables.Clear();
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        categoryPanel = GetComponentInChildren<CategoryPanel>();

        base.Awake();
    }

    #endregion
}
