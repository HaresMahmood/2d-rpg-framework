using System.Collections;
using UnityEngine;

/// <summary>
///
/// </summary>
public abstract class CategorizableSlot : MonoBehaviour
{
    #region Variables

    protected Transform slot;

    #endregion

    #region Miscellaneous Methods

    public void AnimateSlot(float opacity, float duration = -1)
    {
        if (duration > -1)
        {
            StartCoroutine(FadeOpacity(opacity, duration));
        }
        else
        {
            slot.GetComponent<CanvasGroup>().alpha = opacity;
        }
    }

    public void UpdateInformation(Categorizable categorizable, float duration = -1)
    {
        if (!slot.gameObject.activeSelf)
        {
            slot.gameObject.SetActive(true);
        }

        SetInformation(categorizable);

        AnimateSlot(1f, duration);
    }

    protected virtual void SetInformation(Categorizable categorizable)
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
