using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class DialogPauseUserInterface : UserInterface
{
    #region Constants

    public override int MaxObjects => throw new System.NotImplementedException();

    #endregion

    #region Miscellaneous Methods

    public void SetActive(bool isActive, float animationDuration = 0.15f)
    {
        Time.timeScale = isActive ? 0 : 1;

        StartCoroutine(gameObject.FadeOpacity(isActive ? 1f : 0f, animationDuration));
    }

    #endregion

    #region Unity Methods

    protected override void Awake()
    { }

    #endregion
}

