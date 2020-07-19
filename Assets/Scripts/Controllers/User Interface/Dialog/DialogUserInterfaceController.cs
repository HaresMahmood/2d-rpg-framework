using System.Collections.Generic;
using System.Collections;
using UnityEngine;

/// <summary>
///
/// </summary>
public class DialogUserInterfaceController : UserInterfaceController
{
    #region Fields

    private static DialogUserInterfaceController instance;

    [SerializeField] private DialogUserInterface userInterface;

    #endregion

    #region Properties

    /// <summary>
    /// Singleton pattern.
    /// </summary>
    public static DialogUserInterfaceController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DialogUserInterfaceController>();
            }

            return instance;
        }
    }

    public override UserInterface UserInterface
    {
        get { return userInterface; }
    }

    public List<Dialog.DialogData> Dialog { private get; set; }

    #endregion

    #region Miscellaneous Methods

    public override IEnumerator SetActive(bool isActive, bool condition = true)
    {
        StartCoroutine(userInterface.ActivatePanel(isActive));
        userInterface.SetText(Dialog[0].Text);

        yield return new WaitForSeconds(0.15f);

        Flags.isActive = isActive;
    }

    protected override void GetInput(string axisName)
    {
        if (Input.GetButtonDown("Interact"))
        {
            userInterface.Stop();

            //Debug.Log("Pressed Interact");
        }

        if (Input.GetButtonDown("Toggle"))
        {
            Debug.Log("Pressed Toggle.");
        }

        if (Input.GetButtonDown("Start"))
        {
            Debug.Log("Pressed Start");
        }
    }

    #endregion
}
