using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// </summary>
public class InformationContainer : MonoBehaviour
{
    #region Variables

    [Header("Values")]
    [SerializeField] [ReadOnly] private bool isSelected;

    private Transform[] informationPanels;

    #endregion

    #region Miscellaneous Methods

    public void UpdatePanel(bool isSelected)
    {
        this.isSelected = isSelected;

        informationPanels[1].gameObject.SetActive(isSelected);
        informationPanels[2].gameObject.SetActive(isSelected);
    }

    private IEnumerator AnimatePanel(bool isSelected)
    {
        GetComponent<Animator>().SetBool("isSelected", isSelected);

        float duration = GetComponent<Animator>().GetAnimationTime();
        yield return new WaitForSecondsRealtime(duration);
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        informationPanels = transform.GetChildren();
    }

    #endregion
}
