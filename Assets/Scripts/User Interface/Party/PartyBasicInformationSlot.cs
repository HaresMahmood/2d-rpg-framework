using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class PartyBasicInformationSlot : PartyInformationSlot
{
    #region Variables

    private new Transform name;
    private Transform typing;
    private Transform statusAilment;

    private TextMeshProUGUI nameText;
    private Transform gender;
    private TextMeshProUGUI categoryText;
    private TextMeshProUGUI dexText;

    private TypingUserInterface primaryTyping;
    private TypingUserInterface secondaryTyping;

    private TextMeshProUGUI ailmentText;
    private Image pokerusIcon;
    private TextMeshProUGUI pokerusText;

    #endregion

    #region Miscellaneous Methods

    protected override void SetInformation<T>(T slotObject)
    {
        PartyMember member = (PartyMember)Convert.ChangeType(slotObject, typeof(PartyMember));

        nameText.SetText((member.Nickname != "" ? member.Nickname : member.Species.Name));

        Transform[] genders = gender.GetChildren();

        // TODO: Create GenderUserInterface class
        if (member.Gender.Value == PartyMember.MemberGender.Gender.None)
        {
            genders[0].gameObject.SetActive(false);
            genders[1].gameObject.SetActive(false);
        }
        else
        {
            genders[(int)member.Gender.Value - 1].gameObject.SetActive(true);
            genders[ExtensionMethods.IncrementInt((int)member.Gender.Value - 1, 0, 2, 1)].gameObject.SetActive(false);
        }

        categoryText.SetText(member.Species.Category);
        categoryText.GetComponent<AutoTextWidth>().UpdateWidth(member.Species.Category);
        dexText.SetText(member.Species.ID.ToString("000"));

        primaryTyping.Value = member.Species.PrimaryType.Value;
        primaryTyping.UpdateUserInterface(primaryTyping.Type, primaryTyping.Icon);
        secondaryTyping.Value = member.Species.SecondaryType.Value;
        secondaryTyping.UpdateUserInterface(secondaryTyping.Type, secondaryTyping.Icon);

        ailmentText.SetText(member.Ailment.Value.ToString());
        ailmentText.GetComponent<AutoTextWidth>().UpdateWidth(member.Ailment.Value.ToString());
        // TODO: Create PokerusUserInterface class
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        Transform slot = transform.Find("Information Panel").transform;

        name = slot.Find("Name");
        typing = slot.Find("Typing/Values");
        statusAilment = slot.Find("Status Ailment/Values");

        nameText = name.Find("Basic Information/Value").GetComponent<TextMeshProUGUI>();
        gender = name.Find("Basic Information/Gender");
        categoryText = name.Find("Dex Information/Category/Value").GetComponent<TextMeshProUGUI>();
        dexText = name.Find("Dex Information/Dex Number/Value").GetComponent<TextMeshProUGUI>();

        primaryTyping = typing.Find("Primary Typing").GetComponent<TypingUserInterface>();
        secondaryTyping = typing.Find("Secondary Typing").GetComponent<TypingUserInterface>();

        ailmentText = statusAilment.Find("Ailment").GetComponent<TextMeshProUGUI>();
        pokerusText = statusAilment.Find("Pokerus/Value").GetComponent<TextMeshProUGUI>();
        pokerusIcon = statusAilment.Find("Pokerus/Icon").GetComponent<Image>();

        base.Awake();
    }

    #endregion
}
