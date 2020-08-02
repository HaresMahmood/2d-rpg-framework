using System;
using System.Collections;
using UnityEngine;

public class StatsController : PartyInformationController
{
    private bool isActive; // TODO: Turn into flag?

    #region Miscellaneous Methods

    public override IEnumerator SetActive(bool isActive, bool condition = true)
    {
        if (Flags.IsActive != isActive)
        {
            yield return null;

            Flags.IsActive = isActive;

            if (this.isActive)
            {
                this.isActive = false;
                AnimatePanels(1f);
            }
        }

        userInterface.ActivateSlot(0, isActive);

        CharacterSpriteController.Instance.FadeOpacity(isActive ? 0.4f : 1f, 0.15f); // TODO: Debug.

        StartCoroutine(base.SetActive(isActive, condition));
    }

    protected override void GetInput(string axisName)
    {
        base.GetInput(axisName);

        if (Input.GetButtonDown("Interact"))
        {
            isActive = !isActive;
        }
        else if (Input.GetButtonDown("Cancel") && isActive)
        {
            isActive = false;
        }
       
        if (Input.GetButtonDown("Interact") || Input.GetButtonDown("Cancel"))
        {
            float opacity = isActive ? 0.4f : 1f;

            AnimatePanels(opacity);
            userInterface.ActivateSlot(Convert.ToInt32(isActive), true);
        }
    }

    private void AnimatePanels(float opacity, float animationDuration = 0.15f)
    {
        PartyController.Instance.AnimatePanel(0, opacity, animationDuration);
        PartyController.Instance.AnimatePanel(2, opacity, animationDuration);

        CharacterSpriteController.Instance.FadeOpacity(opacity == 1f ? (Flags.IsActive ? 0.4f : 1f) : 0f, animationDuration); // TODO: Denug.
    }

    #endregion
}
