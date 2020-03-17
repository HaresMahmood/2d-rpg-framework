using System.Collections;
using UnityEngine;

/// <summary>
///
/// </summary>
public abstract class InformationUserInterface : UserInterface
{
    #region Properties

    public Transform InformationPanel {get; protected set;}

    #endregion

    #region Miscellaneous Methods

    public virtual void SetValues(ScriptableObject selectedObject)
    {   }

    public virtual void AnimatePanel(ScriptableObject selectedObject, float animationDuration = 0.15f)
    {   }

    protected virtual IEnumerator AnimatePanel(Transform panel, ScriptableObject selectedObject = null, float animationDuration = 0.15f)
    {
        FadePanel(panel, 0f, animationDuration / 2);

        if (selectedObject != null)
        {
            yield return new WaitForSecondsRealtime(animationDuration / 2);

            SetValues(selectedObject);
            FadePanel(panel, 1f, animationDuration / 2);
        }
    }

    public virtual void FadePanel(Transform panel, float opacity, float animationDuration)
    {
        StartCoroutine(panel.gameObject.FadeOpacity(opacity, animationDuration));
    }

    #endregion
}
