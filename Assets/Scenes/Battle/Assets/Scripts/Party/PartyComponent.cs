using System;
using System.Collections.Generic;
using System.Linq;
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

    public void DeselectComponents(PartySubComponent selectedComponent)
    {
        List<UserInterfaceSubComponent> components = this.components.Where(b => (PartySubComponent)b != selectedComponent && ((PartySubComponent)b).transform.Find("Selector").gameObject.activeSelf).ToList();

        foreach (PartySubComponent component in components)
        {
            component.AnimateSlot(0.2f);
            component.transform.Find("Selector").gameObject.SetActive(false);
        }
    }

    public override void SetInformation<T>(T information)
    {
        List<PartyMember> party = ((Party)Convert.ChangeType(information, typeof(Party))).playerParty;

        for (int i = 0; i < party.Count; i++)
        {
            components[i].SetInformation(party[i]);
        }

        for (int i = party.Count; i < components.Count; i++)
        {
            ((PartySubComponent)components[i]).AnimateComponent(0f);
        }
    }

    #endregion

    #region Unity Methods

    private void Start()
    {
        ((PartySubComponent)components[0]).AnimateSlot(0.35f);

        for (int i = 1; i < components.Count; i++)
        {
            components[i].transform.Find("Selector").gameObject.SetActive(false);
        }
    }

    #endregion
}

