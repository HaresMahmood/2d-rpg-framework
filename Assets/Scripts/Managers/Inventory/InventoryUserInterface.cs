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

    public void UpdateInventory(Inventory inventory, int selectedCategory)
    {
        int counter = 0;

        #if DEBUG
                if (GameManager.Debug())
                {
                    Debug.Log("[INVENTORY MANAGER:] Updating inventory.");
                }
        #endif


        ResetInventory();

        categoryText.SetText(InventoryManager.instance.categoryNames[selectedCategory]);

        if (inventory.items.Count > 0)
        {
            if (isEmpty)
            {
                isEmpty = false;
                StartCoroutine(transform.Find("Empty Grid").gameObject.FadeOpacity(1f, 0.1f));
                StartCoroutine(emptyGrid.FadeOpacity(0f, 0.1f));
                indicator.SetActive(true);
            }

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
            StartCoroutine(transform.Find("Empty Grid").gameObject.FadeOpacity(0f, 0.1f));
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

    public void ResetInventory()
    {
        #if DEBUG
                if (GameManager.Debug())
                {
                    Debug.Log("[INVENTORY MANAGER:] Reseting inventory.");
                }
        #endif

        categoryItems.Clear();
    }

    private void SetDescription(Item item = null)
    {
        if (categoryItems.Count > 0)
        {
            informationPanel.SetActive(true);
            informationPanel.transform.Find("Information (Vertical)/Name/Item Name").GetComponent<TextMeshProUGUI>().SetText(item.name);
            informationPanel.transform.Find("Information (Vertical)/Description/Item Description").GetComponent<TextMeshProUGUI>().SetText(item.description);
        }
        else if (item == null)
        {
            informationPanel.SetActive(true);
        }
    }

    public void AnimateCategoryIcons(int selectedCategory, int increment)
    {
        int previousCategory = ExtensionMethods.IncrementInt(selectedCategory, 0, categoryIcons.Length, increment);

        categoryIcons[selectedCategory].GetComponent<Animator>().SetBool("isSelected", true);
        StartCoroutine(categoryIcons[selectedCategory].GetComponentInChildren<Image>().gameObject.FadeColor(GameManager.GetAccentColor(), 0.1f));
        categoryIcons[previousCategory].GetComponent<Animator>().SetBool("isSelected", false);
        StartCoroutine(categoryIcons[previousCategory].GetComponentInChildren<Image>().gameObject.FadeColor(Color.white, 0.1f));
    }

    private IEnumerator AnimateArrows(Animator anim, int value)
    {
        anim.SetBool("isActive", true);
        anim.SetFloat("Blend", value);
        yield return new WaitForSecondsRealtime(0.1f);
        anim.SetFloat("Blend", 0);
        anim.SetBool("isActive", false);
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
