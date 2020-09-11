using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
  
//////////////////////////////////////
// TODO: Needs reworking (Use DOTween!)
//////////////////////////////////////

/// <summary>
///
/// </summary>
public class PartySubComponent : UserInterfaceSubComponent
{
    #region Variables

    [Header("Settings")]
    [SerializeField] private bool animateAtStart;

    [Header("Values")]
    [SerializeField] private PartyMember member;

    private Image sprite;

    private TextMeshProUGUI nameText;
    private TextMeshProUGUI levelText;

    private GenderUserInterface gender;

    private Slider hpBar;
    private TextMeshProUGUI hpText;

    private Slider expBar;
    private TextMeshProUGUI expText;

    #endregion

    #region Events

    //[Header("Events")] [Space(5)]
    //[SerializeField] private UnityEvent OnStart;

    #endregion

    #region Miscellaneous Methods

    public void SetHealth()
    {
        string hpString = hpText.GetParsedText() == "" ? hpText.text : hpText.GetParsedText();
        float hp = (float)member.Stats.HP / member.Stats.Stats[Pokemon.Stat.HP];
        string color = hp >= 0.5f ? "#67FF8F" : (hp >= 0.25f ? "#FFB766" : "#FF7766");
        string hpValue = !hpString.Contains("/") ? "" : $"<color={color}>{member.Stats.HP}</color>/{member.Stats.Stats[Pokemon.Stat.HP]} ";

        // TODO: Make serializable/Link to BattleUI.
        hpBar.DOValue(hp, 0.15f);

        hpBar.fillRect.GetComponent<Image>().color = color.ToColor();
        hpText.SetText($"{hpValue}<color=#{ColorUtility.ToHtmlStringRGB(GameManager.GetAccentColor())}>HP</color>");
    }

    public void AnimateSlot(float opacity)
    {
        StartCoroutine(sprite.gameObject.FadeOpacity(opacity, 0.1f));
        sprite.GetComponent<Animator>().SetBool("isActive", opacity != 0.2f);
    }

    public override void SetInformation<T>(T slotObject)
    {
        PartyMember member = (PartyMember)Convert.ChangeType(slotObject, typeof(PartyMember));

        this.member = member;

        if (GetComponent<CanvasGroup>().alpha == 0)
        {
            StartCoroutine(gameObject.FadeOpacity(1f, 0.1f));
        }

        sprite.sprite = member.Species.Sprites.MenuSprite;

        nameText.SetText(member.Nickname != "" ? member.Nickname : member.Species.Name);
        levelText.SetText(member.Progression.Level.ToString());
        levelText.GetComponent<AutoTextWidth>().UpdateWidth(member.Progression.Level.ToString());

        if (expBar.gameObject.activeSelf)
        {
            float exp = (float)member.Progression.Value / member.Progression.GetRemaining(member.Species);

            expBar.value = exp;
            expText.SetText($"{(int)(exp * 100)}% <color=#{ColorUtility.ToHtmlStringRGB(GameManager.GetAccentColor())}>EXP</color>");
        }

        SetHealth();

        gender.UpdateUserInterface(member.Gender.Value);
    }

    public override void SetInspectorValues()
    {
        sprite = transform.Find("Sprites/Sprite").GetComponent<Image>();

        nameText = transform.Find("Information/Name & Gender/Name").GetComponent<TextMeshProUGUI>();
        gender = transform.Find("Information/Name & Gender/Gender").GetComponent<GenderUserInterface>();

        levelText = transform.Find("Information/Progression/Level/Value").GetComponent<TextMeshProUGUI>();

        expBar = transform.Find("Information/Progression/Experience Bar").GetComponent<Slider>();
        expText = transform.Find("Information/Progression/Experience Bar/Handle Slide Area/Handle/Value").GetComponent<TextMeshProUGUI>();

        hpBar = transform.Find("Information/Health Bar").GetComponent<Slider>();
        hpText = transform.Find("Information/Health Bar/Handle Slide Area/Handle/Value").GetComponent<TextMeshProUGUI>();
    }

    #endregion

    #region Unity Methods

    private void Start()
    {
        if (animateAtStart)
        {
            AnimateSlot(0.35f);
        }
    }

    #endregion
}


