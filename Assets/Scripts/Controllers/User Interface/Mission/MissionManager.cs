using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class MissionManager : MonoBehaviour
{
    /*
    #region Variables

    public static MissionManager instance;

    [Header("Setup")]
    public Missions missions;
    [SerializeField] private MissionUserInterface userInterface;
    public List<PanelButton> buttons = new List<PanelButton>();

    private readonly TestInput input = new TestInput();
    public Flags flags = new Flags(false, false);

    public List<string> categoryNames { get; private set; } = new List<string>();

    private int selectedMission = 0;
    private int selectedCategory;

    #endregion

    #region Structs

    public struct Flags
    {
        public bool isActive { get; set; }
        public bool isMissionSelected { get; set; }

        public Flags(bool isActive, bool isMissionSelected)
        {
            this.isActive = isActive;
            this.isMissionSelected = isMissionSelected;
        }
    }

    #endregion

    #region Miscellaneous Methods

    private void GetInput()
    {
        if (Input.GetAxisRaw("Trigger") == 0) // TODO: Very ugly!
        {
            bool hasInput;
            (selectedMission, hasInput) = input.GetInput("Horizontal", "Vertical", userInterface.categoryMissions.Count, selectedMission, false, 7, 1);
            if (hasInput)
            {
                userInterface.UpdateSelectedSlot(selectedMission);
            }
        }
        else
        {
            bool hasInput;
            (selectedCategory, hasInput) = input.GetInput("Trigger", TestInput.Axis.Horizontal, categoryNames.Count, selectedCategory);
            if (hasInput)
            {
                selectedMission = 0;
                userInterface.UpdateSelectedCategory(missions, selectedCategory, (int)Input.GetAxisRaw("Trigger"));
            }
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        for (int i = 0; i < Enum.GetNames(typeof(Mission.Category)).Length; i++)
        {
            //categoryNames.Add(((Mission.Category)i).ToString());
        }
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        if (PauseManager.instance.flags.isActive && flags.isActive)
        {
            GetInput();
        }
    }

    #endregion
    */
}