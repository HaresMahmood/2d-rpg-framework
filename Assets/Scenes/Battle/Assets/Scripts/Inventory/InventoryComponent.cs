using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// </summary>
public class InventoryComponent : UserInterfaceComponent, UIButtonParentHandler
{
    #region Variables

    [SerializeField] private Inventory party;

    #endregion

    #region Miscellaneous Methods

    public void DeselectComponents(UserInterfaceSubComponent selectedComponent)
    {
        List<UserInterfaceSubComponent> components = this.components.Where(b => b != selectedComponent && b.transform.Find("Selector").gameObject.activeSelf).ToList();

        foreach (UserInterfaceSubComponent component in components)
        {
            component.transform.Find("Selector").gameObject.SetActive(false);
        }
    }

    public override void SetInformation<T>(T information)
    {
        /*
        List<PartyMember> party = ((Party)Convert.ChangeType(information, typeof(Party))).playerParty;

        for (int i = 0; i < party.Count; i++)
        {
            components[i].SetInformation(party[i]);
        }

        for (int i = party.Count; i < components.Count; i++)
        {
            ((PartySubComponent)components[i]).AnimateComponent(0f);
        }
        */
    }

    #endregion

    #region Unity Methods

    private void Start()
    {
        for (int i = 1; i < components.Count; i++)
        {
            components[i].transform.Find("Selector").gameObject.SetActive(false);
        }

        transform.Find("Inventory/Content/Categories").GetComponent<ButtonPromptController>().SetInformation(transform.Find("Inventory/Content/Categories").GetComponent<ButtonList>().PromptGroups);
        GetComponentInChildren<Scrollbar>().value = 0;
    }

    #endregion
}

