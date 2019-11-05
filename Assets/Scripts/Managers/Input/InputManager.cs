using System;
using UnityEngine;

/// <summary>
///
/// </summary>
public class InputManager : MonoBehaviour
{
    #region Variables

    public static InputManager instance;

    public event EventHandler OnUserInput = delegate {};
    public bool hasInput = false;

    public enum Axis
    {
        Horizontal,
        Vertical
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

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
        //Debug.Log(HasInput());
    }

    #endregion

    public static bool HasInput()
    {
        return instance.hasInput;
    }

    public static void SetInput(bool input)
    {
        instance.hasInput = input;
    }

    #region Get Input

    public static (int selectedButton, bool hasInput) GetInput(string button, Axis axis, int totalButtons, int selectedButton, int step = 1)
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

        instance.OnUserInput?.Invoke(instance, EventArgs.Empty);
        return (selectedButton: selectedButton, hasInput: HasInput());
    }

    public static (int selectedButton, bool hasInput) GetInput(string horizontal, string vertical, int totalButtons, int selectedButton, int horizontalStep = 1, int verticalStep = 1)
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

        instance.OnUserInput?.Invoke(instance, EventArgs.Empty);
        return (selectedButton: selectedButton, hasInput: HasInput());
    }

    #endregion
}
