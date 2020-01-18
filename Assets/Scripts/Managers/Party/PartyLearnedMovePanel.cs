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

    /*
    public override void InitializePanel()
    {
        int counter = 0;
        List<Pokemon.LearnedMove> moves = GetMoves(PartyManager.instance.selectedMember);

        informationSlots = transform.GetComponentsInChildren<PartyInformationSlots>();

        foreach (Pokemon.LearnedMove move in moves)
        {
            informationSlots[counter].SetActive(false);
            informationSlots[counter].GetComponentInChildren<MoveSlot>().UpdateInformation(move);
            counter++;
        }

        for (int i = counter; i < informationSlots.Length; i++)
        {
            informationSlots[i].gameObject.SetActive(false);
        }

        informationSlots = RemoveInactiveObjects(informationSlots);

        informationSlots[0].SetActive(true);
    }
    */

    protected override List<Pokemon.LearnedMove> GetMoves(int selectedMember)
    {
        Pokemon member = PartyManager.instance.party.playerParty[selectedMember];

        return member.learnedMoves;
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

        gameObject.SetActive(false);
    }

    #endregion
}
