using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public interface UIHandler
{
    #region Variables



    #endregion

    #region Miscellaneous Methods

    void SetInformation<T>(T information);

    void SetInspectorValues(); // TODO: Crappy name...

    #endregion
}

