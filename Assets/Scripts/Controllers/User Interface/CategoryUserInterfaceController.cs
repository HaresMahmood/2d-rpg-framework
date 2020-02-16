using UnityEngine;

/// <summary>
///
/// </summary>
public class CategoryUserInterfaceController : UserInterfaceController
{
    #region Variables

    protected int selectedCategory;

    #endregion

    #region Miscellaneous Methods

    protected void UpdateSelectedCategory(int selectedCategory, int increment)
    {   }

    protected bool TriggerInput(int selectedCategory, int max)
    {
        bool hasInput;
        (this.selectedCategory, hasInput) = input.GetInput("Trigger", TestInput.Axis.Horizontal, max, selectedCategory);

        return hasInput;
    }

    protected virtual void GetInput()
    {
        if (Input.GetAxisRaw("Trigger") == 0)
        {
            bool hasInput;
            (selectedValue, hasInput) = input.GetInput("Horizontal", "Vertical", 10, selectedValue, true, 1, 7); // TODO: Change max value.
            if (hasInput)
            {
                UpdateSelectedValue(selectedValue);
            }
        }
        else
        {
            bool hasInput = TriggerInput(selectedValue, 10); // TODO: Change max value.
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
            bool hasInput = TriggerInput(selectedValue, 10); // TODO: Change max value.
            if (hasInput)
            {
                UpdateSelectedCategory(selectedCategory, (int)Input.GetAxisRaw("Trigger"));
            }
        }
    }

    #endregion
}
