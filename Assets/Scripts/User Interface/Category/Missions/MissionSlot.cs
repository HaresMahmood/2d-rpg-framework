﻿using UnityEngine;
using TMPro;
using System;

/// <summary>
///
/// </summary>
public class MissionSlot : CategorizableSlot
{
    #region Variables

    private TextMeshProUGUI nameText;
    private TextMeshProUGUI objectiveText;
    private TextMeshProUGUI remainingText;

    #endregion

    #region Miscellaneous Methods

    public void DeactivateSlot()
    {
        base.Awake();
    }

    protected override void SetInformation<T>(T slotObject)
    {
        Mission mission = (Mission)Convert.ChangeType(slotObject, typeof(Mission));

        nameText.SetText(mission.Name);
        objectiveText.SetText(mission.Objective);

        remainingText.SetText(mission.Remaining);
        remainingText.GetComponent<AutoTextWidth>().UpdateWidth(mission.Remaining);

        if (mission.IsCompleted && nameText.color.a != 0.5f)
        {
            FadeSlot(0.5f);
        }
        else if (!mission.IsCompleted && nameText.color.a != 1f)
        {
            FadeSlot(1f);
        }
    }

    private void FadeSlot(float opacity)
    {
        nameText.alpha = opacity;
        objectiveText.transform.parent.GetComponent<CanvasGroup>().alpha = opacity;
    }


    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        slot = transform;

        nameText = transform.Find("Name").GetComponent<TextMeshProUGUI>();
        objectiveText = transform.Find("Information/Objective").GetComponent<TextMeshProUGUI>();
        remainingText = transform.Find("Information/Remaining").GetComponent<TextMeshProUGUI>();
    }

    #endregion
}
