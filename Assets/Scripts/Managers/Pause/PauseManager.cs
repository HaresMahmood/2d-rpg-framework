using System;
using UnityEngine;

//TODO: Should not be here!
using UnityEngine.UI;
using TMPro;

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

    [SerializeField] private GameObject pausePanel;

    public GameObject itemIndicator;

    private Transform[] grid;

    public bool isInteracting;
    public int slotIndex, selectedItem;
    public int maxSlotIndex;

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

        isPaused = false;
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        TogglePause();
        CheckForInput();

        itemIndicator.transform.position = grid[selectedItem].transform.position;
    }

    #endregion

    public void TogglePause()
    {
        if (Input.GetButtonDown("Start") && !DialogManager.instance.isActive)
            isPaused = !isPaused;

        if (isPaused)
        {
            pausePanel.SetActive(true);

            //TODO: Move this whole block to InventoryManager!
            if (InventoryManager.instance.inventory.items.Count > 0)
            {
                maxSlotIndex = grid.Length - 1;
                foreach (Transform itemSlot in grid)
                {
                    if (Array.IndexOf(grid, itemSlot) < InventoryManager.instance.inventory.items.Count)
                    {
                        itemSlot.GetComponent<ItemSelection>().slotIndex = Array.IndexOf(grid, itemSlot);

                        Item item = InventoryManager.instance.inventory.items[Array.IndexOf(grid, itemSlot)];
                        itemSlot.Find("Sprite").GetComponent<Image>().sprite = item.sprite;
                        itemSlot.Find("Sprite").gameObject.SetActive(true);
                        itemSlot.Find("Amount").GetComponentInChildren<TextMeshProUGUI>().SetText(item.amount.ToString());

                        if (selectedItem == Array.IndexOf(grid, itemSlot))
                        {
                            pausePanel.transform.Find("Inventory/Item Information/Name/Item Name").GetComponent<TextMeshProUGUI>().SetText(item.name);
                            pausePanel.transform.Find("Inventory/Item Information/Description/Item Description").GetComponent<TextMeshProUGUI>().SetText(item.description);
                        }
                    }
                }
            }
            //

            Time.timeScale = 0f;
        }
        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    private void CheckForInput()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            if (!isInteracting)
            {
                if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    if (slotIndex < maxSlotIndex)
                        slotIndex++;
                    else
                        slotIndex = 0;
                }
                else if (Input.GetAxisRaw("Horizontal") > 0)
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
                if (Input.GetAxisRaw("Vertical") < 0)
                {
                    if (slotIndex < maxSlotIndex)
                        slotIndex += 6;
                    else
                        slotIndex = 0;
                }
                else if (Input.GetAxisRaw("Vertical") > 0)
                {
                    if (slotIndex > 0)
                        slotIndex += 6;
                    else
                        slotIndex = maxSlotIndex;
                }
                isInteracting = true;
            }
        }
        else
            isInteracting = false;
    }
}
