using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
///
/// </summary>
public class MoveButtonComponent : UserInterfaceComponent
{
    #region Miscellaneous Methods

    public override void DeselectComponents(UserInterfaceSubComponent selectedComponent)
    {
        base.DeselectComponents(selectedComponent);

        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponentInParent<RectTransform>());
    }

    public void EnableButtons(bool isEnabled)
    {
        foreach (UserInterfaceSubComponent component in components)
        {
            component.GetComponent<Button>().interactable = isEnabled;
        }

        if (isEnabled)
        {
            EventSystem.current.SetSelectedGameObject(components[0].gameObject);
            ((MoveButtonSubComponent)components[0]).Select(true);
        }
    }

    public override void SetInformation<T>(T information)
    {
        Party party = ((Party)Convert.ChangeType(information, typeof(Party)));
        PartyMember member = party.playerParty[BattleManager.Instance.CurrentPartner];
        List<PartyMember.MemberMove> moves = member.ActiveMoves;

        SetInspectorValues();

        for (int i = 0; i < moves.Count; i++)
        {
            components[i].gameObject.SetActive(true);
            components[i].SetInformation(moves[i]);
        }

        if (components.Count > moves.Count)
        {
            for (int i = moves.Count - 1; i < components.Count; i++)
            {
                components[i].gameObject.SetActive(false);
                components.RemoveAt(i);
            }
        }
    }

    #endregion
}

