using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class PartyUserInterface : UserInterface
{
    #region Constants

    public override int MaxObjects => 3;

    #endregion

    #region Variables

    private List<PartyInformationController> informationPanels;
    private StatsUserInterface statsUserInterface;

    #endregion

    #region Miscellaneous Methods

    public override void UpdateSelectedObject(int selectedValue, int increment)
    {
        int previousValue = ExtensionMethods.IncrementInt(selectedValue, 0, MaxObjects, -increment);

        StartCoroutine(informationPanels[selectedValue].SetActive(true));
        StartCoroutine(informationPanels[previousValue].SetActive(false));
    }

    public void UpdateSelectedPartyMember(PartyMember member)
    {
        foreach (PartyInformationController panel in informationPanels)
        {
            panel.SetInformation(member);
        }
        
        UpdateSelectedObject(0, 1);

        statsUserInterface.SetInformation(member);
        
        //UpdateSprite(member);
    }

    public void AnimatePanel(int panel, float opacity, float animationDuration)
    {
        StartCoroutine(informationPanels[panel].gameObject.FadeOpacity(opacity, animationDuration));
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        selector = transform.Find("Selector").gameObject;
        //scrollBar = learnedMovesPanel.transform.Find("Scrollbar").GetComponent<Scrollbar>();
        //arrows = transform.Find("Arrows").gameObject;
        
        informationPanels = GetComponentsInChildren<PartyInformationController>().ToList();
        statsUserInterface = transform.Find("Stats").GetComponent<StatsUserInterface>();

        //radarChartMesh = transform.Find("Middle/Stats/Chart/Radar Mesh").GetComponent<CanvasRenderer>();

        //StartCoroutine(FindObjectOfType<BottomPanelUserInterface>().ChangePanelButtons(InventoryController.instance.buttons));

        base.Awake();
    }

    #endregion
}