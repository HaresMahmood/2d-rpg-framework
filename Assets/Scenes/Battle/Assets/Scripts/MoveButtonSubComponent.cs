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

    [SerializeField] private HealthSubComponent enemyHealth;
    [SerializeField] private BattleUserInterface battleUserInterface;

    [Header("Values")]
    [SerializeField] private PartyMember.MemberMove move;

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

        this.move = move;

        moveName.SetText(move.Value.Name);
        pp.SetText($"<color=#{ColorUtility.ToHtmlStringRGB(manager.accentColor)}>PP</color> {move.PP}/{move.Value.PP}");

        icon.Value = move.Value.Typing.Value;
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

    private int CalculateDamage()
    {
        PartyMember partner = battleUserInterface.Partner;
        PartyMember enemy = battleUserInterface.Enemy;

        float attackStat = move.Value.Category == Move.MoveCategory.Physical ? partner.Stats.Stats[Pokemon.Stat.Attack] : partner.Stats.Stats[Pokemon.Stat.SpAttack];
        float defenceStat = move.Value.Category == Move.MoveCategory.Physical ? enemy.Stats.Stats[Pokemon.Stat.Defence] : enemy.Stats.Stats[Pokemon.Stat.SpDefence];

        float modifier = 0.75f // Target (default: one target)
                       * 1f // Weather (default: neutral weather)
                       * 1f // Critical (default: non-critical) https://bulbapedia.bulbagarden.net/wiki/Critical_hit
                       * UnityEngine.Random.Range(0.85f, 1f) // Random
                       * (partner.Species.PrimaryType == move.Value.Typing || partner.Species.SecondaryType == move.Value.Typing ? 1.5f : 1f) // STAB
                       * 1f // Type (default: normally effective type)
                       * 1f // Burn (default: target not burned)
                       * 1f; // Other (default: "1 in most cases")#
                       // https://bulbapedia.bulbagarden.net/wiki/Damage

        float damage = (((((float)(2 * partner.Progression.Level) / 5) + 2) * move.Value.Power * (float)attackStat / (float)defenceStat) / 50) * modifier;

        return (int)damage;
    }

    #endregion

    #region Event Methods

    private void OnClick()
    {
        int damage = CalculateDamage();

        battleUserInterface.damageText.GetComponentInChildren<TextMeshProUGUI>().SetText(damage.ToString());
        //battleUserInterface.damageText.SetActive(true);

        Debug.Log(enemyHealth.SetHealth(damage));
    }

    #endregion

    #region Unity Methods

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    #endregion
}

