using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class InventoryManager : MonoBehaviour
{
    #region Variables

    public static InventoryManager instance;

    public List<string> categoryNames { get; private set; } = new List<string>();

    [Header("Setup")]
    public Inventory inventory;
    [SerializeField] private InventoryUserInterface userInterface;
    public List<PanelButton> buttons = new List<PanelButton>();

    [Header("Settings")]
    [SerializeField] private GameObject menuButtonPrefab;

    [Header("Values")]
    [SerializeField] private SortingMethod sortingMethod = SortingMethod.None;

    private TestInput input = new TestInput();
    public Flags flags = new Flags(false, false, false);

    [HideInInspector] public int selectedItem = 0;
    private int selectedCategory = 0;
    private int selectedButton = 0;

    #endregion

    #region Structs

    public struct Flags
    {
        public bool isActive { get; set; }
        public bool isItemSelected { get; set; }
        public bool isDirty { get; set; }

        public Flags(bool isActive, bool isItemSelected, bool isDirty)
        {
            this.isActive = isActive;
            this.isItemSelected = isItemSelected;
            this.isDirty = isDirty;
        }
    }

    #endregion

    #region Enums

    public enum SortingMethod
    {
        None,
        AToZ,
        ZToA,
        AmountAscending,
        AmountDescending,
        FavoriteFirst,
        NewFirst
    }

    #endregion

    #region Miscellaneous Methods

    public void OnActive()
    {
        StartCoroutine(FindObjectOfType<BottomPanelUserInterface>().ChangePanelButtons(buttons));
    }

    private void UpdateSelectedCategory(int increment)
    {
        selectedItem = 0;
        userInterface.UpdateSelectedCategory(inventory, selectedCategory, increment);
    }

    private void UpdateSelectedItem(int selectedItem)
    {
        userInterface.UpdateSelectedItem(selectedItem);
    }

    private void UpdateSelectedButton(int selectedButton)
    {
        StartCoroutine(userInterface.UpdateIndicator(selectedButton, 0.1f, true));
    }

    private void GetInput()
    {
        if (!flags.isItemSelected)
        {
            if (Input.GetAxisRaw("Trigger") == 0) // TODO: Very ugly!
            {
                bool hasInput;
                (selectedItem, hasInput) = input.GetInput("Horizontal", "Vertical", userInterface.categoryItems.Count, selectedItem, true, 1, 7);
                if (hasInput)
                {
                    UpdateSelectedItem(selectedItem);
                }
            }
            else
            {
                bool hasInput;
                (selectedCategory, hasInput) = input.GetInput("Trigger", TestInput.Axis.Horizontal, categoryNames.Count, selectedCategory);
                flags.isDirty = true;
                if (hasInput)
                {
                    UpdateSelectedCategory((int)Input.GetAxisRaw("Trigger"));
                }
            }

            if (Input.GetButtonDown("Interact"))
            {
                StartCoroutine(userInterface.AnimateItemSelection(0.2f, selectedItem));
                flags.isItemSelected = true;
            }

            if (Input.GetButtonDown("Toggle"))
            {
                sortingMethod = (SortingMethod)ExtensionMethods.IncrementInt((int)sortingMethod, 0, Enum.GetValues(typeof(SortingMethod)).Length, 1);
                if (sortingMethod == SortingMethod.None) sortingMethod = SortingMethod.AToZ;
                userInterface.UpdateSortingMethod(inventory, sortingMethod, selectedCategory);
            }
        }
        else
        {
            // Setting is Selected

            bool hasInput;
            (selectedButton, hasInput) = input.GetInput("Horizontal", TestInput.Axis.Horizontal, userInterface.itemButtons.Count, selectedButton);
            if (hasInput)
            {
                UpdateSelectedButton(selectedButton);
            }

            /*
            if (Input.GetButtonDown("Interact"))
            {
                userInterface.AnimateItemSelection(selectedItem, 0.2f);
            }
            */

            if (Input.GetButtonDown("Cancel"))
            {
                StartCoroutine(userInterface.AnimateItemSelection(0.2f));
                flags.isItemSelected = false;
                selectedButton = 0;
            }
        }
    }

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
        for (int i = 0; i < Enum.GetNames(typeof(Item.Category)).Length; i++)
        {
            categoryNames.Add(((Item.Category)i).ToString());
        }

        OnActive();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        /*
        OnActive();

        if (Input.GetButtonDown("Interact") && isGivingItem && !isDiscardingItem)
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
        */
        // !PauseManager.instance.inPartyMenu 
        if (PauseManager.instance.flags.isActive && flags.isActive)
        {
            GetInput();
        }
    }

    /*
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
    */

    #endregion
}
