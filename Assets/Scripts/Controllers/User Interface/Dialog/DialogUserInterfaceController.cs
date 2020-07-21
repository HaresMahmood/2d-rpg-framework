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

    #region Variables

    private DialogController controller;

    #endregion

    #region Miscellaneous Methods

    public override IEnumerator SetActive(bool isActive, bool condition = true)
    {
        if (condition)
        {
            StartCoroutine(userInterface.ActivatePanel(isActive));
        }
        if (isActive && condition)
        {
            selectedValue = 0;
            userInterface.UpdateInformation(Dialog[selectedValue]);

            yield return new WaitForSeconds(0.15f);
        }
        else if (!condition)
        {
            //StartCoroutine(GetComponent<BranchingDialogUserInterfaceController>().SetActive(!isActive)); // TODO: Debug
            StartCoroutine(userInterface.ActivateBranchedPanel(!isActive));
        }

        Flags.isActive = isActive;
    }

    protected override void GetInput(string axisName)
    {
        if (Input.GetButtonDown("Interact") || Input.GetButtonDown("Cancel"))
        {
            if (!userInterface.Stop())
            {
                if (selectedValue < Dialog.Count - 1)
                {
                    selectedValue++;
                    userInterface.UpdateInformation(Dialog[selectedValue]);
                }
                else
                {
                    if (Dialog[selectedValue].Branch == null)
                    {
                        StartCoroutine(userInterface.ActivatePanel(false));
                        controller.SetActive(false);
                    }
                }
            }

            //Debug.Log("Pressed Interact");
        }

        if (Input.GetButtonDown("Toggle"))
        {
            Debug.Log("Pressed Toggle");
        }

        if (Input.GetButtonDown("Start"))
        {
            Debug.Log("Pressed Start");
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        controller = GetComponent<DialogController>();
    }

    #endregion 
}
