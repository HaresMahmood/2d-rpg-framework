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

    public override int MaxObjects => max;

    #endregion

    #region Variables

    private List<MenuButton> buttons;

    private int max;

    #endregion

    #region Miscellaneous Methods

    public override void UpdateSelectedObject(int selectedValue, int increment = -1)
    {
        int previousValue = ExtensionMethods.IncrementInt(selectedValue, 0, MaxObjects, -increment);

        buttons[selectedValue].AnimateButton(true);
        buttons[previousValue].AnimateButton(false);

        StartCoroutine(UpdateSelector(increment == 0 ? null : buttons[selectedValue].transform));
    }

    public void FadeButtons(bool isActive, List<BranchingDialog.DialogBranch> branch)
    {
        float opacity = isActive ? 1f : 0f;

        max = branch.Count;

        Sequence sequence = DOTween.Sequence();

        if (isActive && branch.Count != buttons.Count)
        {
            for (int i = branch.Count; i < buttons.Count; i++)
            {
                buttons[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < branch.Count; i++)
        {
            //float timeOffset = Mathf.Lerp(0, 1, (i - start) / (float)(end - start));
            float timeOffset = i * 0.08f;
            var buttonSequence = DOTween.Sequence();

            buttons[i].SetValues(branch[i].Text, null);

            buttonSequence.Append(buttons[i].GetComponent<CanvasGroup>().DOFade(opacity, 0.1f));
            sequence.Insert(timeOffset, buttonSequence);
        }


        if (!isActive)
        {
            sequence.OnComplete(() =>
            {
                for (int i = 0; i < buttons.Count; i++)
                {
                    buttons[i].gameObject.SetActive(true);
                }
            });
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

        selector = transform.Find("Selector").gameObject;

        base.Awake();

        StartCoroutine(UpdateSelector());
    }

    #endregion
}

