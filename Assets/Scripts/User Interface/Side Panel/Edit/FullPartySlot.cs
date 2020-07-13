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

    private GameObject gender;

    private Slider hpBar;
    private TextMeshProUGUI hpText;

    #endregion

    #region Miscellaneous Methods

    protected override void SetInformation<T>(T slotObject)
    {
        PartyMember member = (PartyMember)Convert.ChangeType(slotObject, typeof(PartyMember));

        float hp = (float)member.Stats.HP / (float)member.Stats.Stats[Pokemon.Stat.HP];
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

        hpBar.value = hp;
        hpBar.fillRect.GetComponent<Image>().color = color.ToColor();
        hpText.SetText($"<color={color}>{member.Stats.HP}</color>/{member.Stats.Stats[Pokemon.Stat.HP]} <color=#{ColorUtility.ToHtmlStringRGB(GameManager.GetAccentColor())}>HP</color>");
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        sprite = transform.Find("Sprite").GetComponent<Image>();
        heldItem = transform.Find("Held Item").GetComponent<Image>();

        nameText = transform.Find("Information/Name").GetComponent<TextMeshProUGUI>();
        levelText = transform.Find("Information/Level & Gender/Level/Value").GetComponent<TextMeshProUGUI>();

        gender = transform.Find("Information/Level & Gender/Gender").gameObject;

        hpBar = transform.Find("Information/Health/Health Bar").GetComponent<Slider>();
        hpText = transform.Find("Information/Health/Health Bar/Handle Slide Area/Handle/Value").GetComponent<TextMeshProUGUI>();

    }

    #endregion
}

