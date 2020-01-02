using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// </summary>
public class PartyManager : MonoBehaviour
{
    
    #region Variables

    private int lastInput;

    #endregion

    #region Variables

    public static PartyManager instance;

    [Header("Setup")]
    public Party party;
    [SerializeField] private PartyUserInterface userInterface;
    public List<PanelButton> buttons = new List<PanelButton>();

    [UnityEngine.Header("Settings")]
    [SerializeField] private Material chartMaterial;

    private readonly TestInput input = new TestInput();
    public Flags flags = new Flags(false);

    public GameObject pauseContainer { get; private set; }

    private int selectedMove;

    //public event EventHandler OnUserInput = delegate { };

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

    private void GetInput()
    {
        int totalMoves = party.playerParty[PauseManager.instance.selectedSlot].learnedMoves.Count;
        bool hasInput;

        (selectedMove, hasInput) = input.GetInput("Vertical", TestInput.Axis.Vertical, totalMoves, selectedMove);
        if (hasInput)
        {
            // Update moveslot (animate)
        }
        /*
        if ((int)Input.GetAxisRaw("Vertical") != lastInput)
        {
            lastInput = (int)Input.GetAxisRaw("Vertical");
        }
        */
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

        /*
        if (PauseManager.instance.selectedSlot < party.playerParty.Count)
        {
            currentPokemon = party.playerParty[PauseManager.instance.selectedSlot];
        }

        currentMove = currentPokemon.learnedMoves[selectedMove].move;

        DrawParty(currentPokemon);

        indicator.transform.position = movePanels[selectedMove].position;
        indicator.GetComponent<RectTransform>().sizeDelta = movePanels[selectedMove].GetComponent<RectTransform>().sizeDelta;
        if (PauseManager.instance.inPartyMenu)
        {
            indicator.SetActive(false);
        }

        if (PauseManager.instance.isPaused)
        {
            GetInput();
            DrawStatChart(currentPokemon.stats);
        }
        else if (!PauseManager.instance.isPaused && isActive)
        {
            radarChartMesh.gameObject.SetActive(false);
        }
        */
    }

    #endregion
}
