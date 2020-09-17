using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///
/// </summary>
public class PartySelectComponent : UserInterfaceComponent
{
    #region Miscellaneous Methods

    public override void SetInformation<T>(T information)
    {
        List<PartyMember> party = ((Party)Convert.ChangeType(information, typeof(Party))).playerParty;

        for (int i = 0; i < party.Count; i++)
        {
            components[i].SetInformation(party[i]);
        }

        for (int i = party.Count; i < components.Count; i++)
        {
            ((PartySelectSubComponent)components[i]).AnimateComponent(0f);
        }
    }

    #endregion
}

