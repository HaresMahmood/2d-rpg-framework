using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class PartyMoveSlot : PartyInformationSlot
{
    #region Variables

    private Image panel;

    private Transform information;
    private Transform statistics;
    private TextMeshProUGUI descriptionText;

    private TextMeshProUGUI moveNameText;
    private TextMeshProUGUI ppText;

    private GameObject physicalIcon;
    private GameObject specialIcon;
    private TextMeshProUGUI accuracyText;
    private TextMeshProUGUI powertext;

    private TypingUserInterface typing;

    #endregion

    #region Miscellaneous Methods

    protected override void SetInformation<T>(T slotObject)
    {
        PartyMember.MemberMove move = (PartyMember.MemberMove)Convert.ChangeType(slotObject, typeof(PartyMember.MemberMove));

        moveNameText.SetText(move.Value.Name);
   
        ppText.SetText(move.PP.ToString() + "/" + move.Value.PP);

        physicalIcon.SetActive(move.Value.Category == Move.MoveCategory.Physical);
        specialIcon.SetActive(move.Value.Category == Move.MoveCategory.Special);
        accuracyText.SetText(move.Value.Accuracy.ToString());
        powertext.SetText(move.Value.Power.ToString());

        typing.Value = move.Value.Typing.Value;
        typing.UpdateUserInterface(typing.Type, typing.Icon);

        descriptionText.SetText(move.Value.Description);

        float h, s;

        Color.RGBToHSV(new Color(typing.Type.Color.r, typing.Type.Color.g, typing.Type.Color.b), out h, out s, out _);
        Color color = Color.HSVToRGB(h, s, 0.75f);
        panel.color = new Color(color.r, color.g, color.b, 0.4f);
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        panel = transform.Find("Information Panel").GetComponent<Image>();

        Transform slot = transform.Find("Information Panel").transform;

        information = slot.Find("Information");
        statistics = slot.Find("Stats");
        descriptionText = slot.Find("Description/Value").GetComponent<TextMeshProUGUI>();

        moveNameText = information.Find("Name").GetComponent<TextMeshProUGUI>();
        ppText = information.Find("Statistics/PP/Value").GetComponent<TextMeshProUGUI>();

        physicalIcon = statistics.Find("Category/Value/Physical").gameObject;
        specialIcon = statistics.Find("Category/Value/Special").gameObject;
        accuracyText = statistics.Find("Accuracy/Value").GetComponent<TextMeshProUGUI>();
        powertext = statistics.Find("Power/Value").GetComponent<TextMeshProUGUI>();

        typing = information.Find("Statistics/Typing").GetComponent<TypingUserInterface>();

        base.Awake();
    }

    #endregion
}
