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
        int previousValue = ExtensionMethods.IncrementInt(selectedValue, 0, MaxObjects, -increment);

        StartCoroutine(informationPanels[selectedValue].SetActive(true));
        StartCoroutine(informationPanels[previousValue].SetActive(false));

        StartCoroutine(UpdateSelector(informationPanels[selectedValue].transform));
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
        
        //UpdateSprite(member);
    }

    public void AnimatePanel(int panel, float opacity, float animationDuration)
    {
        StartCoroutine(informationPanels[panel].gameObject.FadeOpacity(opacity, animationDuration));
    }

    public void AnimatePanels(PartyInformationController panel, float opacity, float animationDuration)
    {
        int index = informationPanels.IndexOf(panel);

        AnimatePanels(index, opacity, animationDuration);
    }

    private void AnimatePanels(int panel, float opacity, float animationDuration)
    {
        for (int i = 0; i < informationPanels.Count; i++)
        {
            if (i == panel)
            {
                continue;
            }
            else if (informationPanels[i].GetComponent<CanvasGroup>().alpha != 0)
            {
                AnimatePanel(i, opacity, animationDuration);
            }
        }
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

        informationPanels[0] = isActive ? learnedMovesPanel : informationPanel;

        if (isActive)
        {
            informationPanels.RemoveAt(1);

            StartCoroutine(statsPanel.SetActive(false));
        }
        else
        {
            informationPanels.Insert(1, statsPanel);
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