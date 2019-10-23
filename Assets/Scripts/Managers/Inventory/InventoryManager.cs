using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

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

    [HideInInspector] public GameObject inventoryContainer;
    private GameObject itemIndicator, menuPanel;
    private GameObject[] menuButtons;
    private Transform[] grid, categoryContainer;
    [HideInInspector] public Animator categoryAnim, indicatorAnim, rightAnim, leftAnim;
    private TextMeshProUGUI categoryText;
    [HideInInspector] public Item currentItem;
    private List<Item> currentCategoryItems;

    private string[] categories = new string[] { "Key", "Health", "PokéBall", "Battle", "TM", "Berry", "Other" };
    private string currentCategory;

    private int currentCategoryIndex = 0;
    private int counter;
    [HideInInspector] public int itemIndex, maxItemIndex, selectedItem = 0;
    private int buttonIndex, maxButtonIndex, selectedButton = 0;

    [HideInInspector] public bool inMenu = false, givingItem = false;
    private bool isInventoryDrawn, isInteracting = false;

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

        if (Input.GetButtonDown("Interact") && givingItem)
        {
            if (GameManager.instance.party.playerParty[PauseManager.instance.slotIndex].heldItem != currentItem)
            {
                GameManager.instance.party.playerParty[PauseManager.instance.slotIndex].heldItem = currentItem;
                EditorUtility.SetDirty(GameManager.instance.party.playerParty[PauseManager.instance.slotIndex]); //TODO: Debug

                if (currentItem.amount > 1)
                    currentItem.amount--;
                else
                    inventory.items.Remove(currentItem);

                UpdateInventory();
            }

            StartCoroutine(inventoryContainer.FadeObject(1f, 0.1f));
            PauseManager.instance.inPartyMenu = false;
            givingItem = false;
        }
    }

    #endregion
    private void OnPause()
    {
        if (PauseManager.instance.isPaused)
        {
            if (isInventoryDrawn)
            {
                UpdateInventory();
            }
            else
            {
                DrawInventory();
            }
            AnimateCategory();
            CheckForInput();

            if (inMenu && menuButtons.Length > 0)
            {
                menuPanel.transform.Find("Indicator").position = menuButtons[buttonIndex].transform.position;
                menuPanel.transform.Find("Indicator").gameObject.SetActive(true);
            }

            if (currentCategoryItems.Count > 0)
            {
                currentItem = currentCategoryItems[selectedItem];
            }

            if (!PauseManager.instance.inPartyMenu)
            {
                itemIndicator.transform.position = grid[selectedItem].position;
                if (!itemIndicator.activeSelf)
                {
                    itemIndicator.SetActive(true);
                }
            }
            else
            {
                itemIndicator.SetActive(false);
            }

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

    public void DrawInventory()
    {
        if (!isInventoryDrawn)
        {
            categoryText.SetText(currentCategory);

            if (inventory.items.Count > 0)
            {
                counter = 0;
                foreach (Item item in inventory.items)
                {
                    if (item.category.ToString().Equals(currentCategory) && item.isFavorite)
                    {
                        counter = DrawItem(item, counter, true);
                    }
                }

                for (int i = counter; i < inventory.items.Count; i++)
                {
                    if (inventory.items[i].category.ToString().Equals(currentCategory) && !inventory.items[i].isFavorite)
                    {
                        counter = DrawItem(inventory.items[i], counter);
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
                itemSlot.Find("Favorite").gameObject.SetActive(false);
                itemSlot.Find("New").gameObject.SetActive(false);
            }
        }
    }

    private int DrawItem(Item item, int counter, bool isFavorite = false)
    {
        Transform itemSlot = grid[counter];
        itemSlot.GetComponent<ItemSelection>().slotIndex = counter;

        itemSlot.Find("Sprite").GetComponent<Image>().sprite = item.sprite;
        itemSlot.Find("Amount").GetComponentInChildren<TextMeshProUGUI>().SetText(item.amount.ToString());
        itemSlot.Find("Sprite").gameObject.SetActive(true);
        itemSlot.Find("Amount").gameObject.SetActive(true);
        if (isFavorite)
        {
            itemSlot.Find("Favorite").gameObject.SetActive(true);
        }
        else
        {
            itemSlot.Find("Favorite").gameObject.SetActive(false);
        }

        if (item.isNew)
        {
            itemSlot.Find("New").gameObject.SetActive(true);
        }
        else
        {
            itemSlot.Find("New").gameObject.SetActive(false);
        }

        counter++;

        currentCategoryItems.Add(item);

        return counter;
    }


    private void CheckForInput()
    {
        if (!PauseManager.instance.inPartyMenu)
        {
            if (!inMenu)
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
                            {
                                StartCoroutine(inventoryContainer.FadeObject(0.7f, 0.1f));
                                PauseManager.instance.inPartyMenu = true;
                            }
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
            else if (inMenu)
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
                if (!inMenu)
                {
                    StartCoroutine(CreateMenu(indicatorAnim));
                }
            }
            else if (Input.GetButtonUp("Interact") && !inMenu)
            {
                indicatorAnim.Rebind();
            }

            if (Input.GetButtonDown("Interact") && inMenu)
            {
                StartCoroutine(ChoiceMade());
            }
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

        for (int i = 0; i < grid.Length; i++)
        {
            if (grid[i] != grid[selectedItem])
            {
                StartCoroutine(grid[i].gameObject.FadeObject(0.5f, 0.1f));
            }
        }

        StartCoroutine(CreateMenuButtons(currentItem));
        menuPanel.transform.Find("Base").position = grid[selectedItem].position;
        menuPanel.SetActive(true);

        inMenu = true;
    }

    public void DestroyMenu()
    {
        menuPanel.SetActive(false);
        menuPanel.transform.Find("Indicator").gameObject.SetActive(false);

        for (int i = 0; i < grid.Length; i++)
        {
            if (grid[i] != grid[selectedItem])
            {
                StartCoroutine(grid[i].gameObject.FadeObject(1f, 0.1f));
            }
        }

        DestroyMenuButtons();

        inMenu = false;
    }

    public IEnumerator ChoiceMade()
    {
        ItemEventHandler choiceEvent = menuButtons[buttonIndex].GetComponent<ItemEventHandler>();
        choiceEvent.eventHandler.Invoke(currentItem);
        yield return null;
        DestroyMenu();
        selectedButton = 0; buttonIndex = 0;
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

    public void GiveItem(Item item)
    {
        StartCoroutine(HandOverItem());
    }

    private IEnumerator HandOverItem()
    {
        yield return null;
        StartCoroutine(inventoryContainer.FadeObject(0.7f, 0.1f));
        PauseManager.instance.inPartyMenu = true;
        givingItem = true;
    }

    public void UpdateInventory()
    {
        currentCategoryItems.Clear();
        isInventoryDrawn = false;

        categoryText.SetText(currentCategory);

        if (inventory.items.Count > 0)
        {
            counter = 0;
            foreach (Item item in inventory.items)
            {
                if (item.category.ToString().Equals(currentCategory) && item.isFavorite)
                {
                    counter = DrawItem(item, counter, true);
                }
            }

            for (int i = counter; i < inventory.items.Count; i++)
            {
                if (inventory.items[i].category.ToString().Equals(currentCategory) && !inventory.items[i].isFavorite)
                {
                    counter = DrawItem(inventory.items[i], counter);
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
            itemSlot.Find("Favorite").gameObject.SetActive(false);
        }


        isInventoryDrawn = true;

    }
}
