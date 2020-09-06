using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
///
/// </summary>
public class MoveButtonComponent : UserInterfaceComponent
{
    #region Variables

    private List<Button> buttons;

    #endregion

    #region Events

    public event EventHandler<List<int>> OnPartnerAttack;

    #endregion

    #region Miscellaneous Methods

    public void DeselectButtons(Button selectedButton)
    {
        List<Button> buttons = this.buttons.Where(b => b != selectedButton).ToList();

        foreach (Button button in buttons)
        {
            //if (button.)

            button.transform.Find("Text").gameObject.SetActive(false);
            button.transform.Find("Selector").gameObject.SetActive(false);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponentInParent<RectTransform>());
    }

    public override void SetInformation<T>(T information)
    {
        foreach (Button button in buttons)
        {
            button.GetComponent<UserInterfaceSubComponent>().SetInformation(information);
        }
    }

    #endregion

    #region Event Methods

    private void SubComponent_OnPartnerAttack(object sender, int index)
    {
        List<int> list = new List<int>();
        (int damage, int power) = buttons[index].GetComponent<MoveButtonSubComponent>().Attack();

        list.Add(damage); list.Add(power);
        OnPartnerAttack?.Invoke(this, list);
    }

    #endregion

    #region Unity Methods

    protected override void Awake()
    {
        base.Awake();

        buttons = GetComponentsInChildren<Button>().ToList();

        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].GetComponent<MoveButtonSubComponent>().OnPartnerAttack += SubComponent_OnPartnerAttack;
            buttons[i].GetComponent<MoveButtonSubComponent>().Index = i;
        }
    }

    private void Start()
    {
        for (int i = 1; i < buttons.Count; i++)
        {
            buttons[i].transform.Find("Text").gameObject.SetActive(false);
            buttons[i].transform.Find("Selector").gameObject.SetActive(false);
        }
    }

    #endregion
}

