using System;
using UnityEngine;

/// <summary>
///
/// </summary>
public class TestInput
{
    #region Variables

    public event EventHandler OnUserInput = delegate { };
    public bool hasInput = false;

    #endregion

    #region Enums

    public enum Axis
    {
        Horizontal,
        Vertical
    }

    #endregion

    #region Accessor Methods

    public bool HasInput()
    {
        return hasInput;
    }

    #endregion

    #region Helper Methods

    public void SetInput(bool input)
    {
        hasInput = input;
    }

    #endregion

    #region Get Input

    public (int selectedButton, bool hasInput) GetInput(string button, Axis axis, int totalButtons, int selectedButton, bool isBounded = false, int step = 1)
    {
        int startValue = selectedButton;
        bool hasInput = false;

        if (Input.GetAxisRaw(button) != 0)
        {
            if (!HasInput())
            {
                if (axis == Axis.Horizontal)
                {

                    selectedButton = ExtensionMethods.IncrementInt(selectedButton, 0, totalButtons, ((int)Input.GetAxisRaw(button) * step), isBounded);
                }
                else if (axis == Axis.Vertical)
                {
                    selectedButton = ExtensionMethods.IncrementInt(selectedButton, 0, totalButtons, -((int)Input.GetAxisRaw(button) * step), isBounded);
                }
                SetInput(true);
            }
        }
        else
        {
            SetInput(false);
        }

        OnUserInput?.Invoke(this, EventArgs.Empty);

        if (startValue != selectedButton) hasInput = true;
        return (selectedButton, hasInput);
    }

    public (int selectedButton, bool hasInput) GetInput(string horizontal, string vertical, int totalButtons, int selectedButton, bool isBounded = false, int horizontalStep = 1, int verticalStep = 1)
    {
        int startValue = selectedButton;
        bool hasInput = false;

        if (Input.GetAxisRaw(horizontal) != 0)
        {
            if (!HasInput())
            {
                selectedButton = ExtensionMethods.IncrementInt(selectedButton, 0, totalButtons, ((int)Input.GetAxisRaw(horizontal) * horizontalStep), isBounded);
                SetInput(true);
            }
        }
        else if (Input.GetAxisRaw(vertical) != 0)
        {
            if (!HasInput())
            {
                selectedButton = ExtensionMethods.IncrementInt(selectedButton, 0, totalButtons, -((int)Input.GetAxisRaw(vertical) * verticalStep), isBounded);
                SetInput(true);
            }
        }
        else
        {
            SetInput(false);
        }

        OnUserInput?.Invoke(this, EventArgs.Empty);

        if (startValue != selectedButton) hasInput = true;
        return (selectedButton, hasInput);
    }

    #endregion
}
