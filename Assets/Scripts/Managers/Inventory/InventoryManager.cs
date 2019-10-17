using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class InventoryManager : MonoBehaviour
{
    #region Variables

    public static InventoryManager instance;

    [UnityEngine.Header("Setup")]
    public Inventory inventory;

    [UnityEngine.Header("Settings")]
    [SerializeField] private GameObject menuButtonPrefab;

    private GameObject inventoryContainer, itemIndicator, menuPanel;
    private GameObject[] menuButtons;
    private Transform[] grid, categoryContainer;
    [HideInInspector] public Animator categoryAnim, indicatorAnim, rightAnim, leftAnim;
    private TextMeshProUGUI categoryText;
    private Item currentItem;
    private List<Item> currentCategoryItems;

    private string[] categories = new string[] { "Key", "Health", "PokéBall", "Battle", "TM", "Berry", "Other" };
    private string currentCategory;

    private int currentCategoryIndex = 0;
    private int counter;
    [HideInInspector] public int itemIndex, maxItemIndex, selectedItem = 0;
    private int buttonIndex, maxButtonIndex, selectedButton = 0;

    private bool isInventoryDrawn, isInteracting = false, isInMenu = false;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        inventoryContainer = PauseManager.instance.pauseContainer.transform.Find("Inventory").gameObject;
        itemIndicator = inventoryContainer.transform.Find("Indicator").gameObject;
        menuPanel = inventoryContainer.transform.Find("Menu").gameObject;

        grid = inventoryContainer.transform.Find("Item Grid").transform.GetChildren();
        categoryContainer = inventoryContainer.transform.Find("Categories/Category Icons").GetChildren();

        categoryAnim = inventoryContainer.transform.Find("Categories/Category Icons").GetComponent<Animator>();
        rightAnim = inventoryContainer.transform.Find("Categories/Navigation/Right").GetComponent<Animator>();
        leftAnim = inventoryContainer.transform.Find("Categories/Navigation/Left").GetComponent<Animator>();
        indicatorAnim = itemIndicator.GetComponent<Animator>();

        categoryText = inventoryContainer.transform.Find("Categories/Information/Name").GetComponent<TextMeshProUGUI>();
        
        currentCategoryItems = new List<Item>();

        currentCategory = categories[0];

        inventoryContainer.SetActive(false);
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        OnPause();
    }

    #endregion
    private void OnPause()
    {
        if (PauseManager.instance.isPaused)
        {            
            DrawInventory();
            AnimateCategory();
            CheckForInput();

            if (isInMenu && menuButtons.Length > 0)
            {
                menuPanel.transform.Find("Indicator").position = menuButtons[buttonIndex].transform.position;
            }

            if (currentCategoryItems.Count > 0)
            {
                currentItem = currentCategoryItems[selectedItem];
            }

            itemIndicator.transform.position = grid[selectedItem].position;
            inventoryContainer.SetActive(true);
            currentCategory = categories[currentCategoryIndex];

            if (currentCategoryItems.Count > 0)
            {
                Transform itemInfo = inventoryContainer.transform.Find("Item Information");
                itemInfo.gameObject.SetActive(true);
                itemInfo.Find("Name/Item Name").GetComponent<TextMeshProUGUI>().SetText(currentItem.name);
                itemInfo.Find("Description/Item Description").GetComponent<TextMeshProUGUI>().SetText(currentItem.description);
            }
            else if (currentCategoryItems.Count == 0 || currentItem == null)
            {
                inventoryContainer.transform.Find("Item Information").gameObject.SetActive(false);
            }
        }
        else
        {
            inventoryContainer.SetActive(false);
        }
    }

    private void DrawInventory()
    {
        if (!isInventoryDrawn)
        {
            categoryText.SetText(currentCategory);

            if (inventory.items.Count > 0)
            {
                counter = 0;
                foreach (Item item in inventory.items)
                {
                    if (item.category.ToString().Equals(currentCategory))
                    {
                        Transform itemSlot = grid[counter];
                        itemSlot.GetComponent<ItemSelection>().slotIndex = counter;

                        itemSlot.Find("Sprite").GetComponent<Image>().sprite = item.sprite;
                        itemSlot.Find("Amount").GetComponentInChildren<TextMeshProUGUI>().SetText(item.amount.ToString());
                        itemSlot.Find("Sprite").gameObject.SetActive(true);
                        itemSlot.Find("Amount").gameObject.SetActive(true);

                        counter++;

                        currentCategoryItems.Add(item);
                    }
                }

                maxItemIndex = counter - 1;

                isInventoryDrawn = true;
            }

            for (int i = counter; i < grid.Length; i++)
            {
                Transform itemSlot = grid[i];
                itemSlot.Find("Sprite").gameObject.SetActive(false);
                itemSlot.Find("Amount").gameObject.SetActive(false);
            }
        }
    }

    private void CheckForInput()
    {
        if (!isInMenu)
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                if (!isInteracting)
                {
                    if (Input.GetAxisRaw("Horizontal") > 0)
                    {
                        if (itemIndex < maxItemIndex)
                            itemIndex++;
                        else
                            itemIndex = 0;
                    }
                    else if (Input.GetAxisRaw("Horizontal") < 0)
                    {
                        if (itemIndex > 0)
                            itemIndex--;
                        else
                            itemIndex = maxItemIndex;
                    }
                    isInteracting = true;
                }
            }
            else if (Input.GetAxisRaw("Vertical") != 0)
            {
                if (!isInteracting)
                {
                    if (Input.GetAxisRaw("Vertical") > 0)
                    {
                        if ((itemIndex - 6) > 0)
                            itemIndex -= 6;
                        else
                            itemIndex = 0;
                    }
                    else if (Input.GetAxisRaw("Vertical") < 0)
                    {
                        if ((itemIndex + 6) < maxItemIndex)
                            itemIndex += 6;
                        else
                            itemIndex = maxItemIndex;
                    }
                    isInteracting = true;
                }
            }
            else if (Input.GetAxisRaw("Trigger") != 0)
            {
                if (!isInteracting)
                {
                    if (Input.GetAxisRaw("Trigger") > 0)
                    {
                        AnimateArrows(rightAnim);
                        if (currentCategoryIndex < (categories.Length - 1))
                        {
                            currentCategory = categories[Array.IndexOf(categories, currentCategory) + 1];
                            currentCategoryIndex++;
                        }
                        else
                        {
                            currentCategory = categories[0];
                            currentCategoryIndex = 0;
                        }
                    }
                    else if (Input.GetAxisRaw("Trigger") < 0)
                    {
                        AnimateArrows(leftAnim);
                        if (currentCategoryIndex > 0)
                        {
                            currentCategory = categories[Array.IndexOf(categories, currentCategory) - 1];
                            currentCategoryIndex--;
                        }
                        else
                        {
                            currentCategory = categories[categories.Length - 1];
                            currentCategoryIndex = categories.Length - 1;
                        }
                    }
                    isInteracting = true;
                    counter = 0; itemIndex = 0;
                    currentCategoryItems.Clear();
                    isInventoryDrawn = false;
                }
            }
            else
            {
                isInteracting = false;
                leftAnim.Rebind(); rightAnim.Rebind();
            }
        }
        else if (isInMenu)
        {
            if (Input.GetAxisRaw("Vertical") != 0)
            {
                if (!isInteracting)
                {
                    if (Input.GetAxisRaw("Vertical") < 0)
                    {
                        if (buttonIndex < maxButtonIndex)
                            buttonIndex++;
                        else
                            buttonIndex = 0;
                    }
                    else if (Input.GetAxisRaw("Vertical") > 0)
                    {
                        if (buttonIndex > 0)
                            buttonIndex--;
                        else
                            buttonIndex = maxButtonIndex;
                    }
                    isInteracting = true;
                }
            }
            else
                isInteracting = false;
        }

        if (Input.GetButtonDown("Interact"))
        {
            if (!isInMenu)
            {
                StartCoroutine(CreateMenu(indicatorAnim));
            }
        }
        else if (Input.GetButtonUp("Interact") && !isInMenu)
        {
            indicatorAnim.Rebind();
        }

        if (Input.GetButtonDown("Interact") && isInMenu)
        {
            DestroyMenu();
        }
    }

    private void AnimateCategory()
    {
        foreach (Transform category in categoryContainer)
        {
            if (category != categoryContainer[currentCategoryIndex])
            {
                categoryAnim.SetBool(category.name, false);
            }
            else
            {
                categoryAnim.SetBool(category.name, true);
            }
        }
    }

    private void AnimateArrows(Animator anim)
    {
        anim.ResetTrigger("isActive");
        anim.SetTrigger("isActive");
    }

    private IEnumerator CreateMenu(Animator anim)
    {
        anim.ResetTrigger("isPressed");
        anim.SetTrigger("isPressed");
        float waitTime = anim.GetAnimationTime();
        yield return null;

        for (int i = 0; i <= maxItemIndex; i++)
        {
            if (grid[i] != grid[selectedItem])
            {
                foreach (Transform child in grid[i].GetChildren())
                {
                    if (!child.GetComponent<Image>())
                    {
                        foreach (Transform grandChild in child.GetChildren())
                            StartCoroutine(grandChild.gameObject.FadeObject(0.5f, 0.1f));
                    }
                    else
                        StartCoroutine(child.gameObject.FadeObject(0.5f, 0.1f));
                }
            }
        }
        
        StartCoroutine(CreateMenuButtons(currentItem));
        menuPanel.transform.Find("Base").position = grid[selectedItem].position;
        menuPanel.SetActive(true);

        isInMenu = true;
    }

    public void DestroyMenu()
    {
        menuPanel.SetActive(false);

        for (int i = 0; i <= maxItemIndex; i++)
        {
            if (grid[i] != grid[selectedItem])
            {
                foreach (Transform child in grid[i].GetChildren())
                {
                    if (!child.GetComponent<Image>())
                    {
                        foreach (Transform grandChild in child.GetChildren())
                        {
                            StartCoroutine(grandChild.gameObject.FadeObject(1f, 0.1f));
                        }
                    }
                    else
                    {
                        StartCoroutine(child.gameObject.FadeObject(1f, 0.1f));
                    }
                }
            }
        }

        DestroyMenuButtons();

        isInMenu = false;
    }

    public IEnumerator CreateMenuButtons(Item item)
    {
        item = InventoryManager.instance.inventory.items.Find(i => i.id == item.id);
        menuButtons = new GameObject[item.action.behaviorData.Count];

        for (int i = 0; i < menuButtons.Length; i++)
        {
            GameObject menuButtonObj = (GameObject)Instantiate(menuButtonPrefab, Vector3.zero, Quaternion.identity); // Instantiates/creates new choice button from prefab in scene.

            menuButtonObj.name = "Menu Button " + (i + 1); // Gives appropriate name to newly instantiated choice button.
            menuButtonObj.transform.SetParent(menuPanel.transform.Find("Base"), false);
            menuButtonObj.GetComponentInChildren<TextMeshProUGUI>().text = item.action.behaviorData[i].menuOption;

            if (menuButtonObj.GetComponentInChildren<TextMeshProUGUI>().text.Equals("Cancel"))  //TODO: Should not exist here!
            {
                menuButtonObj.transform.Find("Option Text/Button").gameObject.SetActive(true);
            }

            ItemEventHandler eventHandler = menuButtonObj.GetComponent<ItemEventHandler>();
            eventHandler.eventHandler = item.action.behaviorData[i].behaviorEvent;

            menuButtons[i] = menuButtonObj;
        }

        maxButtonIndex = menuButtons.Length - 1;

        yield return null;

        selectedButton = 0;
    }

    private void DestroyMenuButtons()
    {
        for (int i = 0; i < menuButtons.Length; i++)
            Destroy(menuButtons[i]);

        menuButtons = null;
    }

}
