using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void AnimatePanel(bool isSelected)
    {

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
