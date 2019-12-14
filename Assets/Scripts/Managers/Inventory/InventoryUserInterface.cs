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

    public IEnumerator UpdateCategoryItems(Inventory inventory, int selectedCategory, float animationTime, float delay)
    {
        #if DEBUG
                if (GameManager.Debug())
                {
                    Debug.Log("[INVENTORY MANAGER:] Updating category items.");
                }
        #endif

        // Debug

        foreach (Item item in inventory.items)
        {
            if (item.category.ToString().Equals(InventoryManager.instance.categoryNames[selectedCategory]))
            {
                categoryItems.Add(item);
            }
        }

        if (categoryItems.Count > 0)
        {
            int max = categoryItems.Count > 28 ? 28 : categoryItems.Count;

            StartCoroutine(transform.Find("Item Grid").gameObject.FadeOpacity(1f, animationTime));
            StartCoroutine(emptyGrid.FadeOpacity(0f, animationTime));
            indicator.SetActive(true);

            yield return new WaitForSecondsRealtime(animationTime);

            for (int i = 0; i < max; i++)
            {
                StartCoroutine(itemGrid[i].Find("Slot").gameObject.FadeOpacity(0f, animationTime));
                DrawItem(categoryItems[i], i, animationTime);
                yield return new WaitForSecondsRealtime(delay);
            }

            if (max < categoryItems.Count)
            {
                for (int i = max; i < categoryItems.Count; i++)
                {
                    DrawItem(categoryItems[i], i);
                }
            }
        }
        else
        {
            indicator.SetActive(false);
            StartCoroutine(emptyGrid.FadeOpacity(1f, animationTime));
            StartCoroutine(transform.Find("Item Grid").gameObject.FadeOpacity(0f, animationTime));
        }
    }

    private int DrawItem(Item item, int position, float animationTime = -1, bool isFavorite = false)
    {
        Transform itemSlot = itemGrid[position].Find("Slot");

        itemSlot.gameObject.SetActive(true);
        itemSlot.Find("Sprite").GetComponent<Image>().sprite = item.sprite;
        itemSlot.Find("Amount").GetComponentInChildren<TextMeshProUGUI>().SetText(item.amount.ToString());
        itemSlot.Find("Favorite").gameObject.SetActive(isFavorite);
        itemSlot.Find("New").gameObject.SetActive(item.isNew);itemSlot.gameObject.SetActive(true);

        if (animationTime > -1)
        {
            StartCoroutine(itemSlot.gameObject.FadeOpacity(1f, animationTime));
        }
        else
        {
            itemSlot.GetComponent<CanvasGroup>().alpha = 1f;
        }

        position++;

        //categoryItems.Add(item);

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

        foreach (Item item in categoryItems)
        {
            itemGrid[categoryItems.IndexOf(item)].Find("Slot").GetComponent<CanvasGroup>().alpha = 0f;
        }

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
        StopAllCoroutines();
        AnimateCategory(selectedCategory, increment);
        StartCoroutine(UpdateCategoryName(selectedCategory, 0.1f));
        ResetCategoryItems(); 
        StartCoroutine(UpdateCategoryItems(inventory, selectedCategory, 0.15f, 0.03f));
        UpdateSelectedItem(0);
    }

    public void UpdateSelectedItem(int selectedItem)
    {
        StartCoroutine(UpdateIndicator(selectedItem, 0.1f));
        StartCoroutine(UpdateDescription(selectedItem, 0.07f));
    }

    public void AnimateCategoryPosition(int selectedCategory, int increment)
    {
        int previousCategory = ExtensionMethods.IncrementInt(selectedCategory, 0, categoryIcons.Length, increment);

        categoryIcons[selectedCategory].GetComponent<Animator>().SetBool("isSelected", true);
        StartCoroutine(categoryIcons[selectedCategory].GetComponentInChildren<Image>().gameObject.FadeColor(GameManager.GetAccentColor(), 0.1f));
        categoryIcons[previousCategory].GetComponent<Animator>().SetBool("isSelected", false);
        StartCoroutine(categoryIcons[previousCategory].GetComponentInChildren<Image>().gameObject.FadeColor(Color.white, 0.1f));
    }

    private IEnumerator AnimateCategoryIcon(int selectedCategory, int increment)
    {
        int previousCategory = ExtensionMethods.IncrementInt(selectedCategory, 0, categoryIcons.Length, increment);
        Animator selectedAnimator = categoryIcons[selectedCategory].Find("Icon").GetComponent<Animator>();
        Animator previousAnimator = categoryIcons[previousCategory].Find("Icon").GetComponent<Animator>();

        if (selectedAnimator != null) // Debug.
        {
            if (previousAnimator != null && previousAnimator.GetBool("isSelected"))
            {
                previousAnimator.SetBool("isSelected", false);
            }

            selectedAnimator.SetBool("isSelected", true);
            yield return null; float animationTime = selectedAnimator.GetAnimationTime();
            yield return new WaitForSecondsRealtime(animationTime);
            selectedAnimator.SetBool("isSelected", false);
        }
    }

    private void AnimateCategory(int selectedCategory, int increment)
    {
        AnimateCategoryPosition(selectedCategory, -increment);
        StartCoroutine(AnimateCategoryIcon(selectedCategory, -increment));
        //StartCoroutine(AnimateArrows(increment));
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
        //arrowAnimator = transform.Find("Categories/Navigation").GetComponent<Animator>();

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
