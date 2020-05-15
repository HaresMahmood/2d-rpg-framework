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
        foreach (PartyInformationSlot slot in informationSlots)
        {
            slot.UpdateInformation(member);
        }
    }

    #endregion

    #region Unity Methods
 

    #endregion
}
