using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// </summary>
public class BenchSubComponent : UserInterfaceSubComponent
{
    #region Variables

    private RectTransform top;
    private RectTransform bottom;

    #endregion

    #region Miscellaneous Methods

    public override void SetInformation<T>(T information)
    {
        PartyMember member = (PartyMember)Convert.ChangeType(information, typeof(PartyMember));

        if (member.Ailment.Value != PartyMember.StatusAilment.Ailment.None)
        {
            top.GetComponent<Image>().color = member.Ailment.Color;
        }

        GetComponent<CanvasGroup>().alpha = member.Ailment.Value == PartyMember.StatusAilment.Ailment.None ? 1f : 0.75f;

        top.sizeDelta = new Vector2(member.IsOnField ? 100f : 55f, top.sizeDelta.y);
        bottom.sizeDelta = top.sizeDelta;
    }

    public override void SetInspectorValues()
    {
        top = transform.Find("Top").GetComponent<RectTransform>();
        bottom = transform.Find("Bottom").GetComponent<RectTransform>();
    }

    #endregion
}

