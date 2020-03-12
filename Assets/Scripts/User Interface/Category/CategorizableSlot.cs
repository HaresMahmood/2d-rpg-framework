﻿using System.Collections;
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
            bool isActive = opacity == 1f ? true : false;

            slot.GetComponent<CanvasGroup>().alpha = opacity;
            slot.gameObject.SetActive(isActive);
            
        }
    }

    public void UpdateInformation(Categorizable categorizable, float duration = -1)
    {
        SetInformation(categorizable);

        AnimateSlot(1f, duration);
    }

    protected virtual void SetInformation(Categorizable categorizable)
    {   }

    private IEnumerator FadeOpacity(float opacity, float duration)
    {
        if (opacity == 1f)
        {
            slot.gameObject.SetActive(true);
        }

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
