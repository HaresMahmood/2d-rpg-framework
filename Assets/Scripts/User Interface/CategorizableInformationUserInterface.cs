using System.Collections;
using UnityEngine;

/// <summary>
///
/// </summary>
public abstract class CategorizableInformationUserInterface : MonoBehaviour
{
    #region Variables

    protected Transform informationPanel;

    #endregion

    #region Miscellaneous Methods

    public virtual void SetInformation(Categorizable categorizable)
    {   }

    public virtual IEnumerator AnimatatePanel(Categorizable categorizable, Transform panel, float animationDuration = 0.15f)
    {
        FadePanel(panel, 0f, animationDuration / 2);

        yield return new WaitForSecondsRealtime(animationDuration / 2);

        SetInformation(categorizable);
        FadePanel(panel, 1f, animationDuration / 2);
    }

    public virtual void FadePanel(Transform panel, float opacity, float animationDuration)
    {
        StartCoroutine(panel.gameObject.FadeOpacity(opacity, animationDuration));
    }


    #endregion

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        
    }

    #endregion
}
