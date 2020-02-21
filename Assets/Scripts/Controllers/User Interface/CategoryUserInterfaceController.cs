using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class CategoryUserInterfaceController : UserInterfaceController
{
    #region Fields

    private CategoryUserInterface userInterface;

    #endregion

    #region Properties

    protected override UserInterface UserInterface
    {
        get { return userInterface; }
    }

    #endregion

    #region Variables

    protected List<Categorizable> categorizableObjects = new List<Categorizable>();

    protected List<string> categoryNames = new List<string>();

    protected int selectedCategory = 0;

    #endregion

    #region Miscellaneous Methods

    public override void OnPause(bool isPaused)
    {
        int increment = isPaused ? -1 : 0;

        Flags.isActive = true;
        UpdateSelectedCategory(selectedCategory, increment);
    }

    protected void UpdateSelectedCategory(int selectedCategory, int increment)
    {
        selectedValue = 0;
        ((CategoryUserInterface)UserInterface).UpdateSelectedCategory(categorizableObjects, selectedCategory, increment);
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
                UpdateSelectedValue(selectedValue);
            }
        }
        else
        {
            bool hasInput = TriggerInput(categoryNames.Count);
            if (hasInput)
            {
                UpdateSelectedCategory(selectedCategory, (int)Input.GetAxisRaw("Trigger"));
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
                UpdateSelectedCategory(selectedCategory, (int)Input.GetAxisRaw("Trigger"));
            }
        }
    }

    #endregion

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        // Adds name of every category of class "Categorizable" to List.
        for (int i = 0; i < categorizableObjects[0].Categorization.GetTotalCategories(); i++)
        {
            categoryNames.Add(categorizableObjects[0].Categorization.GetCategoryFromIndex(i));
        }
    }
}
