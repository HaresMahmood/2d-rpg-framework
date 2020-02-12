using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class UserInterfaceController : MonoBehaviour
{
    #region Variables

    public static UserInterfaceController instance;

    [Header("Setup")]
    [SerializeField] private UserInterface userInterface;
    public List<PanelButton> buttons = new List<PanelButton>();

    private readonly TestInput input = new TestInput();
    public Flags flags = new Flags(false);

    #endregion

    #region Structs

    public struct Flags
    {
        public bool isActive { get; set; }

        public Flags(bool isActive)
        {
            this.isActive = isActive;
        }
    }

    #endregion

    #region Miscellaneous Methods

    protected void GetInput()
    {
        
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected void Awake()
    {
        if (instance == null)
            instance = this;
    }

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    protected void Start()
    {
        
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    protected void Update()
    {
        if (flags.isActive)
        {
            GetInput();
        }
    }

    #endregion
}
