using System;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class HiddenAbilityUserInterface : ComponentUserInterface
{
    #region Miscellaneous Methods

    public override void UpdateUserInterface<T>(T information)
    {
        PartyMember.MemberPokerus ability = (PartyMember.MemberPokerus)Convert.ChangeType(information, typeof(PartyMember.MemberPokerus));

    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        
    }

    #endregion
}

