using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class MenuContextButton : MenuButton
{
    #region Variables

    private Image sprite;
    private Animator animator;

    private TextMeshProUGUI text;
    private AutoTextWidth textWidth;

    #endregion

    #region Miscellaneous Methods

    public void SetValues(string value, Sprite icon, bool isAnimated)
    {
        sprite.sprite = icon;
        text.SetText(value);

        textWidth.UpdateWidth(value);
    }

    public void AnimateButton(string value) // , int waitTimeMultiplier = 7
    {
        animator.gameObject.SetActive(false);

        animator.GetComponent<TextMeshProUGUI>().SetText(value);

        animator.gameObject.SetActive(true);

        //yield return new WaitForSecondsRealtime(animator.GetAnimationTime() * waitTimeMultiplier);

        //animator.SetBool("isToggling", false); yield return null;

        //yield return new WaitForSecondsRealtime(animator.GetAnimationTime());

        //animator.gameObject.SetActive(false);
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        sprite = transform.Find("Button").GetComponent<Image>();
        text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        animator = transform.Find("Text/Toggle Value").GetComponent<Animator>();
        textWidth = text.GetComponent<AutoTextWidth>();

        animator.gameObject.SetActive(false);
    }

    #endregion
}

