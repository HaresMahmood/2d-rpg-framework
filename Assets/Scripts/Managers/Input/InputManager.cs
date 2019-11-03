using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class InputManager : MonoBehaviour
{
    #region Variables

    private static bool hasInput = false;

    public enum Axis
    {
        Horizontal,
        Vertical
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        
    }

    #endregion

    public static int GetInput(string button, Axis axis, int totalButtons, int selectedButton, int step = 1)
    {
        if (Input.GetAxisRaw(button) != 0)
        {
            if (!hasInput)
            {
                if (axis == Axis.Horizontal)
                {
                    if (Input.GetAxisRaw(button) > 0)
                    {
                        if (selectedButton + (step - 1) < totalButtons)
                        {
                            selectedButton += step;
                        }
                        else
                        {
                            selectedButton = 0;
                        }
                    }
                    else if (Input.GetAxisRaw(button) < 0)
                    {
                        if (selectedButton - (step - 1) > 0)
                        {
                            selectedButton -= step;
                        }
                        else
                        {
                            selectedButton = totalButtons;
                        }
                    }
                }
                else if (axis == Axis.Vertical)
                {
                    if (Input.GetAxisRaw(button) < 0)
                    {
                        if (selectedButton + (step - 1) < totalButtons)
                        {
                            selectedButton += step;
                        }
                        else
                        {
                            selectedButton = 0;
                        }
                    }
                    else if (Input.GetAxisRaw(button) > 0)
                    {
                        if (selectedButton - (step - 1) > 0)
                        {
                            selectedButton -= step;
                        }
                        else
                        {
                            selectedButton = totalButtons;
                        }
                    }
                }
                hasInput = true;
            }
        }
        else
        {
            hasInput = false;
        }

        return selectedButton;
    }

    public static int GetInput(string horizontal, string vertical, int totalButtons, int selectedButton, int horizontalStep = 1, int verticalStep = 1)
    {
        if (Input.GetAxisRaw(horizontal) != 0)
        {
            if (!hasInput)
            {
                if (Input.GetAxisRaw(horizontal) > 0)
                {
                    if (selectedButton + (horizontalStep - 1) < totalButtons)
                    {
                        selectedButton += horizontalStep;
                    }
                    else
                    {
                        selectedButton = 0;
                    }
                }
                else if (Input.GetAxisRaw(horizontal) < 0)
                {
                    if (selectedButton - (horizontalStep - 1) > 0)
                    {
                        selectedButton -= horizontalStep;
                    }
                    else
                    {
                        selectedButton = totalButtons;
                    }
                }
                hasInput = true;
            }
        }
        else if (Input.GetAxisRaw(vertical) != 0)
        {
            if (!hasInput)
            {
                if (Input.GetAxisRaw(vertical) < 0)
                {
                    if (selectedButton + (verticalStep - 1) < totalButtons)
                    {
                        selectedButton += verticalStep;
                    }
                    else
                    {
                        selectedButton = 0;
                    }
                }
                else if (Input.GetAxisRaw(vertical) > 0)
                {
                    if (selectedButton - (verticalStep - 1) > 0)
                    {
                        selectedButton -= verticalStep;
                    }
                    else
                    {
                        selectedButton = totalButtons;
                    }
                }
                hasInput = true;
            }
        }
        else
        {
            hasInput = false;
        }

        return selectedButton;
    }
}
