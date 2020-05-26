using System.Collections;
using UnityEngine;

public class StatsController : PartyInformationController
{
    #region Miscellaneous Methods

    public override IEnumerator SetActive(bool isActive, bool condition = true)
    {
        yield return null;

        Flags.isActive = isActive;

        userInterface.ActivateSlot(0, isActive);
    }

    protected override void GetInput(string axisName)
    {
        bool hasInput = RegularInput(UserInterface.MaxObjects, axisName);
        if (hasInput)
        {
            //UpdateSelectedObject(0, (int)Input.GetAxisRaw(axisName));
        }
    }

    /*
    protected override void InteractInput(int selectedValue)
    {
        base.InteractInput(selectedValue);

        ((InventoryUserInterface)UserInterface).ActiveSubMenu(selectedValue);
    }
    
    protected override void GetInput()
    {
        base.GetInput();

        if (Input.GetButtonDown("Interact"))
        {
            InteractInput(selectedValue);
        }
    }
    */

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
    }

    /*
    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    protected override void Update()
    {
        if (Flags.isActive)
        {
            GetInput();
        }
    }
    */

    #endregion
}
