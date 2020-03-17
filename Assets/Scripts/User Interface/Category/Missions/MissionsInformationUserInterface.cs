using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class MissionsInformationUserInterface : InformationUserInterface
{
    #region Constants

    public override int MaxObjects => throw new System.NotImplementedException();

    #endregion

    #region Variables

    private MissionMainPanel mainPanel;
    private MissionOtherPanel otherPanel;

    #endregion

    #region Miscellaneous Methods

    public override void SetValues(ScriptableObject selectedObject)
    {
        mainPanel.SetValues(selectedObject);
        otherPanel.SetValues(selectedObject);
    }

    public override void AnimatePanel(ScriptableObject selectedObject, float animationDuration = 0.15f)
    {
        StartCoroutine(AnimatePanel(null, selectedObject, animationDuration));
    }

    protected override IEnumerator AnimatePanel(Transform panel, ScriptableObject selectedObject = null, float animationDuration = 0.15F)
    {  
        float delay = 0.03f;

        mainPanel.FadePanel(mainPanel.InformationPanel, 0f, animationDuration / 2);

        yield return new WaitForSecondsRealtime(delay);

        otherPanel.FadePanel(otherPanel.InformationPanel, 0f, animationDuration / 2);

        if (selectedObject != null)
        {
            yield return new WaitForSecondsRealtime(animationDuration / 2);

            SetValues(selectedObject);
            mainPanel.FadePanel(mainPanel.InformationPanel, 1f, animationDuration / 2);

            yield return new WaitForSecondsRealtime(delay);

            otherPanel.FadePanel(otherPanel.InformationPanel, 1f, animationDuration / 2);
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        mainPanel = transform.Find("Main Information").GetComponent<MissionMainPanel>();
        otherPanel = transform.Find("Other Information").GetComponent<MissionOtherPanel>();
    }

    #endregion
}
