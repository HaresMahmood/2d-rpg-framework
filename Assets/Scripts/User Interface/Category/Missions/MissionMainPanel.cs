using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
///
/// </summary>
public class MissionMainPanel : MonoBehaviour
{
    #region Variables

    private Transform panel;

    private TextMeshProUGUI nameText;
    private TextMeshProUGUI objectiveText;
    private TextMeshProUGUI descriptionText;

    #endregion

    #region Miscellaneous Methods

    public void FadePanel(float opacity, float duration = 0.1f)
    {
        StartCoroutine(panel.gameObject.FadeOpacity(opacity, duration));
    }

    public void UpdateInformation(Mission mission)
    {
        nameText.SetText(mission.Name);
        objectiveText.SetText(mission.Objective);
        descriptionText.SetText(mission.Description);
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        panel = transform.Find("Information");

        nameText = panel.Find("Name").GetComponentInChildren<TextMeshProUGUI>();
        objectiveText = panel.Find("Objective/Value").GetComponent<TextMeshProUGUI>();
        descriptionText = panel.Find("Description/Value").GetComponent<TextMeshProUGUI>();
    }

    #endregion
}
