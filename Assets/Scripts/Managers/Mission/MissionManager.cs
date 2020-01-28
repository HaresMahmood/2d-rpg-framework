﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class MissionManager : MonoBehaviour
{
    #region Variables

    public static MissionManager instance;

    [Header("Setup")]
    public Missions missions;
    [SerializeField] private MissionUserInterface userInterface;
    public List<PanelButton> buttons = new List<PanelButton>();

    private readonly TestInput input = new TestInput();
    public Flags flags = new Flags(false, false);

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
        bool hasInput;
        (selectedMission, hasInput) = input.GetInput("Vertical", TestInput.Axis.Vertical, missions.mission.Count, selectedMission);
        if (hasInput)
        {

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
}
