using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// </summary>
public class PartyLearnedMovesUserInterface : PartyMovesPanel
{
    #region Miscellaneous Methods

    public override void UpdateSelectedObject(int selectedValue, int increment)
    {
        base.UpdateSelectedObject(selectedValue, increment);

        UpdateScrollbar(MaxObjects, selectedValue);
    }

    public override List<PartyMember.MemberMove> GetMoves(PartyMember member)
    {
        return member.LearnedMoves;
    }

    #endregion

    #region Unity Methods

    protected override void Awake()
    {
        scrollbar = transform.Find("Scrollbar").GetComponent<Scrollbar>();

        base.Awake();
    }

    #endregion
}

