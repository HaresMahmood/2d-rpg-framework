using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public abstract class ComponentUserInterface : MonoBehaviour
{
    #region Variables

    [SerializeField] protected List<Sprite> icons = new List<Sprite>();

    #endregion

    #region Miscellaneous Methods

    public virtual void UpdateUserInterface<T>(T information)
    { }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected virtual void Awake()
    { }

    #endregion
}

