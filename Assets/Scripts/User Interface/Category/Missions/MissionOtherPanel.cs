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

    private TextMeshProUGUI assigneeText;
    private TextMeshProUGUI locationText;

    private TextMeshProUGUI rewardText;
    private Image rewardSprite;

    private TextMeshProUGUI progressText;

    #endregion

    #region Miscellaneous Methods

    public override void SetValues(ScriptableObject selectedObject)
    {
        Mission mission = (Mission)selectedObject;        

        progressText.SetText(mission.IsFailed ? "Failed" : $"{mission.CompletionPercentage}% Completed");

        if (mission.CompletionPercentage != 100 && !mission.IsFailed)
        {
            progressText.color = "#FFFFFF".ToColor();
        }
        else if (mission.CompletionPercentage == 100 && !mission.IsFailed)
        {
            progressText.color = "#76BA68".ToColor();
        }
        else
        {
            progressText.color = "#B96968".ToColor();
        }

        //assigneeText.SetText(mission.OriginDestination.Assignee.name);
        locationText.SetText(mission.OriginDestination.Destination);

        /*
        rewardText.SetText(mission.Rewards.Amount.ToString());
        rewardSprite.sprite = mission.Reward.Item.sprite;
        */
    }

    public override void FadePanel(Transform panel, float opacity, float animationDuration)
    {
        base.FadePanel(panel, opacity, animationDuration);

        StartCoroutine(progressText.transform.parent.gameObject.FadeOpacity(opacity, animationDuration));
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        InformationPanel = transform.Find("Information");

        //remainingText = InformationPanel.Find("Remaining/Value").GetComponent<TextMeshProUGUI>();
        assigneeText = InformationPanel.Find("Assignee/Value").GetComponent<TextMeshProUGUI>();
        locationText = InformationPanel.Find("Location/Value").GetComponent<TextMeshProUGUI>();

        //rewardText = InformationPanel.Find("Reward/Value/Value").GetComponent<TextMeshProUGUI>();
        //rewardSprite = InformationPanel.Find("Reward/Value/Item").GetComponent<Image>();

        progressText = transform.Find("Progress/Value").GetComponent<TextMeshProUGUI>();
    }

    #endregion
}
