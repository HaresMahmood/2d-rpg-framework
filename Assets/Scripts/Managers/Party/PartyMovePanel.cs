using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class PartyMovePanel : PartyInformationPanel
{
    #region Variables



    #endregion

    #region Miscellaneous Methods

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

    protected virtual List<Pokemon.LearnedMove> GetMoves(int selectedMember)
    {
        Pokemon member = PartyManager.instance.party.playerParty[selectedMember];

        return member.activeMoves;
    }

    private PartyInformationSlots[] RemoveInactiveObjects(PartyInformationSlots[] source)
    {
        List<PartyInformationSlots> list = source.ToList();

        list.RemoveAll(panel => !panel.gameObject.activeSelf);

        return list.ToArray();
    }

    #endregion
}
