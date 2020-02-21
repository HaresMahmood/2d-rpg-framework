﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public abstract class CategoryUserInterface : UserInterface
{
    #region Constants

    public override int MaxObjects => categorizableSlots.Count;

    #endregion

    #region Variables

    public List<Categorizable> activeCategorizables { get; private set; } = new List<Categorizable>();

    protected List<CategorizableSlot> categorizableSlots;

    protected CategoryPanel categoryPanel;

    protected GameObject emptyGrid;

    private Coroutine categoryObjectsListRoutine;

    #endregion

    #region Miscellaneous Methods

    public void UpdateSelectedCategory(List<Categorizable> categorizables, int selectedCategory, int increment)
    {
        string value = categorizables[0].Categorization.GetCategoryFromIndex(selectedCategory);
        categoryPanel.AnimateCategory(selectedCategory, increment);
        StartCoroutine(categoryPanel.UpdateCategoryName(selectedCategory, value));
        ResetCategoryObjects();
        UpdateCategoryObjectsList(categorizables, value);
        UpdateSelectedObject(0);
    }

    public override void UpdateSelectedObject(int selectedValue)
    {
        UpdateSelector(categorizableSlots.Select(slot => slot.transform).ToArray(), selectedValue);
    }

    protected void UpdateCategoryObjectsList(List<Categorizable> categorizables, string value, float animationDuration = 0.15f, float animationDelay = 0.03f)
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
            int max = activeCategorizables.Count > MaxObjects ? MaxObjects : activeCategorizables.Count;

            ToggleEmptyPanel(false);

            StartCoroutine(ActiveSlots(0, max, animationDuration, animationDelay));

            if (max < categorizables.Count)
            {
                StartCoroutine(ActiveSlots(max, activeCategorizables.Count));
            }
        }
        else
        {
            ToggleEmptyPanel(true);
        }
    }

    protected virtual void ToggleEmptyPanel(bool isActive, float animationDuration = 0.15f)
    {
        float opacity = isActive ? 1f : 0f;

        if (selector.activeSelf == isActive)
        {
            selector.SetActive(!isActive);
            StartCoroutine(emptyGrid.FadeOpacity(opacity, animationDuration));
            opacity = isActive ? 0f : 1f;
            StartCoroutine(categorizableSlots[0].transform.parent.gameObject.FadeOpacity(opacity, animationDuration));
        }
    }

    protected IEnumerator ActiveSlots(int min, int max, float animationDuration = -1, float animationDelay = -1)
    {
        for (int i = min; i < max; i++)
        {
            if (i < activeCategorizables.Count)
            {
                ActiveSlots(i, animationDuration);

                if (animationDelay >= 0)
                {
                    yield return new WaitForSecondsRealtime(animationDelay);
                }
            }
        }
    }

    protected void ActiveSlots(int index, float animationDuration)
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
