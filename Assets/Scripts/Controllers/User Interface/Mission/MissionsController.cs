using UnityEngine;

/// <summary>
///
/// </summary>
public class MissionsController : CategoryUserInterfaceController
{
    #region Constants

    public override int MaxViewableObjects => 7;

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
