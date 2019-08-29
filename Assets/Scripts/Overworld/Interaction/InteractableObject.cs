using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    public static bool orientation = false;
    protected virtual bool CanInteract(bool trigger)
    {
        if (orientation && trigger)
            return true;

        return false;
    }
}