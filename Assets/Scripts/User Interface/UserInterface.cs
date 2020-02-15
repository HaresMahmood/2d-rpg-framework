using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// </summary>
public class UserInterface : MonoBehaviour
{
    #region Variables

    protected GameObject selector;

    protected Animator selectorAnimator;

    protected Scrollbar scrollbar;

    #endregion

    #region Miscellaneous Methods

    protected virtual void UpdateSelectedObject(int selectedValue)
    {   }

    /// <summary>
    /// Animates and updates the position of the selector. Dynamically changes position and size of selector 
    /// depending on what situation it is used for. If no value is selected, the indicator completely fades out.
    /// </summary>
    /// <param name="objectSlots"> List of buttons at with the selector can be positioned. </param>
    /// <param name="selectedValue"> Index of the value currently selected. </param>
    /// <param name="animationDuration"> Duration of the animation/fade. </param>
    /// <returns> Co-routine. </returns>
    protected IEnumerator UpdateSelector(Transform[] objectSlots, int selectedValue = -1, float animationDuration = 0.1f)
    {
        selectorAnimator.enabled = false;
        StartCoroutine(selector.FadeOpacity(0f, animationDuration));

        if (selectedValue > -1)
        {
            yield return new WaitForSecondsRealtime(animationDuration);

            selector.transform.position = objectSlots[selectedValue].transform.position;
            selectorAnimator.enabled = true;
        }
    }

    protected void UpdateScrollbar(int objectsAmount, int selectedValue = -1, float animationDuration = 0.08f)
    {
        if (selectedValue > -1)
        {
            float totalSlots = objectsAmount;
            float targetValue = 1.0f - selectedValue / (totalSlots - 1);
            StartCoroutine(scrollbar.LerpScrollbar(targetValue, animationDuration));
        }
        else
        {
            scrollbar.value = 1;
        }
    }

    #endregion
}
