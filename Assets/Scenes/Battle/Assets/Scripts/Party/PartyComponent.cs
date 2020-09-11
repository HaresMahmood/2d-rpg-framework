using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class PartyComponent : UserInterfaceComponent
{
    #region Variables

    [SerializeField] private Party party;

    #endregion

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
            
        }
    }

    #endregion

    #region Unity Methods

    protected override void Awake()
    {
        base.Awake();

        //SetInformation(party);
    }

    #endregion
}

