using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public sealed class InventoryUserInterface : CategoryUserInterface
{
    #region Variables

    private Animator informationAnimator, arrowAnimator;
    private GameObject informationPanel;

    public List<ItemBehavior> itemButtons { get; set; } = new List<ItemBehavior>();

    #endregion

    #region Behavior Definitions

    public void Use()
    {
        informationPanel.transform.Find("Information (Horizontal)/Text").GetComponentInChildren<TextMeshProUGUI>().SetText("Select a Pokémon to use item on.");
        //StartCoroutine(ActivateSidePanel());
        PauseManager.instance.flags.isUsingItem = true;
        PauseManager.instance.flags.isGivingItem = false;
    }

    public void Give()
    {
        informationPanel.transform.Find("Information (Horizontal)/Text").GetComponentInChildren<TextMeshProUGUI>().SetText("Select a Pokémon to give item to.");
        //StartCoroutine(ActivateSidePanel());
        PauseManager.instance.flags.isGivingItem = true;
        PauseManager.instance.flags.isUsingItem = false;
    }

    public void Favorite(Item item)
    {
        item.IsFavorite = !item.IsFavorite;
        Color favoriteColor = item.IsFavorite ? "#EAC03E".ToColor() : "#FFFFFF".ToColor();
        StartCoroutine(GetComponentInChildren<ItemInformation>().buttons[itemButtons.IndexOf(itemButtons.Find(b => b.buttonName == "Favorite"))].Find("Big Icon/Icon").gameObject.FadeColor(favoriteColor, 0.1f));
        StartCoroutine(GetComponentInChildren<ItemInformation>().buttons[itemButtons.IndexOf(itemButtons.Find(b => b.buttonName == "Favorite"))].Find("Small Icon/Icon").gameObject.FadeColor(favoriteColor, 0.1f));
        //UpdateItem(InventoryController.instance.selectedSlot);
    }

    public void Discard(Item item)
    {
        Debug.Log($"Discarding item: {item}");
    }

    #endregion

    #region Miscellaneous Methods

    public override void UpdateSelectedObject(int selectedValue)
    {
        base.UpdateSelectedObject(selectedValue);
        //StartCoroutine(informationPanel.GetComponent<ItemInformation>().SetInformation(categoryItems.Count == 0 ? null : categoryItems[selectedItem]));
    }
    
    /*
    private IEnumerator ActivateSidePanel()
    {
        StartCoroutine(informationPanel.GetComponent<ItemInformation>().AnimateMenuButtons());
        yield return new WaitForSecondsRealtime(0.1f);
        informationAnimator.SetBool("isUsingItem", true);
        StartCoroutine(UpdateIndicator());
        //InventoryController.instance.ActiveSidePanel();
        StartCoroutine(FindObjectOfType<PauseUserInterface>().pauseContainer.transform.Find("Side Panel").gameObject.FadeOpacity(1f, 0.15f));
    }
    */

    /*
    private void ApplySortingMethod(Inventory inventory, InventoryController.SortingMethod sortingMethod)
    {
        switch (sortingMethod)
        {
            default: { break; }
            case (InventoryController.SortingMethod.AToZ):
                {
                    inventory.items.Sort((item1, item2) => string.Compare(item1.Name, item2.Name));
                    break;
                }
            case (InventoryController.SortingMethod.ZToA):
                {
                    inventory.items.Sort((item1, item2) => string.Compare(item2.Name, item1.Name));
                    break;
                }
            case (InventoryController.SortingMethod.AmountAscending):
                {
                    inventory.items.Sort((item1, item2) => item1.Quantity.CompareTo(item2.Quantity));
                    break;
                }
            case (InventoryController.SortingMethod.AmountDescending):
                {
                    inventory.items.Sort((item1, item2) => item2.Quantity.CompareTo(item1.Quantity));
                    break;
                }
            case (InventoryController.SortingMethod.FavoriteFirst):
                {
                    inventory.items.Sort((item1, item2) => item1.IsFavorite.CompareTo(item2.IsFavorite)); // Not working.
                    break;
                }
            case (InventoryController.SortingMethod.NewFirst):
                {
                    inventory.items.Sort((item1, item2) => item1.IsNew.CompareTo(item2.IsNew)); // Not working.
                    break;
                }
        }
    }
    */
    
    /*
    public void FadeUserInterface(float opacity, float duration = 0.2f, bool fadeSidePanel = false)
    {
        int indicatorActive = 1; //opacity == 1f ? InventoryController.instance.selectedSlot : -1;
        StartCoroutine(UpdateIndicator(indicatorActive));

        StartCoroutine(transform.Find("Middle").gameObject.FadeOpacity(opacity, duration));
        FindObjectOfType<PauseUserInterface>().FadeCharacterSprite(opacity, duration);

        if (fadeSidePanel)
        {
            FindObjectOfType<PauseUserInterface>().FadeSidePanel(opacity, duration);
        }
        else
        {
            StartCoroutine(informationPanel.FadeOpacity(opacity, duration));
        }
    }
    */

    /*
    public void CloseSubMenu(int selectedButton = -1)
    {
        if (selectedButton > -1)
        {
            itemButtons[selectedButton].behaviorEvent.Invoke();
        }
        else
        {
            StartCoroutine(AnimateItemSelection());
        }
    }
    */

    /*
    public IEnumerator UpdateIndicator(int selectedValue = -1, float duration = 0.1f, bool isInSubMenu = false)
    {
        indicatorAnimator.enabled = false;
        StartCoroutine(indicator.FadeOpacity(0f, duration));

        if (selectedValue > -1)
        {
            yield return new WaitForSecondsRealtime(duration);

            indicator.transform.position = !isInSubMenu ? itemGrid[selectedValue].Find("Information").position : informationPanel.GetComponent<ItemInformation>().buttons[selectedValue].position;
            indicator.GetComponent<RectTransform>().sizeDelta = !isInSubMenu ? itemGrid[selectedValue].GetComponent<RectTransform>().sizeDelta : informationPanel.GetComponent<ItemInformation>().buttons[selectedValue].GetComponent<RectTransform>().sizeDelta;
            yield return null;

            indicatorAnimator.enabled = true;
        }
    }
    */

    /*
    private IEnumerator UpdateCategoryItems(Inventory inventory, int selectedCategory, float duration = 0.15f, float delay = 0.03f)
    {
        #if DEBUG
                if (GameManager.Debug())
                {
                    Debug.Log("[INVENTORY MANAGER:] Updating category items.");
                }
        #endif

        //categoryItems = inventory.items.Where(item => item.Categorization.ToString().Equals(InventoryController.instance.categoryNames[selectedCategory])).ToList();

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
    */

    /*
    public void UpdateItem(int selectedSlot)
    {
        //itemGrid[selectedSlot].GetComponent<ItemSlot>().UpdateInformation(categoryItems[selectedSlot], -1, false);
    }
    */

    /*
    private void ResetCategoryItems()
    {
        #if DEBUG
                if (GameManager.Debug())
                {
                    Debug.Log("[INVENTORY MANAGER:] Reseting category items.");
                }
        #endif

        foreach (Transform slot in itemGrid)
        {
            slot.GetComponent<ItemSlot>().AnimateSlot(0f);
        }
    }
    */

    /*
    private IEnumerator UpdateCategoryName(int selectedCategory, float animationTime)
    {
        StartCoroutine(categoryText.transform.parent.gameObject.FadeOpacity(0f, animationTime));

        yield return new WaitForSecondsRealtime(animationTime);

        //categoryText.SetText(InventoryController.instance.categoryNames[selectedCategory].Replace('_', ' ')); yield return null;
        categoryText.transform.parent.position = new Vector2(categoryIcons[selectedCategory].position.x, categoryText.transform.parent.position.y);
        StartCoroutine(categoryText.transform.parent.gameObject.FadeOpacity(1f, animationTime));
    }
    */

    /*
    public void UpdateSelectedCategory(Inventory inventory, int selectedCategory, int increment)
    {
        StopAllCoroutines();
        AnimateCategory(selectedCategory, increment);
        StartCoroutine(UpdateCategoryName(selectedCategory, 0.1f));
        ResetCategoryItems(); 
        StartCoroutine(UpdateCategoryItems(inventory, selectedCategory, 0.15f, 0.03f));
        UpdateSelectedItem(0);
    }
    */

    /*
    public void UpdateSelectedButton(int selectedButton, int increment)
    {
        informationPanel.GetComponent<ItemInformation>().UpdateSelectedButton(selectedButton, increment);
    }
    */

    /*
    public void UpdateSortingMethod(Inventory inventory, InventoryController.SortingMethod sortingMethod, int selectedCategory)
    {
        AnimateSortingMethodText(sortingMethod);
        ApplySortingMethod(inventory, sortingMethod);
        ResetCategoryItems();
        StartCoroutine(UpdateCategoryItems(inventory, selectedCategory, 0.15f, 0.03f));
        UpdateSelectedItem(0);
    }
    */

    /*
    public IEnumerator AnimateItemSelection(int selectedSlot = -1, float duration = 0.2f)
    {
        if (selectedSlot > -1)
        {
            FadeUserInterface(0.3f, duration, true);

            informationAnimator.SetBool("Selected", true);
            yield return new WaitForSecondsRealtime(0.2f);

            StartCoroutine(informationPanel.GetComponent<ItemInformation>().AnimateMenuButtons(categoryItems[selectedSlot]));
            StartCoroutine(UpdateIndicator(0, 0.1f, true));
        }
        else
        {
            if (!informationAnimator.GetBool("isUsingItem"))
            {
                StartCoroutine(informationPanel.GetComponent<ItemInformation>().AnimateMenuButtons());
                yield return new WaitForSecondsRealtime(0.2f);

                informationAnimator.SetBool("Selected", false);

                FadeUserInterface(1f, duration, true);
            }
            else
            {
                informationAnimator.SetBool("isUsingItem", false);
                yield return new WaitForSecondsRealtime(0.1f);
                informationAnimator.SetBool("Selected", false);
            }

            //StartCoroutine(UpdateIndicator(InventoryController.instance.selectedSlot));
        }
    }
    */

    /*
    private void AnimateSortingMethodText(InventoryController.SortingMethod sortingMethod)
    {
        string value = string.Concat(sortingMethod.ToString().Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' '); // .FirstToUpper()
        StartCoroutine(FindObjectOfType<BottomPanelUserInterface>().AnimateValue(value, 1f));
    }
    */

    #endregion

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        informationPanel = transform.Find("Item Information").gameObject;
        selector = transform.Find("Indicator").gameObject;

        emptyGrid = transform.Find("Middle/Grid/Empty Grid").gameObject;
        informationAnimator = informationPanel.GetComponent<Animator>();
        //arrowAnimator = transform.Find("Categories/Navigation").GetComponent<Animator>();

        //category = transform.Find("Middle/Categories/Information/Name").GetComponent<TextMeshProUGUI>();
        categorizableSlots = transform.Find("Middle/Grid/Item Grid").GetComponentsInChildren<CategorizableSlot>().ToList();

        //UpdateSelectedCategory(InventoryController.instance.inventory, 0, 1);

        LayoutRebuilder.ForceRebuildLayoutImmediate(informationPanel.transform.Find("Information (Vertical)").GetComponent<RectTransform>());

        StartCoroutine(FindObjectOfType<BottomPanelUserInterface>().ChangePanelButtons(InventoryController.instance.buttons));

        base.Start();
    }

    #endregion
    
}
