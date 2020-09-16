using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
///
/// </summary>
public class OnComponentEnable : MonoBehaviour
{
    #region Variables

    public UnityEvent onEnable;

    #endregion

    #region Miscellaneous Methods



    #endregion

    #region Unity Methods

    private void OnEnable()
    {
        onEnable.Invoke();
    }

    #endregion
}

