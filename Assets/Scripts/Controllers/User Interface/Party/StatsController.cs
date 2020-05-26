using System;
using System.Collections;
using UnityEngine;

public class StatsController : PartyInformationController
{
    private bool isActive; // TODO: Turn into flag?

    #region Miscellaneous Methods

    public override IEnumerator SetActive(bool isActive, bool condition = true)
    {
        yield return null;

        Flags.isActive = isActive;

        if (this.isActive)
        {
            this.isActive = false;
            AnimatePanels(1f);
        }

        userInterface.ActivateSlot(0, isActive);
    }
    
    protected override void GetInput(string axisName)
    {
        base.GetInput(axisName);

        // TODO: Ugly code

        if (Input.GetButtonDown("Interact") && !isActive)
        {
            isActive = true;

            AnimatePanels(0.4f);
            userInterface.ActivateSlot(Convert.ToInt32(isActive), true);
        }
        else if (Input.GetButtonDown("Cancel") && isActive)
        {
            isActive = false;

            AnimatePanels(1f);
            userInterface.ActivateSlot(Convert.ToInt32(isActive), true);
        }
    }

    private void AnimatePanels(float opacity)
    {
        PartyController.Instance.AnimatePanel(0, opacity);
        PartyController.Instance.AnimatePanel(2, opacity);
    }

    #endregion

    #region Unity Methods

    /*
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
    }
    */

    #endregion
}
