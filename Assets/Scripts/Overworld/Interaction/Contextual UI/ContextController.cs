using UnityEngine;

/// <summary>
///
/// </summary>
public class ContextController : MonoBehaviour
{
    #region Variables

    public GameObject contextBox;

    #endregion

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    public void UpdatePosition()
    {
        Vector2 boxPos = CameraController.instance.cam.WorldToScreenPoint(this.transform.position);
        contextBox.transform.position = boxPos;
    }
}
