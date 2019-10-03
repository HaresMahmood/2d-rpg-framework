using UnityEngine;
using TMPro;

public abstract class InteractableObject : MonoBehaviour
{

    protected virtual void Update()
    {
        if (CanInteract() && !DialogManager.instance.isActive)
            SetContextVisible();
        else
            SetContextInvisible();
    }

    protected virtual bool CanInteract(bool trigger = true)
    {
        bool hasOrientation = InteractionHandler.hasOrientation();
        if (hasOrientation && trigger)
            return true;

        return false;
    }

    protected virtual void ToggleAutoAdvance()
    {
        if (DialogManager.instance.isActive || DialogManager.instance.isTyping)
            DialogManager.instance.autoAdvance = !DialogManager.instance.autoAdvance; // Toggles autoAdvance bool.
    }

    protected virtual void SkipDialog()
    {
        if (DialogManager.instance.hasBranchingDialog)
            BranchingDialogManager.instance.SkipChoice();

        DialogManager.instance.EndDialog();
    }

    protected virtual void SetContextVisible()
    {
        PlayerInteraction.SetVisible();
    }

    protected virtual void SetContextInvisible()
    {
        StartCoroutine(PlayerInteraction.SetInvisible());
    }

    protected virtual void SetContextText(string text)
    {
        PlayerInteraction.contextBox.GetComponentInChildren<TextMeshProUGUI>().SetText(text);
    }
}