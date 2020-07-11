using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class PauseUserInterfaceBase : UserInterface
{
    #region Constants

    public override int MaxObjects => throw new System.NotImplementedException();

    #endregion

    #region Miscellaneous Methods

    public virtual void SetActive(bool isActive, bool condition = true)
    { }

    #endregion
}

