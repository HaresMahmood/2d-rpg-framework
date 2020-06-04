using System.Linq;
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

    #endregion

    #region Miscellaneous Methods

    public override void UpdateSelectedObject(int selectedValue, int increment)
    {
        bool selectedCondition = true;
        bool previousCondition = true;

        int previousValue = ExtensionMethods.IncrementInt(selectedValue, 0, MaxObjects, -increment);

        if (informationPanels[previousValue] is PartyMovesPanelController)
        {
            if (informationPanels[selectedValue] is PartyMovesPanelController)
            {
                ((PartyMovesPanelController)informationPanels[selectedValue]).IsActive = ((PartyMovesPanelController)informationPanels[previousValue]).IsActive;

                previousCondition = false;
            }
            else
            {
                ((PartyMovesPanelController)informationPanels[previousValue]).IsActive = false;

                selectedCondition = false;
            }
        }

        StartCoroutine(informationPanels[selectedValue].SetActive(true, selectedCondition));
        StartCoroutine(informationPanels[previousValue].SetActive(false, previousCondition));

        StartCoroutine(UpdateSelector(informationPanels[selectedValue].transform));
    } 

    public void UpdateSelector(bool isActive, float animationDuration)
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
    }

    public void AnimatePanel(int panel, float opacity, float animationDuration)
    {
        StartCoroutine(informationPanels[panel].gameObject.FadeOpacity(opacity, animationDuration));
    }

    public void AnimatePanels(PartyInformationController panel, float opacity, float animationDuration)
    {
        List<PartyInformationController> nonSelectedPanels = informationPanels.Where(p => p != panel && p.GetComponent<CanvasGroup>().alpha != 0).ToList();

        foreach (PartyInformationController informationPanel in nonSelectedPanels)
        {
            AnimatePanel(informationPanel, opacity, animationDuration);
        }

        if (informationPanel.GetComponent<CanvasGroup>().alpha != 0)
        {
            FadeCharacterSprite(opacity, animationDuration);
        }

        if (panel.GetComponent<CanvasGroup>().alpha != (opacity != 1f ? 1f : opacity))
        {
            AnimatePanel(panel, opacity != 1f ? 1f : opacity, animationDuration);
        }
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

        statsUserInterface = transform.Find("Stats").GetComponent<StatsUserInterface>();

        //StartCoroutine(FindObjectOfType<BottomPanelUserInterface>().ChangePanelButtons(InventoryController.instance.buttons));

        base.Awake();
    }

    #endregion
}