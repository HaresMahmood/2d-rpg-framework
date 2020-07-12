using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class PauseController : MonoBehaviour
{
    #region Fields

    private ControllerFlags flags = new ControllerFlags(true, false);

    #endregion

    #region Properties

    public ControllerFlags Flags
    {
        get { return flags; }
    }

    #endregion

    #region Variables



    #endregion

    #region Nested Classes

    public class ControllerFlags
    {
        public bool isActive { get; internal set; }
        public bool isPaused { get; internal set; }

        internal ControllerFlags(bool isActive, bool isPaused)
        {
            this.isActive = isActive;
            this.isPaused = isPaused;
        }
    }

    #endregion

    #region Miscellaneous Methods

    private void SetActive(bool isActive)
    {
        Time.timeScale = flags.isPaused ? 0 : 1;

        Debug.Log(Time.timeScale);

        //CameraController.instance.GetComponent<PostprocessingBlur>().enabled = flags.isActive;
        StartCoroutine(GetComponent<PauseUserInterfaceController>().SetActive(isActive));
        StartCoroutine(GetComponent<SidebarUserInterfaceController>().SetActive(isActive));
    }

    private void GetInput()
    {
        if (Input.GetButtonDown("Start"))
        {
            Flags.isPaused = !Flags.isPaused;

            SetActive(Flags.isPaused);
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        if (Flags.isActive)
        {
            GetInput();
        }
    }

    #endregion
}

