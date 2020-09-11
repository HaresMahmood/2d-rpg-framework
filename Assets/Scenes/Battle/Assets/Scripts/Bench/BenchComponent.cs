using System;
using System.Collections.Generic;

/// <summary>
///
/// </summary>
public class BenchComponent : UserInterfaceComponent
{
    #region Miscellaneous Methods

    public override void SetInformation<T>(T information)
    {
        Party party = (Party)Convert.ChangeType(information, typeof(Party));

        for (int i = 0; i < party.playerParty.Count; i++)
        {
            components[i].SetInformation(party.playerParty[i]);
        }

        for (int i = party.playerParty.Count; i < components.Count; i++)
        {
            components[i].gameObject.SetActive(false);
        }
    }

    #endregion
}

