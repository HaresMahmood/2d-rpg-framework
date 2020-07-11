using UnityEngine;

/// <summary>
///
/// </summary>
public class InventoryController : CategoryUserInterfaceController
{
    #region Constants

    public override int MaxViewableObjects => 28;

    #endregion

    #region Fields

    private static InventoryController instance;

    [SerializeField] private InventoryUserInterface userInterface;

    #endregion

    #region Properties

    /// <summary>
    /// Singleton pattern.
    /// </summary>
    public static InventoryController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryController>();
            }

            return instance;
        }
    }

    public override UserInterface UserInterface
    {
        get { return userInterface; }
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

    protected override void InteractInput(int selectedValue)
    {
        base.InteractInput(selectedValue);

        ((InventoryUserInterface)UserInterface).ActiveSubMenu(selectedValue);
    }

    protected override void GetInput()
    {
        base.GetInput();

        if (Input.GetButtonDown("Interact"))
        {
            InteractInput(selectedValue);
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
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
        if (Flags.isActive)
        {
            GetInput();
        }
    }

    #endregion
}
