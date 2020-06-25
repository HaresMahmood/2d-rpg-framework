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

    private MissionGoalsUserInterface goals;

    private TextMeshProUGUI nameText;
    private TextMeshProUGUI descriptionText;

    #endregion

    #region Miscellaneous Methods

    public override void SetValues(ScriptableObject selectedObject)
    {
        Mission mission = (Mission)selectedObject;

        nameText.SetText(mission.Name);
        descriptionText.SetText(mission.Description);

        goals.SetInformation(mission);
    }

    public override void FadePanel(Transform panel, float opacity, float animationDuration)
    {
        base.FadePanel(panel, opacity, animationDuration);

        StartCoroutine(goals.transform.parent.gameObject.FadeOpacity(opacity, animationDuration));
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
        descriptionText = InformationPanel.Find("Description/Value").GetComponent<TextMeshProUGUI>();

        goals = transform.Find("Goals/Nodes").GetComponent<MissionGoalsUserInterface>();
    }

    #endregion
}
