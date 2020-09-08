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

    public void DeselectButtons(Button selectedButton)
    {
        List<UserInterfaceSubComponent> buttons = components.Where(b => b.GetComponent<Button>() != selectedButton && ((MoveButtonSubComponent)b).IsSelected).ToList();

        foreach (UserInterfaceSubComponent button in buttons)
        {
            ((MoveButtonSubComponent)button).SelectButton(false);
        }

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
            ((MoveButtonSubComponent)components[0]).SelectButton(true);
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

    #region Unity Methods

    private void Start()
    {
        for (int i = 1; i < components.Count; i++)
        {
            ((MoveButtonSubComponent)components[i]).SelectButton(false);
        }
    }

    #endregion
}

