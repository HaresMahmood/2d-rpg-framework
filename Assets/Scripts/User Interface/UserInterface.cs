using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// </summary>
public abstract class UserInterface : MonoBehaviour
{
    #region Constants

    public abstract int MaxObjects { get; }

    #endregion

    #region Variables

    protected GameObject selector;

    protected Animator selectorAnimator;

    protected Scrollbar scrollbar;

    #endregion

    #region Miscellaneous Methods

    public IEnumerator AnimateSelector()
    {
        float animationDuration;

        selectorAnimator.SetBool("isPressed", true);

        yield return null;
        animationDuration = selectorAnimator.GetAnimationTime();

        if (Time.timeScale == 0)
        {
            yield return new WaitForSecondsRealtime(animationDuration);
        }
        else
        {
            yield return new WaitForSeconds(animationDuration);
        }

        selectorAnimator.SetBool("isPressed", false);
        selector.gameObject.SetActive(false);
    }

    public virtual void UpdateSelectedObject(int selectedValue, int increment = -1)
    {   }

    /// <summary>
    /// Animates and updates the position of the selector. Dynamically changes position and size of selector 
    /// depending on what situation it is used for. If no value is selected, the indicator completely fades out.
    /// </summary>
    /// <param name="objectSlots"> List of buttons at with the selector can be positioned. </param>
    /// <param name="selectedValue"> Index of the value currently selected. </param>
    /// <param name="animationDuration"> Duration of the animation/fade. </param>
    /// <returns> Co-routine. </returns>
    protected IEnumerator UpdateSelector(Transform selectedObject = null, float animationDuration = 0.1f)
    {
        if (!selector.activeSelf)
        {
            selector.SetActive(true);
        }

        selectorAnimator.enabled = false;
        StartCoroutine(selector.FadeOpacity(0f, animationDuration));

        if (selectedObject != null)
        {
            yield return new WaitForSecondsRealtime(animationDuration);

            selector.transform.position = selectedObject.position;
            selectorAnimator.enabled = true;
        }
    }

    protected virtual void UpdateScrollbar(int maxObjects = -1, int selectedValue = -1, float animationDuration = 0.08f)
    {
        if (maxObjects > -1)
        {
            if (scrollbar.GetComponent<CanvasGroup>().alpha != 1f)
            {
                StartCoroutine(scrollbar.gameObject.FadeOpacity(1f, animationDuration));
            }

            if (selectedValue > -1)
            {
                float totalSlots = maxObjects;
                float targetValue = (float)Math.Round(1.0f - (selectedValue / (totalSlots - 1)), 1);

                targetValue = (selectedValue == 0) ? 1 : targetValue;

                StartCoroutine(scrollbar.LerpScrollbar(targetValue, animationDuration));
            }
            else
            {
                scrollbar.value = 1;
            }
        }
        else
        {
            if (scrollbar.GetComponent<CanvasGroup>().alpha != 0f)
            {
                StartCoroutine(scrollbar.gameObject.FadeOpacity(0f, animationDuration));
            }
        }
    }

    protected void FadeUserInterface(GameObject panel, float opacity, float animationDuration = 0.15f)
    {
        StartCoroutine(panel.FadeOpacity(opacity, animationDuration));
    }

    protected void FadeCharacterSprite(float opacity, float animationDuration = 0.15f)
    {
        FindObjectOfType<CharacterSpriteController>().FadeSprite(opacity, animationDuration); // TODO: Bad performance, think of different way.
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected virtual void Awake()
    {
        selectorAnimator = selector.GetComponent<Animator>();
    }

    #endregion
}
