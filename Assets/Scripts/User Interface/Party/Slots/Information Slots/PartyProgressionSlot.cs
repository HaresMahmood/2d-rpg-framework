using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class PartyProgressionSlot : PartyInformationSlot
{
    #region Variables

    private Transform level;
    private Transform experience;

    private TextMeshProUGUI levelText;

    private TextMeshProUGUI experienceText;
    private TextMeshProUGUI remainingExperienceText;
    private Slider experienceBar;

    #endregion

    #region Miscellaneous Methods

    protected override void SetInformation<T>(T slotObject)
    {
        PartyMember member = (PartyMember)Convert.ChangeType(slotObject, typeof(PartyMember));

        levelText.SetText(member.Progression.Level.ToString());

        experienceText.SetText(member.Progression.Value.ToString());
        remainingExperienceText.SetText((member.Progression.Level < 100 && member.Progression.Level > 0) ? member.Progression.GetRemaining(member.Species).ToString() : "-");

        double experience = member.Progression.Value, remaining = member.Progression.GetRemaining(member.Species);
        double result = experience / remaining;
        experienceBar.value = (float)result;
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        Transform slot = transform.Find("Information Panel").transform;

        level = slot.Find("Level");
        experience = slot.Find("Experience");

        levelText = level.Find("Value").GetComponent<TextMeshProUGUI>();

        experienceText = experience.Find("Text/Values/Value").GetComponent<TextMeshProUGUI>();
        remainingExperienceText = experience.Find("Text/Values/Remaining/Value").GetComponent<TextMeshProUGUI>();
        experienceBar = experience.Find("Experience Bar").GetComponent<Slider>();

        base.Awake();
    }

    #endregion
}
