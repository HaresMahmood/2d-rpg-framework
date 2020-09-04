using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class BenchUserInterface : UserInterfaceComponent
{
    #region Variables

    private List<BenchUserInterfaceComponent> components;

    #endregion

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

    protected override void SetInspectorValues()
    {
        components = GetComponentsInChildren<BenchUserInterfaceComponent>().ToList();
    }

    #endregion

    #region Unity Methods



    #endregion
}

