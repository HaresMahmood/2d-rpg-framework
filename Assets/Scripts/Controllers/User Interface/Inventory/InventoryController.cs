using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class InventoryController : CategoryUserInterfaceController
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

    public override UserInterfaceFlags Flags
    {
        get { return flags; }
    }

    #endregion

    #region Variables

    public Inventory inventory;

    [Header("Values")]
    [SerializeField] private SortingMethod sortingMethod = SortingMethod.None;

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

    /*
    public void ActiveSidePanel()
    {
        Flags.isActive = false;
        PauseManager.instance.InitializeSidePanel();
    }

    public IEnumerator ActiveSidePanel(float delay)
    {
        if (selectedValue % 7 == 0 || selectedValue == 0) // Debug
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
        selectedValue = selectedButton > -1 ? selectedValue : 0;
    }

    private void UpdateSelectedCategory(int increment)
    {
        selectedValue = 0;
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
        userInterface.UpdateItem(selectedValue);
    }

    protected override void GetInput()
    {
        base.GetInput();

        if (Input.GetButtonDown("Interact"))
        {
            StartCoroutine(userInterface.AnimateItemSelection(selectedValue));
            flags.isInSubmenu = true;
        }

        if (Input.GetButtonDown("Toggle"))
        {
            sortingMethod = (SortingMethod)ExtensionMethods.IncrementInt((int)sortingMethod, 0, Enum.GetValues(typeof(SortingMethod)).Length, 1);
            if (sortingMethod == SortingMethod.None) sortingMethod = SortingMethod.AToZ;
            userInterface.UpdateSortingMethod(inventory, sortingMethod, selectedCategory);
        }
    }

    /*
    protected void GetInput(int something)
    {
        if (!flags.isInSubmenu)
        {

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
    */

    #endregion

    #region Unity Methods

    protected override void Awake()
    {
        categorizableObjects.AddRange(inventory.items);

        base.Awake();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    protected override void Update()
    {
        if (flags.isActive)
        {
            GetInput();
        }
    }

    #endregion
}
