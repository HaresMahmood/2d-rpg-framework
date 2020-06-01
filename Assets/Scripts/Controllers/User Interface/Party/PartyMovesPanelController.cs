using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class PartyMovesPanelController : PartyInformationController
{
    #region Variables

    private bool isActive; // TODO: Bad name

    #endregion

    #region Miscellaneous Methods

    protected override void GetInput(string axisName)
    {
        if (isActive)
        {
            GetInput();
        }
        else
        {
            base.GetInput(axisName);
        }

        if (Input.GetButtonDown("Interact"))
        {
            isActive = !isActive;
        }
        else if (Input.GetButtonDown("Cancel") && isActive)
        {
            isActive = false;
        }
    }

    private void GetInput()
    {
        bool hasInput = RegularInput(UserInterface.MaxObjects, "Vertical");
        if (hasInput)
        {
            ((PartyMovesPanel)userInterface).UpdatePosition(PartyController.Instance.party.playerParty[0], selectedValue, (int)Input.GetAxisRaw("Vertical"));
        }
    }

    #endregion
}

