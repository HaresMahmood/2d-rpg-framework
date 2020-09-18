using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class MoveButtonSubComponent : UserInterfaceSubComponent
{
    #region Variables

    [Header("Values")]
    [SerializeField, ReadOnly] private PartyMember.MemberMove move;

    private GameObject textContainer;

    private TextMeshProUGUI moveName;
    private TextMeshProUGUI pp;

    private TypingIconUserInterface icon;

    #endregion

    #region Miscellaneous Methods

    public override void Select(bool isSelected)
    {
        base.Select(isSelected);

        textContainer.SetActive(isSelected);

        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponentInParent<RectTransform>());
    }

    public override void SetInformation<T>(T information)
    {
        PartyMember.MemberMove move = (PartyMember.MemberMove)Convert.ChangeType(information, typeof(PartyMember.MemberMove));

        this.move = move;

        moveName.SetText(move.Value.Name);
        pp.SetText($"<color=#{ColorUtility.ToHtmlStringRGB(GameManager.instance.accentColor)}>PP</color> {move.PP}/{move.Value.PP}");

        icon.Value = move.Value.Typing.Value;
        icon.UpdateUserInterface(icon.Type, icon.Icon);

        float h, s;

        Color.RGBToHSV(new Color(icon.Type.Color.r, icon.Type.Color.g, icon.Type.Color.b), out h, out s, out _);
        Color color = Color.HSVToRGB(h, s, 0.75f);
        GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0.5f);
    }

    public override void SetInspectorValues()
    {
        base.SetInspectorValues();

        textContainer = transform.Find("Text").gameObject;
        moveName = textContainer.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        pp = textContainer.transform.Find("PP").GetComponent<TextMeshProUGUI>();
        icon = transform.Find("Icon").GetComponent<TypingIconUserInterface>();
    }

    private void Attack()
    {
        BattleManager.Instance.Attack(move.Value.CalculateDamage(BattleManager.Instance.Partner, BattleManager.Instance.Enemy));
    }

    #endregion

    #region Unity Methods

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(Attack);
    }

    #endregion
}

