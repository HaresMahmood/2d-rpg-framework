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
public class PartyController : MonoBehaviour
{
    /*
  #region Variables

  public static PartyManager instance;

  [Header("Setup")]
  public Party party;
  [SerializeField] private PartyUserInterface userInterface;
  public List<PanelButton> buttons = new List<PanelButton>();

  [Header("Settings")]
  [SerializeField] private Material chartMaterial;

  private readonly TestInput input = new TestInput();
  public Flags flags = new Flags(false, false);

  public GameObject pauseContainer { get; private set; }

  public int selectedMember { get; private set; }
  private int selectedPanel;

  //public event EventHandler OnUserInput = delegate { };

  #endregion

  #region Structs

  public struct Flags
  {
      public bool isActive { get; set; }
      public bool isViewingAllMoves { get; set; }

      public Flags(bool isActive, bool isArrangingMoves)
      {
          this.isActive = isActive;
          this.isViewingAllMoves = isArrangingMoves;
      }
  }

  #endregion

  #region Accessor Methods

  public PartyUserInterface GetUserInterface()
  {
      return userInterface;
  }

  #endregion

  #region Miscellaneous Methods


  private void UpdateSelectedPanel(bool arrows = false)
  {
      int selectedSlot = selectedPanel == 0 ? selectedMove : selectedInformation;
      if (selectedPanel == 2)
      {
          selectedSlot = selectedPanel == 2 ? selectedMove : selectedLearnedMove;
      }

      userInterface.AnimateSlot(selectedSlot, false);
      userInterface.UpdateSelectedPanel(selectedPanel);
      if (arrows) userInterface.AnimateArrows(selectedPanel == 2 ? false : true);

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
      UpdateSelectedPanel(true); yield return null;
      StartCoroutine(userInterface.UpdateSelectedSlot(0, -1, true));
  }
  */

    /*
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


    private void GetInput()
    {
        bool hasInput;
        //int previousPanel = selectedPanel;

        (selectedPanel, hasInput) = input.GetInput("Horizontal", TestInput.Axis.Horizontal, 2, selectedPanel);
        if (hasInput)
        {
            selectedPanel = flags.isViewingAllMoves ? (selectedPanel == 0 ? 2 : selectedPanel) : (selectedPanel == 2 ? 0 : selectedPanel);
            int previousPanel = flags.isViewingAllMoves ? (ExtensionMethods.IncrementInt(selectedPanel, 1, 3, (int)Input.GetAxisRaw("Horizontal"))) : (ExtensionMethods.IncrementInt(selectedPanel, 0, 2, (int)Input.GetAxisRaw("Horizontal")));
            userInterface.UpdateSelectedPanel(selectedPanel, previousPanel);
            selectedPanel = flags.isViewingAllMoves ? (selectedPanel == 2 ? 0 : selectedPanel) : selectedPanel;
        }

        if (Input.GetButtonDown("Toggle"))
        {
            flags.isViewingAllMoves = !flags.isViewingAllMoves;

            if (selectedPanel == 0 || selectedPanel == 2)
            {
                userInterface.UpdateSelectedPanel(1, selectedPanel);
                userInterface.SetPanelStatus(selectedPanel, false);
                userInterface.FadePanel(selectedPanel, false);
                selectedPanel = 1;
            }

            userInterface.FadePanel(2, flags.isViewingAllMoves);
            userInterface.FadePanel(0, !flags.isViewingAllMoves);
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
    }

    #endregion
    */
}
