using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class MissionGoalsUserInterface : MonoBehaviour
{
    #region Variables

    private List<MissionGoal> goals;

    #endregion

    #region Miscellaneous Methods

    public void SetInformation(Mission mission)
    {
        for (int i = 0; i < goals.Count; i++)
        {
            goals[i].gameObject.SetActive(true);
        }

        int counter = 0;

        foreach (Mission.MissionGoal goal in mission.Goals)
        {
            goals[counter].SetInformation(goal);

            counter++;
        }

        for (int i = counter; i < goals.Count; i++)
        {
            goals[i].gameObject.SetActive(false);
        }
    }

    #endregion
    
    #region Unity Methods
    
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        goals = transform.GetComponentsInChildren<MissionGoal>().ToList();
    }

    #endregion
}

