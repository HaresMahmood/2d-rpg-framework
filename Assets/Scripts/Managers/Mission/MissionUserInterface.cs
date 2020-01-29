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
    private GameObject indicator;

    private Animator indicatorAnimator;


    #endregion

    #region Miscellaneous Methods

    /// <summary>
    /// Animates and updates the position of the indicator. Dynamically changes position and size of indicator depending on what situation it is used for. If no value is selected, the indicator completely fades out.
    /// </summary>
    /// <param name="selectedValue"> Index of the value currently selected. Defaults to -1 (null/no value). </param>
    /// <param name="duration"> Duration of the animation/fade. </param>
    /// <param name="isInSubMenu"> Whether or not the indicator is used in the sub menu. Defaults to false. </param>
    /// <returns> Co-routine. </returns>
    public IEnumerator UpdateIndicator(int selectedValue, float duration = 0.1f)
    {
        indicatorAnimator.enabled = false;
        StartCoroutine(indicator.FadeOpacity(0f, duration));
        yield return new WaitForSecondsRealtime(duration);

        indicator.transform.position = missionPanels[selectedValue].transform.position; 
        
        yield return null;
        indicatorAnimator.enabled = true;
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        leftPanel = transform.Find("Left").gameObject;
        rightPanel = transform.Find("Right").gameObject;
        indicator = leftPanel.transform.Find("List/Indicator").gameObject;

        indicatorAnimator = indicator.GetComponent<Animator>();

        missionPanels = leftPanel.transform.Find("List/Mission List").GetComponentsInChildren<MissionPanel>();

        rightPanel.GetComponentInChildren<MissionMainPanel>().UpdateInformation(MissionManager.instance.missions.mission[0]);
        for (int i = 0; i < MissionManager.instance.missions.mission.Count; i++)
        {
            missionPanels[i].UpdateInformation(MissionManager.instance.missions.mission[i]);
        }

        StartCoroutine(UpdateIndicator(0));
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {

    }

    #endregion
    
}
