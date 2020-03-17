using System.Collections;
using UnityEngine;

/// <summary>
///
/// </summary>
public abstract class CategorizableSlot : Slot
{
    #region Variables

    protected Transform slot;

    #endregion

    #region Miscellaneous Methods

    public override void AnimateSlot(float opacity, float duration = -1)
    {
        if (duration > -1)
        {
            if (opacity == 1f)
            {
                slot.gameObject.SetActive(true);
            }

            StartCoroutine(FadeOpacity(opacity, duration));
        }
        else
        {
            bool isActive = opacity == 1f ? true : false;

            slot.GetComponent<CanvasGroup>().alpha = opacity;
            slot.gameObject.SetActive(isActive);
            
        }
    }

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
}
