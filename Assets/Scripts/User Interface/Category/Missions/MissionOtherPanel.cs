using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class MissionOtherPanel : InformationUserInterface
{
    #region Constants

    public override int MaxObjects => throw new System.NotImplementedException();

    #endregion

    #region Variables

    private TextMeshProUGUI remainingText;
    private TextMeshProUGUI assigneeText;
    private TextMeshProUGUI locationText;

    private TextMeshProUGUI rewardText;
    private Image rewardSprite;

    private TextMeshProUGUI statusValue;

    #endregion

    #region Miscellaneous Methods

    public override void SetValues(ScriptableObject selectedObject)
    {
        /*
        Mission mission = (Mission)selectedObject;        
        Color statusColor = mission.IsCompleted ? "#3CA658".ToColor() : "#5C5C5C".ToColor();
        string status = mission.IsCompleted ? "Completed" : "Not completed";

        remainingText.SetText(mission.Remaining);
        assigneeText.SetText(mission.Assignee.name);
        locationText.SetText(mission.Origin);

        rewardText.SetText(mission.Rewards.Amount.ToString());
        rewardSprite.sprite = mission.Reward.Item.sprite;

        statusValue.SetText(status);
        if (statusValue.color != statusColor)
        {
            statusValue.color = statusColor;
        }
        */
    }

    public override void FadePanel(Transform panel, float opacity, float animationDuration)
    {
        //base.FadePanel(panel, opacity, animationDuration);

        //StartCoroutine(statusValue.transform.parent.gameObject.FadeOpacity(opacity, animationDuration));
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        //InformationPanel = transform.Find("Information");

        //remainingText = InformationPanel.Find("Remaining/Value").GetComponent<TextMeshProUGUI>();
        //assigneeText = InformationPanel.Find("Assignee/Value").GetComponent<TextMeshProUGUI>();
        //locationText = InformationPanel.Find("Location/Value").GetComponent<TextMeshProUGUI>();

        //rewardText = InformationPanel.Find("Reward/Value/Value").GetComponent<TextMeshProUGUI>();
        //rewardSprite = InformationPanel.Find("Reward/Value/Item").GetComponent<Image>();

        //statusValue = transform.Find("Status/Value").GetComponent<TextMeshProUGUI>();
    }

    #endregion
}
