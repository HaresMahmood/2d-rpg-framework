using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class ItemInformationController : UserInterfaceController
{
    #region Fields

    [SerializeField] private ItemInformationUserInterface userInterface;

    #endregion

    #region Properties

    protected override UserInterface UserInterface
    {
        get { return userInterface; }
    }

    #endregion

    #region Variables



    #endregion

    #region Miscellaneous Methods

    protected override void UpdateSelectedObject(int selectedValue, int increment = -1)
    {
        UserInterface.UpdateSelectedObject(selectedValue, increment);
    }

    protected override void GetInput(string axisName)
    {
        bool hasInput = RegularInput(selectedValue, UserInterface.MaxObjects, axisName);
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
    protected override void Update()
    {
        if (Flags.isActive)
        {
            GetInput("Horizontal");
        }
    }

    #endregion
}
