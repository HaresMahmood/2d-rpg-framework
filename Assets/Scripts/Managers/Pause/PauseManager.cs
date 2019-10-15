using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
//TODO: Should not be here!
using UnityEngine.UI;

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
    public int currentCategoryIndex = 0;
    private Animator categoryAnim;
    private Animator indicatorAnim;

    public Animator rightAnim;
    public Animator leftAnim;
    public int maxSlotIndex;
    public int counter;

    public bool isInMenu;

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
        categoryAnim = pausePanel.transform.Find("Inventory/Categories/Category Icons").GetComponent<Animator>();
        rightAnim = pausePanel.transform.Find("Inventory/Categories/Navigation/Right").GetComponent<Animator>();
        leftAnim = pausePanel.transform.Find("Inventory/Categories/Navigation/Left").GetComponent<Animator>();
        indicatorAnim = itemIndicator.GetComponent<Animator>();

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

            AnimateCategory();
        }
    }

    #endregion

    public void TogglePause()
    {
        if (Input.GetButtonDown("Start") && !DialogManager.instance.isActive)
        {
            isPaused = !isPaused;
            selectedItem = 0; maxSlotIndex = 0;
            currentCategory = categories[currentCategoryIndex];
            eventSystem.firstSelectedGameObject = grid[0].transform.gameObject;
        }

        DrawInventory();

        if (isPaused)
        {
            pausePanel.SetActive(true);
            CameraController.instance.GetComponent<PostprocessingBlur>().enabled = true;

            DrawInventory();
            CheckForInput();

            Time.timeScale = 0f;
        }
        else
        {
            categoryAnim.Rebind();
            pausePanel.SetActive(false);
            CameraController.instance.GetComponent<PostprocessingBlur>().enabled = false;
            Time.timeScale = 1f;
        }
    }

    private void DrawInventory()
    {
        pausePanel.transform.Find("Inventory/Categories/Category Information").GetComponentInChildren<TextMeshProUGUI>().SetText(currentCategory);

        //selectedItem = 0;
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

            maxSlotIndex = counter - 1;
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
                isInteracting = true; counter = 0; selectedItem = 0;
            }
        }
        else
        {
            isInteracting = false;
            leftAnim.Rebind(); rightAnim.Rebind();
        }

        if (Input.GetButtonDown("Interact"))
        {
            if (!isInMenu)
            {
                StartCoroutine(AnimateIndicator(indicatorAnim));
            }
        }
        else if (Input.GetButtonUp("Interact") && !isInMenu)
            indicatorAnim.Rebind();

        if (Input.GetButtonDown("Interact") && isInMenu)
        {
            pausePanel.transform.Find("Inventory/Menu").gameObject.SetActive(false);
            isInMenu = false;
        }
    }

    private void AnimateCategory()
    {
        foreach (Transform category in categoryContainer)
        {
            if (category != categoryContainer[currentCategoryIndex])
                categoryAnim.SetBool(category.name, false);
            else
                categoryAnim.SetBool(category.name, true);
        }
    }

    private void AnimateArrows(Animator anim)
    {
        anim.ResetTrigger("isActive");
        anim.SetTrigger("isActive");
    }

    private IEnumerator AnimateIndicator(Animator anim)
    {
        anim.ResetTrigger("isPressed");
        anim.SetTrigger("isPressed");
        float waitTime = anim.GetAnimationTime();
        yield return null;
        GameObject menuPanel = pausePanel.transform.Find("Inventory/Menu/Base").gameObject;
        pausePanel.transform.Find("Inventory/Menu").gameObject.SetActive(true);
        menuPanel.transform.position = grid[selectedItem].position;
        menuPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(menuPanel.GetComponent<Image>().sprite.rect.width, menuPanel.GetComponent<Image>().sprite.rect.height);
        isInMenu = true;
    }
}
