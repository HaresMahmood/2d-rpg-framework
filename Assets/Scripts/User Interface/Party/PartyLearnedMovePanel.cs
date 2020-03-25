using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// </summary>
public class PartyLearnedMovePanel : PartyMovesPanel
{
    /*
    #region Variables

    PartyMovePanel movePanel;
    Animator animator;
    Scrollbar scrollbar;

    #endregion

    #region Miscellaneous Methods
    protected override IEnumerator SetActive(bool isActive, int selectedSlot)
    {
        StartCoroutine(base.SetActive(isActive, selectedSlot));
        yield return null;
        UpdateScrollbar(this.selectedSlot);
    }

    public override void InitializePanel()
    {
        base.InitializePanel();

        informationSlots = RemoveInactiveObjects(informationSlots);
        GetComponent<CanvasGroup>().alpha = 0f;
    }

    private IEnumerator DeactivatePanel()
    {
        movePanel.SetActive(true);
        yield return null;
        SetActive(false);
    }

    private void UpdateMoveLists()
    {
        SwapMove(PartyManager.instance.selectedMember, movePanel.selectedSlot, selectedSlot);
        UpdateMoveInformation();
        movePanel.UpdateMoveInformation();
    }

    public void AnimatePanel(bool isActive)
    {
        UpdateScrollbar(selectedSlot);
        informationSlots[selectedSlot].AnimateSlot(!isActive);
        animator.SetBool("isActive", isActive);
    }

    protected override IEnumerator AnimateSlot(int selectedSlot, int increment)
    {
        StartCoroutine(base.AnimateSlot(selectedSlot, increment));
        yield return null;
        UpdateScrollbar(selectedSlot);
    }

    private void UpdateScrollbar(int selectedSlot = -1)
    {
        if (selectedSlot > -1)
        {
            float totalMoves = (float)informationSlots.Length;
            float targetValue = 1.0f - (float)selectedSlot / (totalMoves - 1);
            StartCoroutine(scrollbar.LerpScrollbar(targetValue, 0.08f));
        }
        else
        {
            scrollbar.value = 1;
        }
    }

    protected override List<Pokemon.LearnedMove> GetMoves(int selectedMember)
    {
        Pokemon member = PartyManager.instance.party.playerParty[selectedMember];

        return member.learnedMoves;
    }

    private void SwapMove(int selectedMember, int selectedMove, int selectedLearnedMove)
    {
        Pokemon member = PartyManager.instance.party.playerParty[selectedMember];

        Pokemon.LearnedMove move = base.GetMoves(selectedMember)[selectedMove];
        Pokemon.LearnedMove learnedMove = GetMoves(selectedMember)[selectedLearnedMove];

        member.activeMoves[selectedMove] = learnedMove;
        member.learnedMoves[selectedLearnedMove] = move;
    }

    // TODO: Debug.
    private T FindBaseObjectOfType<T>() where T : Object
    {
        T destination = default;

        foreach (T child in FindObjectsOfType<T>())
        {
            if (!child.GetType().IsSubclassOf(typeof(T)))
            {
                destination = child;
            }
        }

        return destination;
    }

    protected override void GetInput()
    {
        RegularInput();

        if (Input.GetButtonDown("Interact"))
        {
            UpdateMoveLists();
            StartCoroutine(DeactivatePanel());
        }

        if (Input.GetButtonDown("Cancel"))
        {
            StartCoroutine(DeactivatePanel());
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    protected override void Start()
    {
        base.Start();

        movePanel = FindBaseObjectOfType<PartyMovePanel>();
        animator = GetComponent<Animator>();
        scrollbar = GetComponentInChildren<Scrollbar>();
    }

    #endregion
    */
}
