using System;
using UnityEngine;

//TODO: Should not be here!
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/// <summary>
///
/// </summary>
public class PauseManager : MonoBehaviour
{
    #region Variables

    public static PauseManager instance;

    /// <summary>
    /// Determines whether the game is paused or not.
    /// </summary>
    public bool isPaused; // Made public for debug purposes.

    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject pausePanel;
    
    public GameObject itemIndicator;

    private Transform[] grid;
    private Transform[] categoryContainer;

    private float initialPosY;
    private Color initialColor;

    public bool isInteracting = false;
    public int slotIndex, selectedItem;
    public string[] categories = new string[] { "Key", "Health", "PokéBall", "Battle", "TM", "Berry", "Other" };
    public string currentCategory;
    public int currentCategoryIndex;
    public int maxSlotIndex;
    public int counter;

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
        itemIndicator = pausePanel.transform.Find("Inventory/Indicator").gameObject;
        grid = pausePanel.transform.Find("Inventory/Item Grid").transform.GetChildren();
        categoryContainer = pausePanel.transform.Find("Inventory/Categories/Category Icons").GetChildren();
        initialPosY = categoryContainer[0].position.y;
        initialColor = categoryContainer[0].gameObject.GetComponent<Image>().color;

        isPaused = false;
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        TogglePause();

        if (isPaused)
        {
            itemIndicator.transform.position = grid[selectedItem].position;
            eventSystem.SetSelectedGameObject(grid[selectedItem].gameObject);
            foreach (Transform category in categoryContainer)
            {
                if (category != categoryContainer[currentCategoryIndex])
                {
                    category.position = new Vector2(category.position.x, initialPosY);
                    category.gameObject.GetComponent<Image>().color = initialColor;
                }
                else
                {
                    category.position = new Vector2(category.position.x, initialPosY + 40);
                    category.gameObject.GetComponent<Image>().color = "51c2fc".ToColor();
                }
            }
        }
    }

    #endregion

    public void TogglePause()
    {
        if (Input.GetButtonDown("Start") && !DialogManager.instance.isActive)
        {
            isPaused = !isPaused;
            selectedItem = 0; maxSlotIndex = 0;
            currentCategory = categories[0];
            eventSystem.firstSelectedGameObject = grid[0].transform.gameObject;
        }

        DrawInventory();

        if (isPaused)
        {
            pausePanel.SetActive(true);

            DrawInventory();
            CheckForInput();

            Time.timeScale = 0f;
        }
        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    private void DrawInventory()
    {
        pausePanel.transform.Find("Inventory/Categories/Category Information").GetComponentInChildren<TextMeshProUGUI>().SetText(currentCategory);

        //TODO: Move this whole block to InventoryManager!
        List<Item> inventory = InventoryManager.instance.inventory.items;

        if (InventoryManager.instance.inventory.items.Count > 0)
        {
            counter = 0; foreach (Item item in inventory)
            {
                if (item.category.ToString().Equals(currentCategory))
                {
                    Transform itemSlot = grid[counter];
                    itemSlot.GetComponent<ItemSelection>().slotIndex = counter;

                    itemSlot.Find("Sprite").GetComponent<Image>().sprite = item.sprite;
                    itemSlot.Find("Sprite").gameObject.SetActive(true);
                    itemSlot.Find("Amount").GetComponentInChildren<TextMeshProUGUI>().SetText(item.amount.ToString());
                    itemSlot.Find("Amount").gameObject.SetActive(true);

                    if (selectedItem == counter)
                    {
                        pausePanel.transform.Find("Inventory/Item Information/Name").gameObject.SetActive(true);
                        pausePanel.transform.Find("Inventory/Item Information/Description").gameObject.SetActive(true);
                        pausePanel.transform.Find("Inventory/Item Information/Name/Item Name").GetComponent<TextMeshProUGUI>().SetText(item.name);
                        pausePanel.transform.Find("Inventory/Item Information/Description/Item Description").GetComponent<TextMeshProUGUI>().SetText(item.description);
                    }
                    counter++;
                }
            }

            for (int i = counter; i < grid.Length; i++)
            {
                Transform itemSlot = grid[i];
                itemSlot.Find("Sprite").gameObject.SetActive(false);
                itemSlot.Find("Amount").gameObject.SetActive(false);
            }

            maxSlotIndex = counter;
        }
        //
    }

    private void CheckForInput()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            if (!isInteracting)
            {
                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    if (slotIndex < maxSlotIndex)
                        slotIndex++;
                    else
                        slotIndex = 0;
                }
                else if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    if (slotIndex > 0)
                        slotIndex--;
                    else
                        slotIndex = maxSlotIndex;
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
                    if ((slotIndex - 6) > 0)
                        slotIndex -= 6;
                    else
                        slotIndex = 0;
                }
                else if (Input.GetAxisRaw("Vertical") < 0)
                {
                    if ((slotIndex + 6) < maxSlotIndex)
                        slotIndex += 6;
                    else
                        slotIndex = maxSlotIndex;
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
                    if (Array.IndexOf(categories, currentCategory) < categories.Length)
                    {
                        currentCategory = categories[Array.IndexOf(categories, currentCategory) + 1];
                        currentCategoryIndex++;
                    }
                    else
                        currentCategory = categories[categories.Length - 1];
                }
                else if (Input.GetAxisRaw("Trigger") < 0)
                {
                    if (Array.IndexOf(categories, currentCategory) > 0)
                    {
                        currentCategory = categories[Array.IndexOf(categories, currentCategory) - 1];
                        currentCategoryIndex--;
                    }
                    else
                        currentCategory = categories[0];
                }
                isInteracting = true; counter = 0; DrawInventory();
            }
        }
        else
            isInteracting = false;
    }
}
