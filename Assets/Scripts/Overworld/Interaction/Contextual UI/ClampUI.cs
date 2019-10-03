using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// </summary>
public class ClampUI : MonoBehaviour
{
    #region Variables

    public GameObject contextBox;

    #endregion
    
    #region Unity Methods

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        Vector2 boxPos = CameraController.instance.cam.WorldToScreenPoint(this.transform.position);
        contextBox.transform.position = boxPos;
    }

    #endregion
}
