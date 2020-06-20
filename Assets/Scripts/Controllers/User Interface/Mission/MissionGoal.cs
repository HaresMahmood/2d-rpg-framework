using UnityEngine;
using TMPro;

/// <summary>
///
/// </summary>
public class MissionGoal : MonoBehaviour
{
    #region Variables

    private TextMeshProUGUI description;

    #endregion

    #region Miscellaneous Methods

    public void SetInformation(Mission mission, int index)
    {
        string objective = mission.Goals[index].Type == Mission.MissionGoal.GoalType.Talk || mission.Goals[index].Type == Mission.MissionGoal.GoalType.Deliver || mission.Goals[index].Type == Mission.MissionGoal.GoalType.Escort ? mission.Goals[index].Character.name : (mission.Goals[index].Type == Mission.MissionGoal.GoalType.Battle ? mission.Goals[index].Pokemon.Name : mission.Goals[index].Item.Name);

        description.SetText($"{mission.Goals[index].Type} {(mission.Goals[index].Type == Mission.MissionGoal.GoalType.Gather || mission.Goals[index].Type == Mission.MissionGoal.GoalType.Battle ? mission.Goals[index].Amount.ToString() : "")} {objective}");
    }

    #endregion
    
    #region Unity Methods
    
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        description = transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        
    }

    #endregion
}

