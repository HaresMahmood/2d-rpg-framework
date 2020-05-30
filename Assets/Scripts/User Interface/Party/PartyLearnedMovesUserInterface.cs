using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class PartyLearnedMovesUserInterface : PartyMovesPanel
{
    #region Miscellaneous Methods

    protected override List<PartyMember.MemberMove> GetMoves(PartyMember member)
    {
        return member.LearnedMoves;
    }

    #endregion
}

