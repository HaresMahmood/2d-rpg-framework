using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public abstract class SystemUserInterfaceBase : UserInterface
{
    #region Constants

    public override int MaxObjects => throw new System.NotImplementedException();

    #endregion

    #region Miscellaneous Methods

    public virtual void SetActive(bool isActive)
    { }

    #endregion
}

