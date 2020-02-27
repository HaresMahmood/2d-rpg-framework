using System.Collections;
using UnityEngine;

/// <summary>
///
/// </summary>
public class QuantitySelectorController : UserInterfaceController
{
    #region Fields

    private static QuantitySelectorController instance;

    [SerializeField] private QuantitySelectorUserInterface userInterface;

    #endregion

    #region Properties

    /// <summary>
    /// Singleton pattern.
    /// </summary>
    public static QuantitySelectorController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<QuantitySelectorController>();
            }

            return instance;
        }
    }

    public override UserInterface UserInterface
    {
        get { return userInterface; }
        set { userInterface = (QuantitySelectorUserInterface)value; }
    }

    #endregion

    #region Miscellaneous Methods

    public override IEnumerator SetActive(bool isActive, bool condition = true)
    {
        selectedValue = 1;

        yield return null;

        Flags.isActive = isActive;
    }

    protected override void InteractInput(int selectedValue)
    {
        userInterface.ToggleSelector(false, null, -1);
        ((ItemInformationUserInterface)ItemInformationController.Instance.UserInterface).ToggleSubMenu(null, false);

        StartCoroutine(SetActive(false));
    }

    protected override void GetInput(string axisName)
    {
        base.GetInput(axisName);

        if (Input.GetButtonDown("Interact"))
        {
            InteractInput(selectedValue);
        }
    }

    #endregion
}
