using UnityEngine;

/// <summary>
///
/// </summary>
public abstract class UserInterfaceComponent : MonoBehaviour
{
    #region Variables



    #endregion

    #region Miscellaneous Methods

    public abstract void SetInformation<T>(T information);

    protected virtual void SetInspectorValues() // TODO: Crappy name...
    { }

    #endregion
    
    #region Unity Methods
    
    protected virtual void Awake()
    {
        SetInspectorValues();
    }

    #endregion
}

