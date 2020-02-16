using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class InventoryController : UserInterfaceController
{
    #region Fields

    [SerializeField] private InventoryUserInterface userInterface;

    private InventoryFlags flags = new InventoryFlags(false, false);

    #endregion

    #region Properties

    protected override UserInterface UserInterface
    {
        get { return userInterface; }
    }

    protected override UserInterfaceFlags Flags
    {
        get { return flags; }
    }

    #endregion

    #region Variables

    [Header("Setup")]
    public Inventory inventory;

    [Header("Values")]
    [SerializeField] private SortingMethod sortingMethod = SortingMethod.None;

    public Item selectedItem { get; set; }

    public List<string> categoryNames { get; private set; } = new List<string>();

    public int selectedSlot { get; private set; } = 0; // TODO: Should be private field
    private int selectedCategory = 0;
    public int selectedButton { get; private set; } = 0;

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

    #region Nested Classes

    protected sealed class InventoryFlags : UserInterfaceFlags
    {
        public bool isInSubmenu { get; set; }

        internal InventoryFlags(bool isActive, bool isInSubmenu) : base(isActive)
        {
            this.isInSubmenu = isInSubmenu;
        }
    }

    #endregion

    #region Miscellaneous Methods

    public void ActiveSidePanel()
    {
        Flags.isActive = false;
        PauseManager.instance.InitializeSidePanel();
    }

    public IEnumerator ActiveSidePanel(float delay)
    {
        if (selectedSlot % 7 == 0 || selectedSlot == 0) // Debug
        {
            yield return new WaitForSecondsRealtime(delay);
            if (Input.GetAxisRaw("Horizontal") == -1)
            {
                flags.isActive = false;
                userInterface.FadeUserInterface(0.3f, 0.15f);
                PauseManager.instance.InitializeSidePanel();
            }
        }
    }

    public IEnumerator DeactivateSidePanel(float delay)
    {
        userInterface.FadeUserInterface(1f, 0.15f);
        yield return new WaitForSecondsRealtime(delay); flags.isActive = true;
    }

    public void CloseSelectionMenu(int selectedButton = -1)
    {
        userInterface.CloseSubMenu(selectedButton);

        flags.isInSubmenu = selectedButton > -1 ? true : false;
        this.selectedButton = selectedButton > -1 ? this.selectedButton : 0;
    }

    private void UpdateSelectedCategory(int increment)
    {
        selectedSlot = 0;
        userInterface.UpdateSelectedCategory(inventory, selectedCategory, increment);
    }

    private void UpdateSelectedItem(int selectedItem)
    {
        userInterface.UpdateSelectedItem(selectedItem);
    }

    private void UpdateSelectedButton(int selectedButton, int increment)
    {
        StartCoroutine(userInterface.UpdateIndicator(selectedButton, 0.1f, true));
        userInterface.UpdateSelectedButton(selectedButton, increment);
    }

    public void UpdateItem()
    {
        userInterface.UpdateItem(selectedSlot);
    }

    private void GetInput()
    {
        if (!flags.isInSubmenu)
        {
            if (Input.GetAxisRaw("Trigger") == 0) // TODO: Very ugly!
            {
                bool hasInput;
                (selectedSlot, hasInput) = input.GetInput("Horizontal", "Vertical", userInterface.categoryItems.Count, selectedSlot, true, 1, 7);
                if (hasInput)
                {
                    UpdateSelectedItem(selectedSlot);
                }
            }
            else
            {
                bool hasInput;
                (selectedCategory, hasInput) = input.GetInput("Trigger", TestInput.Axis.Horizontal, categoryNames.Count, selectedCategory);
                if (hasInput)
                {
                    UpdateSelectedCategory((int)Input.GetAxisRaw("Trigger"));
                }
            }

            if (Input.GetButtonDown("Interact"))
            {
                StartCoroutine(userInterface.AnimateItemSelection(selectedSlot));
                flags.isInSubmenu = true;
            }

            if (Input.GetButtonDown("Toggle"))
            {
                sortingMethod = (SortingMethod)ExtensionMethods.IncrementInt((int)sortingMethod, 0, Enum.GetValues(typeof(SortingMethod)).Length, 1);
                if (sortingMethod == SortingMethod.None) sortingMethod = SortingMethod.AToZ;
                userInterface.UpdateSortingMethod(inventory, sortingMethod, selectedCategory);
            }

            StartCoroutine(ActiveSidePanel(0.2f));
        }
        else
        {
            bool hasInput;
            (selectedButton, hasInput) = input.GetInput("Horizontal", TestInput.Axis.Horizontal, userInterface.itemButtons.Count, selectedButton);
            if (hasInput)
            {
                UpdateSelectedButton(selectedButton, -(int)Input.GetAxisRaw("Horizontal"));
            }

            if (Input.GetButtonDown("Interact"))
            {
                CloseSelectionMenu(selectedButton);
            }

            if (Input.GetButtonDown("Cancel"))
            {
                CloseSelectionMenu();
            }
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        for (int i = 0; i < Enum.GetNames(typeof(Item.Category)).Length; i++)
        {
            //categoryNames.Add(((Item.Category)i).ToString());
        }
    }

    #endregion
}
