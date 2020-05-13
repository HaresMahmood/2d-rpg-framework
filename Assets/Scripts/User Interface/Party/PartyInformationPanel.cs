using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class PartyInformationPanel : PartyInformationUserInterface
{
    #region Variables



    #endregion

    #region Miscellaneous Methods

    public override void SetInformation(PartyMember member)
    {
        informationSlots[0].UpdateInformation(member);
    }

    #endregion

    #region Unity Methods
 

    #endregion
}
