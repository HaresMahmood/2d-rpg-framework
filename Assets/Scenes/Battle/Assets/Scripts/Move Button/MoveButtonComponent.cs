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
    #region Events

    public event EventHandler<int> OnPartnerAttack;

    #endregion

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
        List<PartyMember.MemberMove> moves = ((PartyMember)Convert.ChangeType(information, typeof(PartyMember))).ActiveMoves;

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
            }
        }
    }

    #endregion

    #region Event Methods

    private void SubComponent_OnPartnerAttack(object sender, int index)
    {
        OnPartnerAttack?.Invoke(this, components[index].GetComponent<MoveButtonSubComponent>().Attack());
    }

    #endregion

    #region Unity Methods

    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < components.Count; i++)
        {
            ((MoveButtonSubComponent)components[i]).OnPartnerAttack += SubComponent_OnPartnerAttack;
        }
    }

    private void Start()
    {
        for (int i = 1; i < components.Count; i++)
        {
            ((MoveButtonSubComponent)components[i]).SelectButton(false);
        }
    }

    #endregion
}

