using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
[CreateAssetMenu(fileName = "New Party", menuName = "Collections/Party")]
public class Party : ScriptableObject
{
    public List<PartyMember> playerParty = new List<PartyMember>();
}
