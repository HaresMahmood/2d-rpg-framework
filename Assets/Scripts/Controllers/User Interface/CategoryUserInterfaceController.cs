using System.Collections;
using System.Collections.Generic;
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

    #endregion

    #region Miscellaneous Methods

    public override IEnumerator SetActive(bool isActive, bool condition = true)
    {
        int increment = isActive ? -1 : 0;

        yield return null;

        Flags.isActive = isActive;

        if (isActive && condition)
        {
            UpdateSelectedCategory(selectedCategory, selectedValue, increment);
        }
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

    protected virtual void GetInput()
    {
        if (Input.GetAxisRaw("Trigger") == 0)
        {
            bool hasInput;
            (selectedValue, hasInput) = input.GetInput("Horizontal", "Vertical", UserInterface.MaxObjects, selectedValue, true, 1, 7); // TODO: Change max value.
            if (hasInput)
            {
                UpdateSelectedObject(selectedValue);
            }
        }
        else
        {
            bool hasInput = TriggerInput(categoryNames.Count);
            if (hasInput)
            {
                selectedValue= 0;
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
