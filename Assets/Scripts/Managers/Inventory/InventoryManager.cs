﻿using System;
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
    [HideInInspector] public Transform[] grid, categoryContainer;
    [HideInInspector] public Animator indicatorAnim, arrowAnim;
    private GameObject itemIndicator, menuPanel, amountPicker, itemInfo;
    private GameObject[] menuButtons;
    private TextMeshProUGUI categoryText;

    [HideInInspector] public Item currentItem;
    private List<Item> categoryItems;
    private string[] categories = new string[] { "Key", "Health", "PokéBall", "Battle", "TM", "Berry", "Other" };
    private string currentCategory;
    [HideInInspector] public int selectedSlot = 0;
    private int totalSlots, selectedCategory = 0, selectedMenuButton = 0, totalMenuButtons, counter, amount;

    [HideInInspector] public bool isActive, inContextMenu = false, isGivingItem = false, isDiscardingItem = false;
    private bool isInventoryDrawn, isInteracting = false, isDirty = false;

    private TestInput input = new TestInput();

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
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
        amountPicker = inventoryContainer.transform.Find("Amount Picker").gameObject;
        itemInfo = inventoryContainer.transform.Find("Item Information").gameObject;

        arrowAnim = inventoryContainer.transform.Find("Categories/Navigation").GetComponent<Animator>();
        indicatorAnim = itemIndicator.GetComponent<Animator>();

        categoryText = inventoryContainer.transform.Find("Categories/Information/Name").GetComponent<TextMeshProUGUI>();

        grid = inventoryContainer.transform.Find("Item Grid").transform.GetChildren();
        categoryContainer = inventoryContainer.transform.Find("Categories/Category Icons").GetChildren();

        categoryItems = new List<Item>();
        currentCategory = categories[0];
        inventoryContainer.SetActive(false);

        UpdateInventory();
        AnimateCategory();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        OnActive();

        if (Input.GetButtonDown("Interact") && isGivingItem &&!isDiscardingItem)
        {
            if (PartyManager.instance.party.playerParty[PauseManager.instance.selectedSlot].heldItem != currentItem)
            {
                PartyManager.instance.party.playerParty[PauseManager.instance.selectedSlot].heldItem = currentItem;
                EditorUtility.SetDirty(PartyManager.instance.party.playerParty[PauseManager.instance.selectedSlot]); //TODO: Debug

                RemoveItem(currentItem);
            }

            //StartCoroutine(inventoryContainer.FadeOpacity(1f, 0.1f));
            Fade(1f);
            PauseManager.instance.inPartyMenu = false;
            isGivingItem = false;
        }

        if (!isDiscardingItem)
            amountPicker.SetActive(false);
        else
        {
            StartCoroutine(DiscardItem(currentItem, amount));

            if (Input.GetAxisRaw("Vertical") != 0)
            {
                if (!isInteracting)
                {
                    if (Input.GetAxisRaw("Vertical") < 0)
                    {
                        if (amount > 1)
                            amount--;
                        else
                            amount = currentItem.amount;
                    }
                    else if (Input.GetAxisRaw("Vertical") > 0)
                    {
                        if (amount < currentItem.amount)
                            amount++;
                        else
                           amount = 1;
                    }
                    isInteracting = true;
                }
            }
            else
                isInteracting = false;
        }
    }

    private void InventoryManager_OnUserInput(object sender, System.EventArgs e)
    {
        #if DEBUG
            if (GameManager.Debug())
            {
                Debug.Log("[INVENTORY MANAGER:] Event function called (OnUserInput).");
            }
        #endif

        if (isDirty)
        {
            UpdateInventory();
            AnimateCategory();
            isDirty = false;
        }

        if (categoryItems.Count > 0)
        {
            if (selectedSlot > -1)
            {
                currentItem = categoryItems[selectedSlot];
            }
            else
            {
                currentItem = categoryItems[0];
            }
            SetDescription(currentItem);
        }
        else if (categoryItems.Count == 0 || currentItem == null)
        {
            SetDescription();
        }

        if (itemIndicator.activeSelf)
        {
            if (selectedSlot > -1)
            {
                itemIndicator.transform.position = grid[selectedSlot].position;
            }
            else
            {
                itemIndicator.transform.position = grid[0].position;
            }
        }

        input.OnUserInput -= InventoryManager_OnUserInput;
    }

    #endregion
    private void OnActive()
    {
        if (PauseManager.instance.isPaused)
        {
            inventoryContainer.SetActive(true);

            GetInput();

            if (inContextMenu && menuButtons.Length > 0)
            {
                menuPanel.transform.Find("Indicator").position = menuButtons[selectedMenuButton].transform.position;
                menuPanel.transform.Find("Indicator").gameObject.SetActive(true);
            }
        }
        else
        {
            inventoryContainer.SetActive(false);
        }
    }

    public void UpdateInventory()
    {
        #if DEBUG
            if (GameManager.Debug())
            {
                Debug.Log("[INVENTORY MANAGER:] Updating inventory.");
            }
        #endif
        ResetInventory();

        currentCategory = categories[selectedCategory];
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

            totalSlots = counter;

            isInventoryDrawn = true;
        }

        for (int i = counter; i < grid.Length; i++)
        {
            Transform itemSlot = grid[i];
            itemSlot.Find("Sprite").gameObject.SetActive(false);
            itemSlot.Find("Amount").gameObject.SetActive(false);
            itemSlot.Find("Favorite").gameObject.SetActive(false);
        }
    }

    private int DrawItem(Item item, int position, bool isFavorite = false)
    {
        Transform itemSlot = grid[position];

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

        position++;

        categoryItems.Add(item);

        return position;
    }

    private void SetDescription(Item item = null)
    {
        if (categoryItems.Count > 0)
        {
            itemInfo.SetActive(true);
            itemInfo.transform.Find("Name/Item Name").GetComponent<TextMeshProUGUI>().SetText(item.name);
            itemInfo.transform.Find("Description/Item Description").GetComponent<TextMeshProUGUI>().SetText(item.description);
        }
        else if (item == null)
        {
            inventoryContainer.transform.Find("Item Information").gameObject.SetActive(false);
        }
    }

    private void GetInput()
    {
        if (isActive && !PauseManager.instance.inPartyMenu && !isGivingItem)
        {
            if (!inContextMenu && !isDiscardingItem)
            {
                if (Input.GetAxisRaw("Trigger") == 0)
                {
                    bool hasInput;
                    (selectedSlot, hasInput) = input.GetInput("Horizontal", "Vertical", totalSlots, selectedSlot, 1, 6);
                    if (hasInput)
                    {
                        input.OnUserInput += InventoryManager_OnUserInput;
                    }
                }
                else
                {
                    bool hasInput;
                    (selectedCategory, hasInput) = input.GetInput("Trigger", TestInput.Axis.Horizontal, categories.Length, selectedCategory);
                    isDirty = true;
                    if (hasInput)
                    {
                        input.OnUserInput += InventoryManager_OnUserInput;
                        StartCoroutine(AnimateArrows(arrowAnim, (int)Input.GetAxisRaw("Trigger")));
                    }
                }
            }
            else if (inContextMenu)
            {
                bool hasInput;
                (selectedMenuButton, hasInput) = input.GetInput("Vertical", TestInput.Axis.Vertical, totalMenuButtons, selectedMenuButton);
            }

            if (!isDiscardingItem)
            {
                if (Input.GetButtonDown("Interact"))
                {
                    if (!inContextMenu)
                    {
                        StartCoroutine(CreateMenu(indicatorAnim));
                    }
                }
                if (Input.GetButtonDown("Interact") && inContextMenu)
                {
                    StartCoroutine(ChoiceMade());
                }
            }
        }
    }

    public void Fade(float opacity, bool fadeFull = true)
    {
        Transform[] children = inventoryContainer.transform.GetChildren();

        foreach (Transform child in children)
        {
            if (child != null)
            {
                StartCoroutine(child.gameObject.FadeOpacity(opacity, 0.1f));
            }
        }

        if (fadeFull)
        {
            StartCoroutine(PauseManager.instance.pauseContainer.transform.Find("Target Sprite").gameObject.FadeOpacity(opacity, 0.1f));
        }
    }

    private void AnimateCategory()
    {
        foreach (Transform category in categoryContainer)
        {
            if (category != categoryContainer[selectedCategory])
            {
                category.GetComponent<Animator>().SetBool("isSelected", false);
                StartCoroutine(category.GetComponentInChildren<Image>().gameObject.FadeColor(Color.white, 0.1f));
            }
            else
            {
                category.GetComponent<Animator>().SetBool("isSelected", true);
                StartCoroutine(category.GetComponentInChildren<Image>().gameObject.FadeColor(GameManager.GetAccentColor(), 0.1f));
            }
        }
    }

    private IEnumerator AnimateArrows(Animator anim, int value)
    {
        anim.SetBool("isActive", true);
        anim.SetFloat("Blend", value);
        yield return new WaitForSecondsRealtime(0.1f);
        anim.SetFloat("Blend", 0);
        anim.SetBool("isActive", false);
    }

    public void ResetInventory()
    {
        #if DEBUG
                if (GameManager.Debug())
                {
                    Debug.Log("[INVENTORY MANAGER:] Reseting inventory.");
                }
        #endif

        counter = 0; selectedSlot = 0;
        categoryItems.Clear();
        //leftAnim.Rebind(); rightAnim.Rebind();
    }

    private IEnumerator CreateMenu(Animator anim)
    {
        anim.ResetTrigger("isPressed");
        anim.SetTrigger("isPressed");
        float waitTime = anim.GetAnimationTime();
        yield return new WaitForSecondsRealtime(0.15f);
        itemIndicator.SetActive(false);

        for (int i = 0; i < grid.Length; i++)
        {
            if (grid[i] != grid[selectedSlot])
            {
                StartCoroutine(grid[i].gameObject.FadeOpacity(0.5f, 0.1f));
            }
        }

        StartCoroutine(CreateMenuButtons(currentItem));
        menuPanel.transform.Find("Base").position = grid[selectedSlot].position;
        menuPanel.SetActive(true);

        inContextMenu = true;
    }

    public IEnumerator DestroyMenu()
    {
        StartCoroutine(menuPanel.FadeOpacity(0f, 0.1f));
        yield return new WaitForSecondsRealtime(0.1f);
        menuPanel.SetActive(false);
        menuPanel.transform.Find("Indicator").gameObject.SetActive(false);
        menuPanel.GetComponent<CanvasGroup>().alpha = 1;

        if (!isDiscardingItem)
        {
            for (int i = 0; i < grid.Length; i++)
            {
                if (grid[i] != grid[selectedSlot])
                {
                    StartCoroutine(grid[i].gameObject.FadeOpacity(1f, 0.1f));
                }
            }
        }

        DestroyMenuButtons();

        inContextMenu = false;
        itemIndicator.SetActive(true);
    }

    public IEnumerator ChoiceMade()
    {
        ItemEventHandler choiceEvent = menuButtons[selectedMenuButton].GetComponent<ItemEventHandler>();
        choiceEvent.eventHandler.Invoke(currentItem);
        yield return null;
        StartCoroutine(DestroyMenu());
        selectedMenuButton = 0;
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
            menuButtonObj.GetComponent<CanvasGroup>().alpha = 0;

            menuButtons[i] = menuButtonObj;
        }

        totalMenuButtons = menuButtons.Length;
        selectedMenuButton = 0;

        float menuAnimationDelay = 0.2f / menuButtons.Length;
        StartCoroutine(menuPanel.transform.Find("Base").gameObject.FadeOpacity(0.9f, menuAnimationDelay));

        foreach (GameObject button in menuButtons)
        {
            float buttonAnimationDelay = 0.05f;
            StartCoroutine(button.FadeOpacity(1f, buttonAnimationDelay));
            yield return new WaitForSecondsRealtime(buttonAnimationDelay);
        }
    }

    private void DestroyMenuButtons()
    {
        StartCoroutine(menuPanel.transform.Find("Base").gameObject.FadeOpacity(0f, 0.0001f));

        for (int i = 0; i < menuButtons.Length; i++)
            Destroy(menuButtons[i]);

        menuButtons = null;
    }

    public void GiveItem(Item item)
    {
        StartCoroutine(GiveItemRoutine());
    }

    private IEnumerator GiveItemRoutine()
    {
        yield return null;
        StartCoroutine(inventoryContainer.FadeOpacity(0.5f, 0.1f));
        PauseManager.instance.inPartyMenu = true;
        isGivingItem = true;
    }

    public IEnumerator DiscardItem(Item item, int amount)
    {
        yield return null;
        amountPicker.transform.position = new Vector2(grid[selectedSlot].position.x + 215, grid[selectedSlot].position.y);
        amountPicker.SetActive(true);

        amountPicker.GetComponentInChildren<TextMeshProUGUI>().SetText(amount.ToString());

        if (Input.GetButtonDown("Interact") && isDiscardingItem)
        {
            RemoveItem(item, amount);
            for (int i = 0; i < grid.Length; i++)
            {
                if (grid[i] != grid[selectedSlot])
                {
                    StartCoroutine(grid[i].gameObject.FadeOpacity(1f, 0.1f));
                }
            }
            isDiscardingItem = false;
            amountPicker.SetActive(false);
        }
    }

    public void RemoveItem(Item item, int amount = 1)
    {
        if (item.amount > 1 || (item.amount - amount > 0))
        {
            item.amount -= amount;
        }
        else
        {
            inventory.items.Remove(item);
        }

        UpdateInventory();
    }
}
