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

    [SerializeField] private BattleUserInterface battleUserInterface;

    [Header("Values")]
    [SerializeField] private PartyMember.MemberMove move;

    private GameObject textContainer;
    private GameObject selector;

    private TextMeshProUGUI moveName;
    private TextMeshProUGUI pp;

    private TypingIconUserInterface icon;

    #endregion

    #region Properties

    public bool IsSelected { get; private set; }

    #endregion

    #region Events

    public event EventHandler<int> OnPartnerAttack;

    #endregion

    #region Miscellaneous Methods

    public void SelectButton(bool isSelected)
    {
        IsSelected = isSelected;

        textContainer.SetActive(isSelected);
        transform.Find("Selector").gameObject.SetActive(isSelected);
    }

    public int Attack()
    {
        return move.Value.CalculateDamage(battleUserInterface.Partner, battleUserInterface.Enemy);
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
        textContainer = transform.Find("Text").gameObject;
        selector = transform.Find("Selector").gameObject;
        moveName = textContainer.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        pp = textContainer.transform.Find("PP").GetComponent<TextMeshProUGUI>();
        icon = transform.Find("Icon").GetComponent<TypingIconUserInterface>();
    }

    #endregion

    #region Event Methods

    private void OnClick()
    {
        OnPartnerAttack?.Invoke(this, Index);
    }

    #endregion

    #region Unity Methods

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    #endregion
}

