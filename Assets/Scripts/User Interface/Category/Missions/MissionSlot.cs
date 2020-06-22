using UnityEngine;
using TMPro;
using System;

/// <summary>
///
/// </summary>
public class MissionSlot : CategorizableSlot
{
    #region Variables

    private TextMeshProUGUI nameText;
    private TextMeshProUGUI locationText;
    private TextMeshProUGUI progressText;

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

        locationText.SetText(mission.OriginDestination.Destination);
        locationText.GetComponent<AutoTextWidth>().UpdateWidth(locationText.text);

        progressText.SetText(mission.IsFailed ? "Failed" : $"{mission.CompletionPercentage}% Completed");
        progressText.GetComponent<AutoTextWidth>().UpdateWidth(progressText.text);

        if (mission.CompletionPercentage != 100 && !mission.IsFailed)
        {
            progressText.color = "#787878".ToColor();
        }
        else if (mission.CompletionPercentage == 100 && !mission.IsFailed)
        {
            progressText.color = "#76BA68".ToColor();
        }
        else
        {
            progressText.color = "#B96968".ToColor();
        }

        /*
        if (mission.IsCompleted && nameText.color.a != 0.5f)
        {
            FadeSlot(0.5f);
        }
        else if (!mission.IsCompleted && nameText.color.a != 1f)
        {
            FadeSlot(1f);
        }
        */
    }

    private void FadeSlot(float opacity)
    {
        nameText.alpha = opacity;
        locationText.transform.parent.GetComponent<CanvasGroup>().alpha = opacity;
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
        locationText = transform.Find("Information/Location").GetComponent<TextMeshProUGUI>();
        progressText = transform.Find("Information/Progress").GetComponent<TextMeshProUGUI>();
    }

    #endregion
}
