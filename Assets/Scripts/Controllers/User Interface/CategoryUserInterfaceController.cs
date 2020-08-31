using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///
/// </summary>
public abstract class CategoryUserInterfaceController : UserInterfaceController
{
    #region Constants

    public abstract int MaxViewableObjects { get; }

    #endregion

    #region Fields

    private CategoryUserInterface userInterface;

    #endregion

    #region Properties

    public override UserInterface UserInterface
    {
        get { return userInterface; }
    }

    #endregion

    #region Variables

    protected List<Categorizable> categorizableObjects  = new List<Categorizable>();

    protected List<string> categoryNames;

    protected int selectedCategory = 0;
    protected int selectedSortingMethod;

    #endregion

    #region Miscellaneous Methods

    public override IEnumerator SetActive(bool isActive, bool condition = true)
    {
        int increment = isActive ? -1 : 0;

        yield return null;

        Flags.IsActive = isActive;

        if (isActive && condition)
        {
            UpdateSelectedCategory(selectedCategory, selectedValue, increment);
        }

        StartCoroutine(base.SetActive(isActive, condition));
    }

    protected void UpdateSelectedCategory(int selectedCategory, int selectedValue, int increment)
    {
        ((CategoryUserInterface)UserInterface).UpdateSelectedCategory(categorizableObjects, selectedCategory, selectedValue, increment, MaxViewableObjects);
    }

    protected bool TriggerInput(int max)
    {
        bool hasInput;
        (selectedCategory, hasInput) = input.GetInput("Trigger", TestInput.Axis.Horizontal, max, selectedCategory);

        return hasInput;
    }

    protected void ToggleInput(string value, int max)
    {
        int index = buttons.IndexOf(buttons.Find(b => b.isAnimated == true));

        BottomPanelUserInterface.Instance.AnimateButton(index, value);

        selectedSortingMethod = ExtensionMethods.IncrementInt(selectedSortingMethod, 0, max, 1);
    }

    protected virtual void UpdateSortingMethod()
    { }

    protected virtual void GetInput(int max)
    {
        if (Input.GetAxisRaw("Trigger") == 0)
        {
            int selectedValue = this.selectedValue;
            bool hasInput;
            (this.selectedValue, hasInput) = input.GetInput("Horizontal", "Vertical", UserInterface.MaxObjects, this.selectedValue, true, 1, max);

            if (hasInput)
            {
                if (selectedValue % max != 0)
                {
                    UpdateSelectedObject(this.selectedValue);
                }
                else
                {
                    if (selectedValue - this.selectedValue != 1)
                    {
                        UpdateSelectedObject(this.selectedValue);
                    }
                    else
                    {
                        this.selectedValue = selectedValue;
                    }
                }
            }
        }
        else
        {
            bool hasInput = TriggerInput(categoryNames.Count);
            if (hasInput)
            {
                selectedValue = 0;
                UpdateSelectedCategory(selectedCategory, selectedValue, (int)Input.GetAxisRaw("Trigger"));
            }
        }
    }

    protected override void GetInput(string axis)
    {
        if (Input.GetAxisRaw("Trigger") == 0)
        {
            base.GetInput(axis);
        }
        else
        {
            bool hasInput = TriggerInput(categoryNames.Count);
            if (hasInput)
            {
                selectedValue = 0;
                UpdateSelectedCategory(selectedCategory, selectedValue, (int)Input.GetAxisRaw("Trigger"));
            }
        }
    }

    #endregion

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected virtual void Awake()
    {
        categoryNames = new List<string>();

        // Adds name of every category of class "Categorizable" to List.
        for (int i = 0; i < categorizableObjects[0].Categorization.GetTotalCategories(); i++)
        {
            categoryNames.Add(categorizableObjects[0].Categorization.GetCategoryFromIndex(i));
        }
    }
}
