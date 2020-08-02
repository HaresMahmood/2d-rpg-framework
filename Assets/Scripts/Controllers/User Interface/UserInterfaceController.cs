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

    private UserInterfaceFlags flags = new UserInterfaceFlags(false);

    #endregion

    #region Properties

    public virtual UserInterface UserInterface
    {
        get { return userInterface; }
        set { }
    }

    public virtual UserInterfaceFlags Flags
    {
        get { return flags; }
    }

    #endregion

    #region Variables

    [Header("Setup")]
    public List<PanelButton> buttons = new List<PanelButton>();

    protected readonly TestInput input = new TestInput();

    protected int selectedValue = 0;

    #endregion

    #region Nested Classes

    public class UserInterfaceFlags
    {
        public bool IsActive { get; internal set; }

        internal UserInterfaceFlags(bool isActive)
        {
            IsActive = isActive;
        }
    }

    #endregion

    #region Miscellaneous Methods

    public virtual IEnumerator SetActive(bool isActive, bool condition = true)
    {
        if (isActive)
        {
            StartCoroutine(BottomPanelUserInterface.Instance.ChangePanelButtons(buttons));
        }

        yield break;
    }

    protected virtual void UpdateSelectedObject(int selectedValue, int increment = -1)
    {
        UserInterface.UpdateSelectedObject(selectedValue, increment);
    }

    protected virtual bool RegularInput(int max, string axisName)
    {
        TestInput.Axis axis = axisName.Equals("Horizontal") ? TestInput.Axis.Horizontal : TestInput.Axis.Vertical;

        bool hasInput;
        (selectedValue, hasInput) = input.GetInput(axisName, axis, max, selectedValue);

        return hasInput;
    }

    protected virtual void InteractInput(int selectedValue)
    {
        Flags.IsActive = false;
        StartCoroutine(UserInterface.AnimateSelector());
    }

    protected virtual void CancelInput(int selectedValue)
    {   }

    protected virtual void GetInput(string axisName)
    {
        bool hasInput = RegularInput(UserInterface.MaxObjects, axisName);
        if (hasInput)
        {
            UpdateSelectedObject(selectedValue, (int)Input.GetAxisRaw(axisName));
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    protected virtual void Update()
    {
        if (Flags.IsActive)
        {
            GetInput("Vertical");
        }
    }

    #endregion
}
