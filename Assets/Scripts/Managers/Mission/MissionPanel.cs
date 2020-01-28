using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
///
/// </summary>
public class MissionPanel : MonoBehaviour
{
    #region Variables

    private TextMeshProUGUI nameText;
    private TextMeshProUGUI objectiveText;
    private TextMeshProUGUI remainingText;

    #endregion

    #region Miscellaneous Methods

    public void UpdateInformation(Mission mission)
    {
        nameText.SetText(mission.Name);
        objectiveText.SetText(mission.objective);
        remainingText.SetText(mission.remaining);
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
