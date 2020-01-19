using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// </summary>
public class PartyLearnedMovePanel : PartyMovePanel
{
    #region Variables

    Animator animator;
    Scrollbar scrollbar;

    #endregion

    #region Miscellaneous Methods

    protected override IEnumerator SetActive(bool isActive, int selectedSlot)
    {
        StartCoroutine(base.SetActive(isActive, selectedSlot));
        informationSlots[selectedSlot].SetActive(isActive);
        this.selectedSlot = 0;
        yield return null; yield return null;
        UpdateScrollbar();
    }

    public override void InitializePanel()
    {
        UpdateMoveInformation();

        informationSlots = RemoveInactiveObjects(informationSlots);
        GetComponent<CanvasGroup>().alpha = 0f;
    }
    private IEnumerator DectivatePanel()
    {
        SwapMove(PartyManager.instance.selectedMember, FindObjectOfType<PartyMovePanel>().selectedSlot, selectedSlot);
        UpdateMoveInformation();
        FindObjectOfType<PartyMovePanel>().UpdateMoveInformation();
        FindObjectOfType<PartyMovePanel>().SetActive(true);
        yield return null;
        SetActive(false);
    }

    public void AnimatePanel(bool isActive, float duration = 0.2f)
    {
        float opacity = isActive ? 0.7f : 0f;

        UpdateScrollbar();
        animator.SetBool("isActive", isActive);
        StartCoroutine(gameObject.FadeOpacity(opacity, duration));
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

    protected override void GetInput()
    {
        RegularInput();

        if (Input.GetButtonDown("Interact"))
        {
            StartCoroutine(DectivatePanel());
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

        animator = GetComponent<Animator>();
        scrollbar = GetComponentInChildren<Scrollbar>();
    }

    #endregion
}
