using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public abstract class UserInterfaceController : MonoBehaviour
{
    #region Fields

    private UserInterface userInterface;

    private UserInterfaceFlags flags;

    #endregion

    #region Properties

    protected virtual UserInterface UserInterface
    {
        get { return userInterface; }
    }

    public virtual UserInterfaceFlags Flags
    {
        get { return flags; }
    }

    #endregion

    #region Variables

    public static UserInterfaceController instance;

    [Header("Setup")]
    public List<PanelButton> buttons = new List<PanelButton>();

    protected readonly TestInput input = new TestInput();

    protected int selectedValue = 0;

    #endregion

    #region Nested Classes

    public abstract class UserInterfaceFlags
    {
        public bool isActive { get; set; }

        protected UserInterfaceFlags(bool isActive)
        {
            this.isActive = isActive;
        }
    }

    #endregion

    #region Miscellaneous Methods

    public virtual void OnPause(bool isPaused)
    {   }

    protected virtual void UpdateSelectedObject(int selectedValue)
    {
        UserInterface.UpdateSelectedObject(selectedValue);
    }

    protected virtual bool RegularInput(int selectedSlot, int max, string axisName)
    {
        TestInput.Axis axis = axisName == "Horizontal" ? TestInput.Axis.Horizontal : TestInput.Axis.Vertical;

        bool hasInput;
        (selectedValue, hasInput) = input.GetInput(axisName, axis, max, selectedSlot);

        return hasInput;
    }

    protected virtual void GetInput(string axisName)
    {
        bool hasInput = RegularInput(selectedValue, UserInterface.MaxObjects, axisName); // TODO: Change max value.
        if (hasInput)
        {
            UpdateSelectedObject(selectedValue);
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected virtual void Awake()
    {
        if (instance == null)
            instance = this;
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    protected virtual void Update()
    {
        if (Flags.isActive)
        {
            GetInput("Vertical");
        }
    }

    #endregion
}
