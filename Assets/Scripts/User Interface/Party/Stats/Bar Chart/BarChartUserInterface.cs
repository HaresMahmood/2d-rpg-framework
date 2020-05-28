using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
///
/// </summary>
public class BarChartUserInterface : MonoBehaviour
{
    #region Variables

    private List<BarComponent> bars;

    #endregion

    #region Miscellaneous Methods

    public void SetInformation(PartyMember.StatDictionary evs, PartyMember.StatDictionary ivs)
    {
        foreach (BarComponent bar in bars)
        {
            Pokemon.Stat stat = (Pokemon.Stat)Enum.Parse(typeof(Pokemon.Stat), Regex.Replace(bar.name, "[^A-Za-z]", ""));

            bar.SetInformation(evs[stat], ivs[stat]);
        }
    }

    #endregion
    
    #region Unity Methods
    
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        bars = GetComponentsInChildren<BarComponent>().ToList();
    }

    #endregion
}

