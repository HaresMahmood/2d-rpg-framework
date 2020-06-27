using UnityEngine;

/// <summary>
///
/// </summary>
public class MissionsController : CategoryUserInterfaceController
{
    #region Constants

    public override int MaxViewableObjects => 6;

    #endregion

    #region Fields

    private static MissionsController instance;

    [SerializeField] private MissionsUserInterface userInterface;

    #endregion

    #region Properties

    /// <summary>
    /// Singleton pattern.
    /// </summary>
    public static MissionsController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MissionsController>();
            }

            return instance;
        }
    }

    public override UserInterface UserInterface
    {
        get { return userInterface; }
    }

    #endregion

    #region Variables

    [SerializeField] private Missions missions;

    [Header("Values")]
    [SerializeField] private SortingMethod sortingMethod = SortingMethod.None;

    #endregion

    #region Enums

    public enum SortingMethod
    {
        None,
        AToZ,
        ZToA,
        AmountAscending,
        AmountDescending,
        FavoriteFirst,
        NewFirst
    }

    #endregion

    #region Miscellaneous Methods

    public void SetActive(bool isActive)
    {
        Flags.isActive = isActive;
    }

    protected override void InteractInput(int selectedValue)
    {
        base.InteractInput(selectedValue);

        ((MissionsUserInterface)UserInterface).ActivateSubMenu(selectedValue);
    }

    protected override void GetInput(string axis)
    {
        base.GetInput(axis);

        if (Input.GetButtonDown("Interact"))
        {
            InteractInput(selectedValue);
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        categorizableObjects.AddRange(missions.mission);

        base.Awake();
    }

    #endregion
}
