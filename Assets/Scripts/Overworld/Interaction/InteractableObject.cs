using TMPro;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{

    protected virtual void Update()
    {
        if (CanInteract() && !DialogManager.instance.isActive && !PauseManager.instance.isPaused)
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

    protected virtual void SetContextVisible()
    {
        PlayerInteraction.UpdatePosition();
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