﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class PartyMovesPanelController : PartyInformationController
{
    #region Properties

    // TODO: bad name
    public bool IsActive { get; set; }

    #endregion

    #region Miscellaneous Methods

    public override IEnumerator SetActive(bool isActive, bool condition = true)
    {
        StartCoroutine(base.SetActive(isActive));

        yield return null;

        if (!condition)
        {
            float opacity = IsActive ? 0.4f : 1f;

            PartyController.Instance.AnimatePanels(this, opacity);
            PartyController.Instance.UpdateSelector(IsActive);
        }
    }

    protected override void GetInput(string axisName)
    {
        if (IsActive)
        {
            GetInput();
        }
        else
        {
            base.GetInput(axisName);
        }

        if (Input.GetButtonDown("Interact"))
        {
            IsActive = !IsActive;
        }
        else if (Input.GetButtonDown("Cancel") && IsActive)
        {
            IsActive = false;
        }

        if (Input.GetButtonDown("Interact") || Input.GetButtonDown("Cancel"))
        {
            float opacity = IsActive ? 0.4f : 1f;

            PartyController.Instance.AnimatePanels(this, opacity);
            PartyController.Instance.UpdateSelector(IsActive);
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

