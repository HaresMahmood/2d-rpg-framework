using UnityEngine;

/// <summary>
///
/// </summary>
public abstract class UserInterfaceComponent : MonoBehaviour, UIHandler
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

