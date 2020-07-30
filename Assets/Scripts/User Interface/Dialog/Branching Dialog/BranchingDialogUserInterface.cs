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

        UpdateSelectorPosition(increment == 0 ? null : buttons[selectedValue].transform);

        /*
        if (increment != 0)
        {
            UpdateSelectorPosition(buttons[selectedValue].transform); // increment == 0 ? null : 
        }
        else
        {
            DeactivateSelector();
        }
        */
    }

    // TODO: Debug
    public void FadeButtons(bool isActive, List<BranchingDialog.DialogBranch> branches)
    {
        float opacity = isActive ? 1f : 0f;

        max = branches.Count;

        Sequence sequence = DOTween.Sequence();

        if (isActive && branches.Count != buttons.Count)
        {
            for (int i = branches.Count; i < buttons.Count; i++)
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
        else if (!isActive)
        {
            UpdateSelectedObject(0, 0);
        }

        //yield return null;

        for (int i = 0; i < branches.Count; i++)
        {
            float timeOffset = i * 0.08f;
            var buttonSequence = DOTween.Sequence();

            buttons[i].SetValues(branches[i].Text, null);

            buttonSequence.Append(buttons[i].GetComponent<CanvasGroup>().DOFade(opacity, 0.1f));
            sequence.Insert(timeOffset, buttonSequence);
        }

        sequence.OnComplete(() =>
        {
            if (!isActive)
            {
                for (int i = 0; i < buttons.Count; i++)
                {
                    buttons[i].gameObject.SetActive(true);
                }
            }
            else
            {
                UpdateSelectedObject(0, 1);
            }
        });
    }

    public void InvokeButton(BranchingDialog.DialogBranch branch)
    {
        Debug.Log("Invoked " + branch.Text);

        if (branch.BranchEvent != null)
        {
            branch.BranchEvent.Invoke();
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

        selector = transform.Find("Selector").GetComponent<SelectorController>();

    }

    private void Start()
    {
        UpdateSelectorPosition();
    }

    #endregion
}

