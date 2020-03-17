using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
///
/// </summary>
public class MissionMainPanel : InformationUserInterface
{
    #region Constants

    public override int MaxObjects => throw new System.NotImplementedException();

    #endregion

    #region Variables

    private TextMeshProUGUI nameText;
    private TextMeshProUGUI objectiveText;
    private TextMeshProUGUI descriptionText;

    #endregion

    #region Miscellaneous Methods

    public override void SetValues(ScriptableObject selectedObject)
    {
        Mission mission = (Mission)selectedObject;

        nameText.SetText(mission.Name);
        objectiveText.SetText(mission.Objective);
        descriptionText.SetText(mission.Description);
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        InformationPanel = transform.Find("Information");

        nameText = InformationPanel.Find("Name").GetComponentInChildren<TextMeshProUGUI>();
        objectiveText = InformationPanel.Find("Objective/Value").GetComponent<TextMeshProUGUI>();
        descriptionText = InformationPanel.Find("Description/Value").GetComponent<TextMeshProUGUI>();
    }

    #endregion
}
