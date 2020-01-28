using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class MissionUserInterface : MonoBehaviour
{
    #region Variables

    private MissionPanel[] missionPanels;

    private GameObject leftPanel;
    private GameObject rightPanel;


    #endregion

    #region Miscellaneous Methods

    

    #endregion

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        leftPanel = transform.Find("Left").gameObject;
        rightPanel = transform.Find("Right").gameObject;

        missionPanels = leftPanel.transform.Find("List/Mission List").GetComponentsInChildren<MissionPanel>();

        for (int i = 0; i < MissionManager.instance.missions.mission.Count; i++)
        {
            missionPanels[i].UpdateInformation(MissionManager.instance.missions.mission[i]);
        }
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {

    }

    #endregion
    
}
