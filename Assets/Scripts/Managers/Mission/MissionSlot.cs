using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
///
/// </summary>
public class MissionSlot : MonoBehaviour
{
    #region Variables

    private TextMeshProUGUI nameText;
    private TextMeshProUGUI objectiveText;
    private TextMeshProUGUI remainingText;

    #endregion

    #region Miscellaneous Methods

    public void AnimateSlot(float opacity, float duration = -1f)
    {
        if (duration > -1)
        {
            StartCoroutine(gameObject.FadeOpacity(opacity, duration));
        }
        else
        {
            GetComponent<CanvasGroup>().alpha = opacity;
        }

        if (opacity == 0f) gameObject.SetActive(false);
    }

    public void UpdateInformation(Mission mission, float duration = -1f)
    {
        nameText.SetText(mission.Name);
        objectiveText.SetText(mission.objective);

        remainingText.SetText(mission.remaining);
        remainingText.GetComponent<AutoTextWidth>().UpdateWidth(mission.remaining);

        AnimateSlot(1f, duration);
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        nameText = transform.Find("Name").GetComponent<TextMeshProUGUI>();
        objectiveText = transform.Find("Information/Objective").GetComponent<TextMeshProUGUI>();
        remainingText = transform.Find("Information/Remaining").GetComponent<TextMeshProUGUI>();
    }

    #endregion
}
