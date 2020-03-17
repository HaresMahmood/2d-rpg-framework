using System.Collections;
using UnityEngine;

/// <summary>
///
/// </summary>
public abstract class Slot : MonoBehaviour
{
    #region Variables

    protected Transform slot;

    #endregion

    #region Miscellaneous Methods

    public virtual void AnimateSlot(float opacity, float duration = -1)
    {   }

    public virtual void UpdateInformation(ScriptableObject slotObject, float duration = -1)
    {
        SetInformation(slotObject);

        AnimateSlot(1f, duration);
    }

    protected virtual void SetInformation(ScriptableObject slotObject)
    {   }

    private IEnumerator FadeOpacity(float opacity, float duration)
    {
        StartCoroutine(slot.gameObject.FadeOpacity(opacity, duration));

        yield return new WaitForSecondsRealtime(duration);

        if (opacity == 0f)
        {
            slot.gameObject.SetActive(false);
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected virtual void Awake()
    {
        slot.gameObject.SetActive(false);
    }

    #endregion
}
