using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public abstract class UserInterfaceComponent : MonoBehaviour, UIHandler
{
    #region Variables

    protected List<UserInterfaceSubComponent> components;

    #endregion

    #region Miscellaneous Methods

    public virtual void SetInformation<T>(T information)
    { }

    public virtual void SetInspectorValues()
    {
        components = GetComponentsInChildren<UserInterfaceSubComponent>().ToList();
    }

    #endregion

    #region Unity Methods

    protected virtual void Awake()
    {
        SetInspectorValues();
    }

    #endregion
}

