using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
///
/// </summary>
public class BranchingDialogUserInterface : UserInterface
{
    #region Constants

    public override int MaxObjects => buttons.Count;

    #endregion

    #region Variables

    private List<MenuButton> buttons;

    #endregion

    #region Miscellaneous Methods

    public void FadeButtons(bool isActive)
    {
        int start = 0; int end = buttons.Count;
        float opacity = isActive ? 1f : 0f;

        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i <= buttons.Count; i++)
        {
            var timeOffset = Mathf.Lerp(0, 1, (i - start) / (float)(end - start + 1));
            var charSequence = DOTween.Sequence();
            charSequence.Append(buttons[i].GetComponent<CanvasGroup>().DOFade(0, 0.5f).From()).SetEase(Ease.Linear);
            sequence.Insert(timeOffset, charSequence);
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        buttons = GetComponentsInChildren<MenuButton>().ToList();   
    }

    #endregion
}

