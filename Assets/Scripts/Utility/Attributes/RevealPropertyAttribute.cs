using UnityEngine;

/// <summary>
///
/// </summary>
public sealed class RevealPropertyAttribute : PropertyAttribute
{
    #region Variables

    public new readonly string name;
    public bool dirty;

    #endregion

    #region Constructor

    public RevealPropertyAttribute(string name)
    {
        this.name = name;
    }

    #endregion
}
