using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class UserInterfaceController : MonoBehaviour
{
    #region Fields

    [Header("Setup")]
    [SerializeField] private UserInterface userInterface;

    private UserInterfaceFlags flags;

    #endregion

    #region Properties

    protected virtual UserInterface UserInterface
    {
        get { return userInterface; }
    }

    protected virtual UserInterfaceFlags Flags
    {
        get { return flags; }
    }

    #endregion

    #region Variables

    public static UserInterfaceController instance;

    [Header("Setup")]
    public List<PanelButton> buttons = new List<PanelButton>();

    protected readonly TestInput input = new TestInput();

    protected int selectedValue;

    #endregion

    #region Nested Classes

    protected abstract class UserInterfaceFlags
    {
        public bool isActive { get; set; }

        protected UserInterfaceFlags(bool isActive)
        {
            this.isActive = isActive;
        }
    }

    #endregion

    #region Miscellaneous Methods

    protected virtual void GetInput()
    {
        
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected virtual void Awake()
    {
        if (instance == null)
            instance = this;
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    protected virtual void Update()
    {
        if (flags.isActive)
        {
            GetInput();
        }
    }

    #endregion
}
