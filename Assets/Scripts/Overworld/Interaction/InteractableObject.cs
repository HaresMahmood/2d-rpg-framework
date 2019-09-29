using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    protected virtual bool CanInteract(Vector2 position, bool trigger = true)
    {
        bool hasOrientation = InteractionHandler.hasOrientation(position);
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
            ChoiceManager.instance.SkipChoice();

        DialogManager.instance.EndDialog();
    }
}