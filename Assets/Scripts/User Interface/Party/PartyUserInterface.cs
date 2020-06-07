using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class PartyUserInterface : UserInterface
{
    #region Constants

    public override int MaxObjects => informationPanels.Count - 1;

    #endregion

    #region Variables

    private List<PartyInformationController> informationPanels;
    private PartyLearnedMovesController learnedMovesPanel;
    private PartyInformationController informationPanel;
    private StatsController statsPanel;

    private StatsUserInterface statsUserInterface;

    private PartyMember selectedMember;

    #endregion

    #region Miscellaneous Methods

    public override void UpdateSelectedObject(int selectedValue, int increment)
    {
        int previousValue = ExtensionMethods.IncrementInt(selectedValue, 0, MaxObjects, -increment);

        // TODO: Debug
        if (informationPanels[previousValue] is PartyMovesPanelController && informationPanels[selectedValue] is PartyMovesPanelController && ((PartyMovesPanelController)informationPanels[previousValue]).IsActive)
        {
            if (((PartyMovesPanelController)informationPanels[selectedValue]).CanInsertMove(selectedMember) && ((PartyMovesPanelController)informationPanels[previousValue]).CanRemoveMove(selectedMember))
            {
                ((PartyMovesPanelController)informationPanels[selectedValue]).InsertMove(selectedMember, ((PartyMovesPanelController)informationPanels[previousValue]).GetSelectedMove(selectedMember));
                ((PartyMovesPanelController)informationPanels[previousValue]).RemoveMove(selectedMember);

                ((PartyMovesPanelController)informationPanels[selectedValue]).IsActive = ((PartyMovesPanelController)informationPanels[previousValue]).IsActive;
            }
        }
        else if (informationPanels[previousValue] is PartyMovesPanelController && (!(informationPanels[selectedValue] is PartyMovesPanelController)))
        {
            //AnimatePanels(informationPanels[previousValue], 1f, false);
            UpdateSelector(false);
        }

        StartCoroutine(informationPanels[selectedValue].SetActive(true));
        StartCoroutine(informationPanels[previousValue].SetActive(false));

        StartCoroutine(UpdateSelector(new Vector2(informationPanels[selectedValue].transform.position.x, selector.transform.position.y)));
    } 

    public void UpdateSelector(bool isActive, float animationDuration = 0.15f)
    {
        Color color = isActive ? GameManager.instance.oppositeColor : Color.white;

        StartCoroutine(selector.FadeColor(color, animationDuration));
    }

    public void UpdateSelectedPartyMember(PartyMember member)
    {
        foreach (PartyInformationController panel in informationPanels)
        {
            panel.SetInformation(member);
        }

        learnedMovesPanel.SetInformation(member);

        UpdateSelectedObject(0, 1);

        statsUserInterface.SetInformation(member);

        SetCharacterSprite(member.Species.Sprites.FrontSprite);

        selectedMember = member;
    }

    public void AnimatePanel(int panel, float opacity, float animationDuration)
    {
        StartCoroutine(informationPanels[panel].gameObject.FadeOpacity(opacity, animationDuration));
    }

    public void AnimatePanels(PartyInformationController panel, float opacity, bool condition, float animationDuration = 0.15f) // TODO: Weird bool name
    {
        List<PartyInformationController> nonSelectedPanels = informationPanels.Where(p => p != panel && p.GetComponent<CanvasGroup>().alpha != 0).ToList();

        if (nonSelectedPanels.Contains(learnedMovesPanel) && !condition)
        {
            nonSelectedPanels.Remove(learnedMovesPanel);
        }

        foreach (PartyInformationController informationPanel in nonSelectedPanels)
        {
            AnimatePanel(informationPanel, opacity, animationDuration);
        }

        if (informationPanel.GetComponent<CanvasGroup>().alpha != 0 && condition)
        {
            FadeCharacterSprite(opacity, animationDuration);
        }

        /*
        if (panel.GetComponent<CanvasGroup>().alpha != (opacity != 1f ? 1f : opacity))
        {
            AnimatePanel(panel, opacity != 1f ? 1f : opacity, animationDuration);
        }
        */
    }

    private void AnimatePanel(PartyInformationController panel, float opacity, float animationDuration)
    {
        int index = informationPanels.IndexOf(panel);

        AnimatePanel(index, opacity, animationDuration);
    }

    public void AnimatePanel(GameObject panel, float opacity, float animationDuration)
    {
        StartCoroutine(panel.FadeOpacity(opacity, animationDuration));
    }

    public bool AnimatePanel(float animationDuration = 0.15f)
    {
        //bool active = 

        bool isActive = !learnedMovesPanel.GetComponent<Animator>().GetBool("isActive");

        float opacity = isActive ? 0f : 1f;

        learnedMovesPanel.AnimatePanel(isActive);
        AnimatePanel(statsPanel.gameObject, opacity, animationDuration);
        AnimatePanel(informationPanel.gameObject, opacity, animationDuration);
        //FadeCharacterSprite(opacity, animationDuration);

        informationPanels[0] = isActive ? learnedMovesPanel : informationPanel;

        if (isActive)
        {
            informationPanels.RemoveAt(1);

            StartCoroutine(statsPanel.SetActive(false));
        }
        else
        {
            informationPanels.Insert(1, statsPanel);

            StartCoroutine(learnedMovesPanel.SetActive(false));
        }

        return isActive;
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        selector = transform.Find("Selector").gameObject;
        
        informationPanels = GetComponentsInChildren<PartyInformationController>().ToList();
        learnedMovesPanel = transform.Find("Learned Moves").GetComponent<PartyLearnedMovesController>();
        informationPanel = informationPanels[0];
        statsPanel = (StatsController)informationPanels[1];

        statsUserInterface = transform.Find("Stats/Stats").GetComponent<StatsUserInterface>();

        //StartCoroutine(FindObjectOfType<BottomPanelUserInterface>().ChangePanelButtons(InventoryController.instance.buttons));

        base.Awake();
    }

    #endregion
}