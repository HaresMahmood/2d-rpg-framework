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

    public static PartyManager instance;

    [Header("Setup")]
    public Party party;
    [SerializeField] private PartyUserInterface userInterface;
    public List<PanelButton> buttons = new List<PanelButton>();

    [Header("Settings")]
    [SerializeField] private Material chartMaterial;

    private readonly TestInput input = new TestInput();
    public Flags flags = new Flags(false, false, false);

    public GameObject pauseContainer { get; private set; }

    private int selectedInformation;
    private int selectedMove;
    private int selectedLearnedMove;
    private int selectedPanel;

    //public event EventHandler OnUserInput = delegate { };

    #endregion

    #region Structs

    public struct Flags
    {
        public bool isActive { get; set; }
        public bool isViewingAllMoves { get; set; }
        public bool isRearrangingMoves { get; set; }

        public Flags(bool isActive, bool isArrangingMoves, bool isRearrangingMoves)
        {
            this.isActive = isActive;
            this.isViewingAllMoves = isArrangingMoves;
            this.isRearrangingMoves = isRearrangingMoves;

        }
    }

    #endregion

    #region Miscellaneous Methods

    private void UpdateSelectedPanel()
    {
        int selectedSlot = selectedPanel == 0 ? selectedMove : selectedInformation;
        if (selectedPanel == 2)
        {
            selectedSlot = selectedPanel == 2 ? selectedMove : selectedLearnedMove;
        }

        userInterface.AnimateSlot(selectedSlot, false);
        userInterface.UpdateSelectedPanel(selectedPanel);

        if (selectedPanel != 2)
        {
            selectedSlot = selectedPanel == 0 ? selectedInformation : selectedMove;
        }
        else
        {
            selectedSlot = selectedLearnedMove;
        }

        StartCoroutine(userInterface.UpdateSelectedSlot(selectedSlot, -1));
    }

    private IEnumerator UpdateMovePosition(int selectedMember, int selectedMove, int increment)
    {
        userInterface.UpdateMovePosition(party, selectedMember, selectedMove, increment);
        yield return null;
        userInterface.UpdateArrows(selectedMove);
    }

    private IEnumerator SwapMove()
    {
        userInterface.SwapMove(party.playerParty[0], selectedMove, selectedLearnedMove);
        selectedPanel = 1; selectedLearnedMove = 0;
        UpdateSelectedPanel();
        yield return new WaitForSecondsRealtime(0.15f);
        userInterface.RearrangeMove(flags.isRearrangingMoves, selectedMove);
    }

    private IEnumerator EnableLearnedMovePanel()
    {
        flags.isRearrangingMoves = false;
        selectedPanel = 2;
        UpdateSelectedPanel(); yield return null;
        StartCoroutine(userInterface.UpdateSelectedSlot(0, -1, true));
    }

    private void GetInput()
    {
        if (!flags.isViewingAllMoves)
        {
            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                bool hasInput;
                int selectedSlot = selectedPanel == 0 ? selectedInformation : selectedMove;

                (selectedSlot, hasInput) = input.GetInput("Vertical", TestInput.Axis.Vertical, 4, selectedSlot);
                if (hasInput)
                {
                    if (selectedPanel == 0)
                    {
                        selectedInformation = selectedSlot;
                    }
                    else
                    {
                        selectedMove = selectedSlot;
                    }

                    StartCoroutine(userInterface.UpdateSelectedSlot(selectedSlot, (int)Input.GetAxisRaw("Vertical")));
                }
            }
            else
            {
                bool hasInput;

                (selectedPanel, hasInput) = input.GetInput("Horizontal", TestInput.Axis.Horizontal, 2, selectedPanel);
                if (hasInput)
                {
                    UpdateSelectedPanel();
                }
            }
        }
        else
        {
            if (Input.GetButtonDown("Interact"))
            {
                if (selectedPanel != 2)
                {
                    flags.isRearrangingMoves = !flags.isRearrangingMoves;
                    userInterface.RearrangeMove(flags.isRearrangingMoves, selectedMove);
                }
                else
                {
                    StartCoroutine(SwapMove());
                }
            }

            if (Input.GetButtonDown("Remove") && flags.isRearrangingMoves)
            {
                StartCoroutine(EnableLearnedMovePanel());
            }

            bool hasInput;
            int selectedSlot = selectedPanel == 2 ? selectedLearnedMove : selectedMove;

            (selectedSlot, hasInput) = input.GetInput("Vertical", TestInput.Axis.Vertical, userInterface.selectedPanel.Length, selectedSlot);
            if (hasInput)
            {

                if (selectedPanel == 2)
                {
                    selectedLearnedMove = selectedSlot;
                }
                else
                {
                    selectedMove = selectedSlot;
                }

                if (flags.isRearrangingMoves)
                {
                    StartCoroutine(UpdateMovePosition(0, selectedSlot, (int)Input.GetAxisRaw("Vertical")));
                }
                else
                {
                    StartCoroutine(userInterface.UpdateSelectedSlot(selectedSlot, (int)Input.GetAxisRaw("Vertical"), true));
                }
            }
        }

        if (Input.GetButtonDown("Toggle"))
        {
            if (selectedPanel == 1)
            {
                flags.isViewingAllMoves = !flags.isViewingAllMoves;
                StartCoroutine(userInterface.SwitchMode(flags.isViewingAllMoves, selectedMove));
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
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        if (PauseManager.instance.flags.isActive) // && flags.isActive
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
