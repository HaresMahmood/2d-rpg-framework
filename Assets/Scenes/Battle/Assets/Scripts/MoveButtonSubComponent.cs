using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class MoveButtonSubComponent : SubUserInterfaceComponent
{
    #region Variables

    [SerializeField] private GameManager manager;

    private TextMeshProUGUI moveName;
    private TextMeshProUGUI pp;

    private TypingIconUserInterface icon;

    #endregion

    #region Properties

    public int Index { private get; set; }

    #endregion

    #region Miscellaneous Methods

    public override void SetInformation<T>(T information)
    {
        Party party = (Party)Convert.ChangeType(information, typeof(Party));
        PartyMember member = party.playerParty[0];
        PartyMember.MemberMove move = member.ActiveMoves[Index];

        moveName.SetText(move.Value.name);
        pp.SetText($"<color=#{ColorUtility.ToHtmlStringRGB(manager.accentColor)}>PP</color> {move.PP}/{move.Value.pp}");

        icon.Value = move.Value.typing.Value;
        icon.UpdateUserInterface(icon.Type, icon.Icon);

        float h, s;

        Color.RGBToHSV(new Color(icon.Type.Color.r, icon.Type.Color.g, icon.Type.Color.b), out h, out s, out _);
        Color color = Color.HSVToRGB(h, s, 0.75f);
        GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0.5f);
    }

    public override void SetInspectorValues()
    {
        moveName = transform.Find("Text/Name").GetComponent<TextMeshProUGUI>();
        pp = transform.Find("Text/PP").GetComponent<TextMeshProUGUI>();
        icon = transform.Find("Icon").GetComponent<TypingIconUserInterface>();
    }

    #endregion
}

