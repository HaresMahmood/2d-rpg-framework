using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class ItemInformationUserInterface : CategorizableInformationUserInterface
{
    #region Constants

    public override int MaxObjects => buttons.Count;

    #endregion

    #region Variables

    private List<MenuButton> buttons = new List<MenuButton>();

    private Transform verticalPanel;
    private Transform horizontalPanel;

    private Animator informationAnimator;

    private TextMeshProUGUI nameText;
    private TextMeshProUGUI descriptionText;
    private TextMeshProUGUI valueText;
    private Image spriteImage;

    private TextMeshProUGUI effectTypeText;
    private TextMeshProUGUI effectQuantityText;
    private GameObject effectArrow;

    private RectTransform buttonPanel;

    #endregion

    #region Miscellaneous Methods

    public override void SetInformation(Categorizable categorizable)
    {
        Color textColor = ((Item)categorizable).Effect.GetQuantity().Equals("") ? "#B0B0B0".ToColor() : GameManager.GetAccentColor();
        bool arrowState = ((Item)categorizable).Effect.GetQuantity().Equals("") ? false : true;

        nameText.SetText(categorizable.Name);
        descriptionText.SetText(categorizable.Description);

        effectTypeText.SetText(((Item)categorizable).Effect.ToString());
        effectQuantityText.SetText(((Item)categorizable).Effect.GetQuantity());

        effectTypeText.GetComponent<AutoTextWidth>().UpdateWidth(((Item)categorizable).Effect.ToString());

        if (!effectTypeText.color.Equals(textColor))
        {
            effectTypeText.color = textColor;
        }

        if (!effectArrow.activeSelf.Equals(arrowState))
        {
            effectArrow.SetActive(arrowState);
        }
    }

    public void ToggleSubMenu(Categorizable categorizable, bool isActive)
    {
        if (isActive)
        {
            SetObjectDefinitionsFromPanel(horizontalPanel);
            SetInformation(categorizable);
            valueText.SetText(((Item)categorizable).Quantity.ToString());
            spriteImage.sprite = ((Item)categorizable).Sprite;
        }

        FindObjectOfType<ItemInformationController>().Flags.isActive = isActive;
        StartCoroutine(AnimateSubMenu(isActive));
    }

    public override void AnimatePanel(Categorizable categorizable, float animationDuration = 0.15f)
    {
        StartCoroutine(AnimatePanel(verticalPanel, categorizable, animationDuration));
    }

    public override void UpdateSelectedObject(int selectedValue, int increment = -1)
    {
        int previousValue = ExtensionMethods.IncrementInt(selectedValue, 0, MaxObjects, -increment);

        StartCoroutine(UpdateSelector(buttons[selectedValue].transform));
        buttons[selectedValue].AnimateButton(true);
        buttons[previousValue].AnimateButton(false);
    }

    protected override IEnumerator AnimatePanel(Transform panel, Categorizable categorizable = null, float animationDuration = 0.15F)
    {
        SetObjectDefinitionsFromPanel(panel);

        yield return null;

        StartCoroutine(base.AnimatePanel(panel, categorizable, animationDuration));
    }

    private IEnumerator AnimateSubMenu(bool isActive, float delay = 0.03f)
    {
        float opacity = isActive ? 1f : 0f;

        if (!isActive)
        {
            StartCoroutine(FadeButtons(opacity));
            yield return new WaitForSecondsRealtime(delay * buttons.Count);
        }

        informationAnimator.SetBool("isActive", isActive);
        yield return null; yield return new WaitForSecondsRealtime(informationAnimator.GetAnimationTime());

        if (isActive)
        {
            StartCoroutine(FadeButtons(opacity));
            UpdateSelectedObject(0);
        }
    }

    private IEnumerator FadeButtons(float opacity, float delay = 0.03f)
    {
        foreach (MenuButton button in buttons)
        {
            button.FadeButton(opacity);
            yield return new WaitForSecondsRealtime(delay);
        }
    }

    private void SetObjectDefinitionsFromPanel(Transform panel)
    {
        if (nameText != panel.Find("Name/Value").GetComponent<TextMeshProUGUI>())
        {
            nameText = panel.Find("Name/Value").GetComponent<TextMeshProUGUI>();
            descriptionText = panel.Find("Description/Value").GetComponent<TextMeshProUGUI>();

            effectTypeText = panel.Find("Effect/Type").GetComponent<TextMeshProUGUI>();
            effectQuantityText = panel.Find("Effect/Quantity").GetComponent<TextMeshProUGUI>();
            effectArrow = panel.Find("Effect/Arrow").gameObject;
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        informationAnimator = transform.GetComponent<Animator>();

        verticalPanel = transform.Find("Information (Vertical)");
        horizontalPanel = transform.Find("Information (Horizontal)");

        informationPanel = verticalPanel.transform;

        valueText = horizontalPanel.transform.Find("Amount/Value").GetComponent<TextMeshProUGUI>();
        spriteImage = horizontalPanel.transform.Find("Name/Icon").GetComponent<Image>();

        buttonPanel = horizontalPanel.transform.Find("Buttons").GetComponent<RectTransform>();
        buttons = buttonPanel.GetComponentsInChildren<MenuButton>().ToList();

        selector = buttonPanel.Find("Indicator").gameObject;

        base.Awake();

        StartCoroutine(UpdateSelector());
    }

    #endregion

    /*
    
    #region Variables

    private Animator informationAnimator;

    private GameObject informationPanel;

    public List<ItemBehavior> itemButtons { get; set; } = new List<ItemBehavior>();

    #endregion

    private void Awake()
    {
        informationPanel = transform.Find("Item Information").gameObject;
        selector = transform.Find("Indicator").gameObject;

        emptyGrid = transform.Find("Middle/Grid/Empty Grid").gameObject;
        informationAnimator = informationPanel.GetComponent<Animator>();

        categorizableSlots = transform.Find("Middle/Grid/Item Grid").GetComponentsInChildren<CategorizableSlot>().ToList();

        //LayoutRebuilder.ForceRebuildLayoutImmediate(informationPanel.transform.Find("Information (Vertical)").GetComponent<RectTransform>());

        StartCoroutine(FindObjectOfType<BottomPanelUserInterface>().ChangePanelButtons(InventoryController.instance.buttons));
    }  
     
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
        StartCoroutine(GetComponentInChildren<ItemInformationPanel>().buttons[itemButtons.IndexOf(itemButtons.Find(b => b.buttonName == "Favorite"))].Find("Big Icon/Icon").gameObject.FadeColor(favoriteColor, 0.1f));
        StartCoroutine(GetComponentInChildren<ItemInformationPanel>().buttons[itemButtons.IndexOf(itemButtons.Find(b => b.buttonName == "Favorite"))].Find("Small Icon/Icon").gameObject.FadeColor(favoriteColor, 0.1f));
        //UpdateItem(InventoryController.instance.selectedSlot);
    }

    public void Discard(Item item)
    {
        Debug.Log($"Discarding item: {item}");
    }

    #endregion

    /// <summary>
    /// Sets the desciption of the selected item.
    /// </summary>
    /// <param name="item"> Item currently selected. </param>
    /// <param name="duration"> Duration of the animation. Defaults to 0.1f. </param>
    /// <returns> Co-routine. </returns>
    public IEnumerator SetInformation(Item item = null, float duration = 0.1f)
    {
        if (item != null)
        {
            StartCoroutine(verticalInformation.FadeOpacity(0f, duration));

            yield return new WaitForSecondsRealtime(duration);

            nameText.SetText(item.Name);
            //descriptionText.SetText(item.description);

            StartCoroutine(verticalInformation.FadeOpacity(1f, duration));
        }
        else
        {
            StartCoroutine(verticalInformation.FadeOpacity(0f, duration));
        }
    }

    public void UpdateSelectedButton(int selectedButton, int increment)
    {
        int previousButton = ExtensionMethods.IncrementInt(selectedButton, 0, buttons.Length, increment);

        buttons[selectedButton].GetComponent<Animator>().SetBool("isSelected", true);
        buttons[previousButton].GetComponent<Animator>().SetBool("isSelected", false);
    }

    /// <summary>
    /// Sets correct information for and animates sub-menu buttons.
    /// </summary>
    /// <param name="item"> The selected item. </param>
    /// <param name="duration"> The duration of the animations. </param>
    /// <param name="delay"> The delay at which certain animations should occur. </param>
    /// <returns></returns>
    public IEnumerator AnimateMenuButtons(Item item = null, float duration = 0.2f, float delay = 0.03f)
    {
        if (item != null)
        {
            List<ItemBehavior> itemButtons = item.GenerateButtons(); 
            //this.itemButtons = itemButtons;

            UpdateSelectedButton(0, -1);

            for (int i = 0; i < itemButtons.Count; i++)
            {
                buttons[i].gameObject.SetActive(true);

                if (buttons[i].GetComponent<CanvasGroup>().alpha > 0) buttons[i].GetComponent<CanvasGroup>().alpha = 0;

                if (Array.IndexOf(buttons, buttons[i]) < itemButtons.Count)
                {
                    buttons[i].GetComponentInChildren<TextMeshProUGUI>().SetText(itemButtons[i].buttonName);
                    buttons[i].Find("Small Icon").GetComponentInChildren<Image>().sprite = itemButtons[i].iconSprite;
                    buttons[i].Find("Big Icon").GetComponentInChildren<Image>().sprite = itemButtons[i].iconSprite;

                    StartCoroutine(buttons[i].gameObject.FadeOpacity(1f, duration));

                    yield return new WaitForSecondsRealtime(delay);
                }
                else
                {
                    buttons[i].gameObject.SetActive(false);
                }
            }

            //Color favoriteColor = item.isFavorite ? "#EAC03E".ToColor() : Color.white;
            //StartCoroutine(buttons[itemButtons.IndexOf(itemButtons.Find(b => b.buttonName == "Favorite"))].Find("Big Icon/Icon").gameObject.FadeColor(favoriteColor, 0.1f));
            //StartCoroutine(buttons[itemButtons.IndexOf(itemButtons.Find(b => b.buttonName == "Favorite"))].Find("Small Icon/Icon").gameObject.FadeColor(favoriteColor, 0.1f));
        }
        else
        {
            //UpdateSelectedButton(InventoryController.instance.selectedButton, 0);

            for (int i = 0; (i < buttons.Length && buttons[i].GetComponent<CanvasGroup>().alpha == 1); i++)
            {
                StartCoroutine(buttons[i].gameObject.FadeOpacity(0f, duration));
                yield return new WaitForSecondsRealtime(delay);
            }

            //FindObjectOfType<InventoryUserInterface>().itemButtons.Clear();
        }
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
}
