using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class HealthSubComponent : SubUserInterfaceComponent // TODO: Crap namea
{
    #region Variables

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

    #region Miscellaneous Methods

    public void SetHealth(int health)
    {
        member.Stats.HP -= health;

        if (member.Stats.HP <= 0)
        {
            //return false;
        }

        float hp = (float)member.Stats.HP / (float)member.Stats.Stats[Pokemon.Stat.HP];
        string color = hp >= 0.5f ? "#67FF8F" : (hp >= 0.25f ? "#FFB766" : "#FF7766");
        string hpValue = hpText.text == "HP" ? "" : $"<color={color}>{member.Stats.HP}</color>/{member.Stats.Stats[Pokemon.Stat.HP]} ";

        hpBar.value = hp;
        hpBar.fillRect.GetComponent<Image>().color = color.ToColor();
        hpText.SetText($"{hpValue}<color=#{ColorUtility.ToHtmlStringRGB(GameManager.GetAccentColor())}>HP</color>");

        //return true;
    }

    public void AnimateSlot(float opacity, float duration = -1)
    {
        StartCoroutine(sprite.gameObject.FadeOpacity(opacity, duration));
        sprite.GetComponent<Animator>().SetBool("isActive", opacity != 0.3f);
    }

    public void AnimateSlot(float opacity)
    {
        if (transform.Find("Sprites").GetComponent<CanvasGroup>().alpha != opacity)
        {
            transform.Find("Sprites").GetComponent<CanvasGroup>().alpha = opacity;
            transform.Find("Information").GetComponent<CanvasGroup>().alpha = opacity;
        }
    }

    public override void SetInformation<T>(T slotObject)
    {
        PartyMember member = (PartyMember)Convert.ChangeType(slotObject, typeof(PartyMember));

        this.member = member;

        float hp = (float)member.Stats.HP / (float)member.Stats.Stats[Pokemon.Stat.HP];
        float exp = (float)member.Progression.Value / (float)member.Progression.GetRemaining(member.Species);

        string color = hp >= 0.5f ? "#67FF8F" : (hp >= 0.25f ? "#FFB766" : "#FF7766");

        if (GetComponent<CanvasGroup>().alpha == 0)
        {
            StartCoroutine(gameObject.FadeOpacity(1f, 0.1f));
        }

        sprite.sprite = member.Species.Sprites.MenuSprite;

        nameText.SetText(member.Nickname != "" ? member.Nickname : member.Species.Name);
        levelText.SetText(member.Progression.Level.ToString());
        levelText.GetComponent<AutoTextWidth>().UpdateWidth(member.Progression.Level.ToString());

        if (expBar != null)
        {
            expBar.value = exp;
            expText.SetText($"{exp}% <color=#{ColorUtility.ToHtmlStringRGB(GameManager.GetAccentColor())}>EXP</color>");
        }

        string hpValue = hpText.text == "HP" ? "" : $"<color={color}>{member.Stats.HP}</color>/{member.Stats.Stats[Pokemon.Stat.HP]} ";

        hpBar.value = hp;
        hpBar.fillRect.GetComponent<Image>().color = color.ToColor();
        hpText.SetText($"{hpValue}<color=#{ColorUtility.ToHtmlStringRGB(GameManager.GetAccentColor())}>HP</color>");

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
}


