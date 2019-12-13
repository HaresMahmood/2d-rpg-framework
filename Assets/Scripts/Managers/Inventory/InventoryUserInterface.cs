using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class InventoryUserInterface : MonoBehaviour
{
    #region Variables

    private Transform[] itemGrid, categoryIcons;
    private Animator informationAnimator, indicatorAnimator, arrowAnimator;
    private GameObject emptyGrid, informationPanel, indicator;
    private TextMeshProUGUI categoryText;

    public List<Item> categoryItems { get; private set; } = new List<Item>();

    private bool isEmpty;
    #endregion

    #region Miscellaneous Methods

    public IEnumerator UpdateIndicator(int selectedItem, float animationDuration)
    {
        indicatorAnimator.enabled = false;
        StartCoroutine(indicator.FadeOpacity(0f, animationDuration));
        yield return new WaitForSecondsRealtime(animationDuration);

        indicator.transform.position = itemGrid[selectedItem].Find("Slot").position;
        yield return null;

        indicatorAnimator.enabled = true;
    }

    public void UpdateCategoryItems(Inventory inventory, int selectedCategory, float animationTime)
    {
        int counter = 0;

        #if DEBUG
                if (GameManager.Debug())
                {
                    Debug.Log("[INVENTORY MANAGER:] Updating category items.");
                }
        #endif

        if (inventory.items.Count > 0)
        {
            foreach (Item item in inventory.items)
            {
                if (item.category.ToString().Equals(InventoryManager.instance.categoryNames[selectedCategory]) && item.isFavorite)
                {
                    counter = DrawItem(item, counter, true);
                    //yield return new WaitForSecondsRealtime(0.07f);
                }
            }

            for (int i = counter; i < inventory.items.Count; i++)
            {
                if (inventory.items[i].category.ToString().Equals(InventoryManager.instance.categoryNames[selectedCategory]) && !inventory.items[i].isFavorite)
                {
                    counter = DrawItem(inventory.items[i], counter);
                    //yield return new WaitForSecondsRealtime(0.07f);
                }
            }
        }

        if ((inventory.items.Count < 1 || categoryItems.Count < 1) && !isEmpty)
        {
            isEmpty = true;
            indicator.SetActive(false);
            StartCoroutine(emptyGrid.FadeOpacity(1f, 0.1f));
            StartCoroutine(transform.Find("Item Grid").gameObject.FadeOpacity(0f, animationTime));
        }
        else
        {
            isEmpty = false;
            StartCoroutine(transform.Find("Item Grid").gameObject.FadeOpacity(1f, animationTime));
            StartCoroutine(emptyGrid.FadeOpacity(0f, 0.1f));
            indicator.SetActive(true);
        }

        for (int i = counter; i < itemGrid.Length; i++)
        {
            //itemGrid[i].Find("Slot").gameObject.SetActive(false);
            StartCoroutine(itemGrid[i].Find("Slot").gameObject.FadeOpacity(0f, animationTime));
        }
    }

    private int DrawItem(Item item, int position, bool isFavorite = false)
    {
        Transform itemSlot = itemGrid[position].Find("Slot");

        itemSlot.gameObject.SetActive(true);
        itemSlot.Find("Sprite").GetComponent<Image>().sprite = item.sprite;
        itemSlot.Find("Amount").GetComponentInChildren<TextMeshProUGUI>().SetText(item.amount.ToString());
        itemSlot.Find("Favorite").gameObject.SetActive(isFavorite);
        itemSlot.Find("New").gameObject.SetActive(item.isNew);itemSlot.gameObject.SetActive(true);

        StartCoroutine(itemSlot.gameObject.FadeOpacity(1f, 0.15f));

        position++;

        categoryItems.Add(item);

        return position;
    }

    public void ResetCategoryItems()
    {
        #if DEBUG
                if (GameManager.Debug())
                {
                    Debug.Log("[INVENTORY MANAGER:] Reseting category items.");
                }
        #endif

        categoryItems.Clear();
    }

    public IEnumerator UpdateCategoryName(int selectedCategory, float animationTime)
    {
        StartCoroutine(categoryText.transform.parent.gameObject.FadeOpacity(0f, animationTime));
        yield return new WaitForSecondsRealtime(animationTime);
        categoryText.SetText(InventoryManager.instance.categoryNames[selectedCategory]);
        categoryText.transform.parent.position = new Vector2(categoryIcons[selectedCategory].position.x, categoryText.transform.parent.position.y);
        StartCoroutine(categoryText.transform.parent.gameObject.FadeOpacity(1f, animationTime));
    }

    public IEnumerator UpdateDescription(int selectedItem, float animationTime)
    {
        if (categoryItems.Count > 0)
        {
            StartCoroutine(informationPanel.transform.Find("Information (Vertical)").gameObject.FadeOpacity(0f, animationTime));

            yield return new WaitForSecondsRealtime(animationTime);

            informationPanel.transform.Find("Information (Vertical)/Name/Item Name").GetComponent<TextMeshProUGUI>().SetText(categoryItems[selectedItem].name);
            informationPanel.transform.Find("Information (Vertical)/Description/Item Description").GetComponent<TextMeshProUGUI>().SetText(categoryItems[selectedItem].description);

            StartCoroutine(informationPanel.transform.Find("Information (Vertical)").gameObject.FadeOpacity(1f, animationTime));
        }
        else
        {
            StartCoroutine(informationPanel.transform.Find("Information (Vertical)").gameObject.FadeOpacity(0f, animationTime));
        }
    }

    public void UpdateSelectedCategory(Inventory inventory, int selectedCategory, int increment)
    {
        AnimateCategoryIcons(selectedCategory, -increment);
        ResetCategoryItems();
        UpdateCategoryItems(inventory, selectedCategory, 0.15f);
        StartCoroutine(UpdateCategoryName(selectedCategory, 0.1f));
        StartCoroutine(AnimateArrows(increment));
        UpdateSelectedItem(0);
    }

    public void UpdateSelectedItem(int selectedItem)
    {
        StartCoroutine(UpdateIndicator(selectedItem, 0.1f));
        StartCoroutine(UpdateDescription(selectedItem, 0.07f));
    }

    public void AnimateCategoryIcons(int selectedCategory, int increment)
    {
        int previousCategory = ExtensionMethods.IncrementInt(selectedCategory, 0, categoryIcons.Length, increment);

        categoryIcons[selectedCategory].GetComponent<Animator>().SetBool("isSelected", true);
        StartCoroutine(categoryIcons[selectedCategory].GetComponentInChildren<Image>().gameObject.FadeColor(GameManager.GetAccentColor(), 0.1f));
        categoryIcons[previousCategory].GetComponent<Animator>().SetBool("isSelected", false);
        StartCoroutine(categoryIcons[previousCategory].GetComponentInChildren<Image>().gameObject.FadeColor(Color.white, 0.1f));
    }

    public IEnumerator AnimateArrows(int increment)
    {
        arrowAnimator.SetBool("isActive", true);
        arrowAnimator.SetFloat("Blend", increment);

        yield return null; float waitTime = arrowAnimator.GetAnimationTime();
        yield return new WaitForSecondsRealtime(waitTime);

        //yield return new WaitForSecondsRealtime(0.1f);

        arrowAnimator.SetFloat("Blend", 0);
        arrowAnimator.SetBool("isActive", false);
    }

    public void AnimateItemSelection(int selectedItem = -1)
    {
        // Fade side-panel, target sprite and rest of inventory.

        informationAnimator.SetBool("Selected", true);
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        informationPanel = transform.Find("Item Information").gameObject;
        indicator = transform.Find("Indicator").gameObject;

        emptyGrid = transform.Find("Empty Grid").gameObject;
        informationAnimator = informationPanel.GetComponent<Animator>();
        indicatorAnimator = indicator.GetComponent<Animator>();
        arrowAnimator = transform.Find("Categories/Navigation").GetComponent<Animator>();

        categoryText = transform.Find("Categories/Information/Name").GetComponent<TextMeshProUGUI>();
        itemGrid = transform.Find("Item Grid").GetChildren();
        categoryIcons = transform.Find("Categories/Category Icons").GetChildren();

        UpdateSelectedCategory(InventoryManager.instance.inventory, 0, -1);

        LayoutRebuilder.ForceRebuildLayoutImmediate(informationPanel.transform.Find("Information (Vertical)").GetComponent<RectTransform>());
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        
    }

    #endregion
    
}
