using UnityEngine;
using TMPro;

/// <summary>
///
/// </summary>
public class MissionGoal : MonoBehaviour
{
    #region Variables

    private TextMeshProUGUI descriptionText;

    #endregion

    #region Miscellaneous Methods

    public void SetInformation(Mission.MissionGoal goal, float animationDuration = 0.15f)
    {
        string objective = goal.Type == Mission.MissionGoal.GoalType.Talk || goal.Type == Mission.MissionGoal.GoalType.Escort ? goal.Character.name : (goal.Type == Mission.MissionGoal.GoalType.Defeat ? goal.Pokemon.Name : (goal.Type == Mission.MissionGoal.GoalType.Deliver ? $"{goal.Item.Name} to {goal.Character.name}" : goal.Item.Name));

        descriptionText.SetText($"{goal.Type}{(goal.Type == Mission.MissionGoal.GoalType.Talk ? " to" : "")}{(goal.Type == Mission.MissionGoal.GoalType.Gather || goal.Type == Mission.MissionGoal.GoalType.Defeat || goal.Type == Mission.MissionGoal.GoalType.Deliver ? $" {goal.Amount}" : "")} {objective}");

        StartCoroutine(gameObject.FadeOpacity(goal.IsCompleted || goal.IsFailed ? 0.3f : 1f, animationDuration));

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
    }

    #endregion
    
    #region Unity Methods
    
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        descriptionText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        
    }

    #endregion
}

