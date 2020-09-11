﻿using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class FullPartySlot : Slot
{
    #region Variables

    private Image sprite;
    private Image heldItem;

    private TextMeshProUGUI nameText;
    private TextMeshProUGUI levelText;

    private GenderUserInterface gender;

    private Slider hpBar;
    private TextMeshProUGUI hpText;

    private Slider expBar;
    private TextMeshProUGUI expText;

    #endregion

    #region Miscellaneous Methods

    public override void AnimateSlot(float opacity, float duration = -1)
    {
        StartCoroutine(sprite.gameObject.FadeOpacity(opacity, duration));
        sprite.GetComponent<Animator>().SetBool("isActive", opacity == 0.5f);
    }

    public void AnimateSlot(float opacity)
    {
        if (transform.Find("Sprites").GetComponent<CanvasGroup>().alpha != opacity)
        {
            transform.Find("Sprites").GetComponent<CanvasGroup>().alpha = opacity;
            transform.Find("Information").GetComponent<CanvasGroup>().alpha = opacity;
        }
    }

    protected override void SetInformation<T>(T slotObject)
    {
        PartyMember member = (PartyMember)Convert.ChangeType(slotObject, typeof(PartyMember));

        float hp = (float)member.Stats.HP / (float)member.Stats.Stats[Pokemon.Stat.HP];
        float exp = (float)member.Progression.Value /(float)member.Progression.GetRemaining(member.Species);

        string color = hp >= 0.5f ? "#67FF8F" : (hp >= 0.25f ? "#FFB766" : "#FF7766");

        if (GetComponent<CanvasGroup>().alpha == 0)
        {
            StartCoroutine(gameObject.FadeOpacity(1f, 0.1f));
        }

        sprite.sprite = member.Species.Sprites.MenuSprite;

        if (heldItem.sprite != null)
        {
            heldItem.GetComponent<CanvasGroup>().alpha = 1;
            heldItem.sprite = member.HeldItem.Sprite;
        }
        else
        {
            heldItem.GetComponent<CanvasGroup>().alpha = 0;
        }

        nameText.SetText(member.Nickname != "" ? member.Nickname : member.Species.Name);
        levelText.SetText(member.Progression.Level.ToString());
        levelText.GetComponent<AutoTextWidth>().UpdateWidth(member.Progression.Level.ToString());

        expBar.value = exp;
        hpText.SetText($"{exp}% <color=#{ColorUtility.ToHtmlStringRGB(GameManager.GetAccentColor())}>EXP</color>");

        hpBar.value = hp;
        hpBar.fillRect.GetComponent<Image>().color = color.ToColor();
        hpText.SetText($"<color={color}>{member.Stats.HP}</color>/{member.Stats.Stats[Pokemon.Stat.HP]} <color=#{ColorUtility.ToHtmlStringRGB(GameManager.GetAccentColor())}>HP</color>");

        gender.UpdateUserInterface(member.Gender.Value);
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        sprite = transform.Find("Sprites/Sprite").GetComponent<Image>();
        heldItem = transform.Find("Sprites/Held Item").GetComponent<Image>();

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

