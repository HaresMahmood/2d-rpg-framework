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

    Scrollbar scrollbar;

    #endregion

    #region Miscellaneous Methods

    protected override IEnumerator SetActive(bool isActive, int selectedSlot)
    {
        StartCoroutine(base.SetActive(isActive, selectedSlot));
        informationSlots[selectedSlot].SetActive(isActive);
        selectedSlot = this.selectedSlot = 0;
        yield return null; yield return null;
        UpdateScrollbar();
    }

    public override void InitializePanel()
    {
        UpdateMoveInformation();

        informationSlots = RemoveInactiveObjects(informationSlots);
        GetComponent<CanvasGroup>().alpha = 0f;
    }

    public void AnimatePanel(bool isActive, float duration = 0.25f)
    {
        float opacity = isActive ? 0.7f : 0f;

        UpdateScrollbar();

        Vector3 position = isActive ? new Vector3(transform.position.x + 400, transform.position.y) : new Vector3(transform.position.x - 400, transform.position.y);
        StartCoroutine(transform.LerpPosition(position, duration));
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
        Pokemon.LearnedMove move = base.GetMoves(selectedMember)[selectedMove];
        Pokemon.LearnedMove learnedMove = GetMoves(selectedMember)[selectedLearnedMove];

        base.GetMoves(selectedMember)[selectedMove] = learnedMove;
        GetMoves(selectedMember)[selectedLearnedMove] = move;
    }

    protected override void GetInput()
    {
        RegularInput();

        if (Input.GetButtonDown("Interact"))
        {
            SwapMove(PartyManager.instance.selectedMember, base.selectedSlot, selectedSlot);
            UpdateMoveInformation();
            FindObjectOfType<PartyMovePanel>().UpdateMoveInformation();
            SetActive(false);
            FindObjectOfType<PartyMovePanel>().SetActive(true);
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

        scrollbar = GetComponentInChildren<Scrollbar>();
    }

    #endregion
}
