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

    public void UpdateIndicator(int selectedItem)
    {
        indicator.transform.position = itemGrid[selectedItem].Find("Slot").position;
    }

    public void UpdateCategoryItems(Inventory inventory, int selectedCategory)
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
            StartCoroutine(transform.Find("Item Grid").gameObject.FadeOpacity(0f, 0.1f));
        }
        else
        {
            isEmpty = false;
            StartCoroutine(transform.Find("Item Grid").gameObject.FadeOpacity(1f, 0.1f));
            StartCoroutine(emptyGrid.FadeOpacity(0f, 0.1f));
            indicator.SetActive(true);
        }

        for (int i = counter; i < itemGrid.Length; i++)
        {
            //itemGrid[i].Find("Slot").gameObject.SetActive(false);
            StartCoroutine(itemGrid[i].Find("Slot").gameObject.FadeOpacity(0f, 0.1f));
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

        StartCoroutine(itemSlot.gameObject.FadeOpacity(1f, 0.1f));

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

    public void UpdateCategoryName(int selectedCategory)
    {
        categoryText.SetText(InventoryManager.instance.categoryNames[selectedCategory]);
    }

    public void UpdateDescription(int selectedItem)
    {
        if (categoryItems.Count > 0)
        {
            informationPanel.SetActive(true);
            informationPanel.transform.Find("Information (Vertical)/Name/Item Name").GetComponent<TextMeshProUGUI>().SetText(categoryItems[selectedItem].name);
            informationPanel.transform.Find("Information (Vertical)/Description/Item Description").GetComponent<TextMeshProUGUI>().SetText(categoryItems[selectedItem].description);
        }
        /*
        else if (item == null)
        {
            informationPanel.SetActive(false);
        }
        */
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
