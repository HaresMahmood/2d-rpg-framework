using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class SubUserInterfaceComponent : MonoBehaviour, UIHandler
{
    #region Variables



    #endregion

    #region Miscellaneous Methods

    public virtual void SetInformation<T>(T information)
    { }

    public virtual void SetInspectorValues()
    { }

    #endregion

    #region Unity Methods

    protected virtual void Awake()
    {
        SetInspectorValues();
    }

    #endregion
}

