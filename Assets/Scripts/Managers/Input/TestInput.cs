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

    public enum Axis
    {
        Horizontal,
        Vertical
    }

    #endregion

    public bool HasInput()
    {
        return hasInput;
    }

    public void SetInput(bool input)
    {
        hasInput = input;
    }

    #region Get Input

    public (int selectedButton, bool hasInput) GetInput(string button, Axis axis, int totalButtons, int selectedButton, int step = 1)
    {
        if (Input.GetAxisRaw(button) != 0)
        {
            if (!HasInput())
            {
                if (axis == Axis.Horizontal)
                {
                    selectedButton = ExtensionMethods.IncrementCircularInt(selectedButton, totalButtons, ((int)Input.GetAxisRaw(button) * step));
                }
                else if (axis == Axis.Vertical)
                {
                    selectedButton = ExtensionMethods.IncrementCircularInt(selectedButton, totalButtons, -((int)Input.GetAxisRaw(button) * step));
                }
                SetInput(true);
            }
        }
        else
        {
            SetInput(false);
        }

        OnUserInput?.Invoke(this, EventArgs.Empty);
        return (selectedButton: selectedButton, hasInput: HasInput());
    }

    public (int selectedButton, bool hasInput) GetInput(string horizontal, string vertical, int totalButtons, int selectedButton, int horizontalStep = 1, int verticalStep = 1)
    {
        if (Input.GetAxisRaw(horizontal) != 0)
        {
            if (!HasInput())
            {
                selectedButton = ExtensionMethods.IncrementCircularInt(selectedButton, totalButtons, ((int)Input.GetAxisRaw(horizontal) * horizontalStep));
                SetInput(true);
            }
        }
        else if (Input.GetAxisRaw(vertical) != 0)
        {
            if (!HasInput())
            {
                selectedButton = ExtensionMethods.IncrementCircularInt(selectedButton, totalButtons, -((int)Input.GetAxisRaw(vertical) * verticalStep));
                SetInput(true);
            }
        }
        else
        {
            SetInput(false);
        }

        OnUserInput?.Invoke(this, EventArgs.Empty);
        return (selectedButton: selectedButton, hasInput: HasInput());
    }

    #endregion
}
