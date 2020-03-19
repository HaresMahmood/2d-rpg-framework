using System.Collections;
using UnityEngine;

public class PartyInformationController : UserInterfaceController
{
    #region Fields

    [SerializeField] private PartyInformationUserInterface userInterface;

    #endregion

    #region Properties

    public override UserInterface UserInterface
    {
        get { return userInterface; }
    }

    #endregion

    #region Miscellaneous Methods

    public override IEnumerator SetActive(bool isActive, bool condition = true)
    {
        yield return null;

        Flags.isActive = isActive;
    }

    public void SetInformation(Pokemon member)
    {
        userInterface.SetInformation(member);
    }

    /*
    protected override void InteractInput(int selectedValue)
    {
        base.InteractInput(selectedValue);

        ((InventoryUserInterface)UserInterface).ActiveSubMenu(selectedValue);
    }
    
    protected override void GetInput()
    {
        base.GetInput();

        if (Input.GetButtonDown("Interact"))
        {
            InteractInput(selectedValue);
        }
    }
    */

    #endregion

    #region Unity Methods

    /*
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        categorizableObjects.AddRange(inventory.items);

        base.Awake();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    protected override void Update()
    {
        if (Flags.isActive)
        {
            GetInput();
        }
    }
    */

    #endregion
}
