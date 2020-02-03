using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class MissionOtherPanel : MonoBehaviour
{
    #region Variables

    private Transform informationPanel;

    private TextMeshProUGUI remainingText;
    private TextMeshProUGUI assigneeText;
    private TextMeshProUGUI locationText;

    private TextMeshProUGUI rewardText;
    private Image rewardSprite;

    private TextMeshProUGUI statusValue;

    #endregion

    #region Miscellaneous Methods

    public void FadePanel(float opacity, float duration = 0.1f)
    {
        StartCoroutine(informationPanel.gameObject.FadeOpacity(opacity, duration));
        StartCoroutine(statusValue.transform.parent.gameObject.FadeOpacity(opacity, duration));
    }

    public void UpdateInformation(Mission mission)
    {
        string status = mission.isCompleted ? "Completed" : "Not completed";
        Color statusColor = mission.isCompleted ? "#3CA658".ToColor() : "#5C5C5C".ToColor();

        remainingText.SetText(mission.remaining);
        assigneeText.SetText(mission.assignee.name);
        locationText.SetText(mission.location);

        rewardText.SetText(mission.rewardAmount);
        rewardSprite.sprite = mission.rewardItem.sprite;

        statusValue.SetText(status);
        if (statusValue.color != statusColor)
        {
            statusValue.color = statusColor;
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        informationPanel = transform.Find("Information");

        remainingText = informationPanel.Find("Remaining/Value").GetComponent<TextMeshProUGUI>();
        assigneeText = informationPanel.Find("Assignee/Value").GetComponent<TextMeshProUGUI>();
        locationText = informationPanel.Find("Location/Value").GetComponent<TextMeshProUGUI>();

        rewardText = informationPanel.Find("Reward/Value/Value").GetComponent<TextMeshProUGUI>();
        rewardSprite = informationPanel.Find("Reward/Value/Item").GetComponent<Image>();

        statusValue = transform.Find("Status/Value").GetComponent<TextMeshProUGUI>();
    }

    #endregion
}
