using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class EditUserInterface : UserInterface
{
    #region Constants

    public override int MaxObjects => slots.Count;

    #endregion

    #region Variables

    List<FullPartySlot> slots;

    #endregion

    #region Miscellaneous Methods

    public void UpdateInformation(List<PartyMember> party)
    {
        int counter = 0;

        for (int i = 0; i < party.Count; i++)
        {
            slots[i].UpdateInformation(party[i]);
            counter = i;
        }

        if (++counter < slots.Count)
        {
            Debug.Log(true);
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        slots = GetComponentsInChildren<FullPartySlot>().ToList();   
    }

    #endregion
}

