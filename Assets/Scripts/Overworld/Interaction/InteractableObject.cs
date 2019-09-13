﻿using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    protected virtual bool CanInteract(bool trigger)
    {
        bool hasOrientation = InteractionHandler.hasOrientation();
        if (hasOrientation && trigger)
            return true;

        return false;
    }

    protected virtual void SkipDialog()
    {
        if (DialogManager.instance.hasBranchingDialog)
            ChoiceManager.instance.SkipChoice();

        DialogManager.instance.EndDialog();
    }
}