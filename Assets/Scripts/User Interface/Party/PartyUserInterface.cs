using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class PartyUserInterface : UserInterface
{
    #region Constants

    public override int MaxObjects => 2;

    #endregion

    #region Variables

    private List<PartyInformationController> informationPanels;

    #endregion

    #region Miscellaneous Methods

    public override void UpdateSelectedObject(int selectedValue, int previousValue)
    {
        int value = ExtensionMethods.IncrementInt(selectedValue, 0, MaxObjects, previousValue); // Debug.

        StartCoroutine(informationPanels[selectedValue].SetActive(true));
        StartCoroutine(informationPanels[value].SetActive(false));
    }

    public void UpdateSelectedPartyMember(Pokemon member)
    {
        foreach (PartyInformationController panel in informationPanels)
        {
            panel.SetInformation(member);
        }
        
        UpdateSelectedObject(0, 1);
        
        //UpdateSprite(member);
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

        //radarChartMesh = transform.Find("Middle/Stats/Chart/Radar Mesh").GetComponent<CanvasRenderer>();

        //StartCoroutine(FindObjectOfType<BottomPanelUserInterface>().ChangePanelButtons(InventoryController.instance.buttons));

        base.Awake();
    }

    #endregion
}

   /*
    #region Variables

    private GameObject indicator, arrows, movesPanel, learnedMovesPanel;

    private PartyInformationPanel[] informationPanels;

    public Transform[] selectedPanel { get; private set; }
    private Scrollbar scrollBar;
    private CanvasRenderer radarChartMesh;

    #endregion

    #region Miscellaneous Methods

    public void InitializeMenu(Party party, int selectedMember)
    {
        Pokemon member = party.playerParty[selectedMember];

        //informationPanel.UpdateInformation(member);

        //UpdateMoveInformation(member, movesPanels);
        //UpdateMoveInformation(member, learnedMovesPanels);
        //UpdateScrollbar();
        //learnedMovesPanel.SetActive(false);

        //LayoutRebuilder.ForceRebuildLayoutImmediate(informationPanel.transform.parent.GetComponent<RectTransform>());

        //UpdateSelectedPanel(0, 0);
        //StartCoroutine(UpdateSelectedSlot(0, -1, false, 0.2f));
        UpdateSelectedPanel(0, -1);
        UpdateSprite(member);
    }

    public void UpdateSelectedPanel(int selectedPanel, int previousPanel)
    {
        informationPanels[selectedPanel].SetActive(true);
        informationPanels[previousPanel].SetActive(false);
    }

    public void SetPanelStatus(int selectedPanel, bool isActive)
    {
        informationPanels[selectedPanel].SetActive(isActive);
    }

    public void FadePanel(int selectedPanel, bool isActive, float duration = 0.25f)
    {
        float opacity = isActive ? 1f : 0f;

        StartCoroutine(informationPanels[selectedPanel].gameObject.FadeOpacity(opacity, duration));
        if (selectedPanel == 2) informationPanels[selectedPanel].GetComponent<PartyLearnedMovePanel>().AnimatePanel(isActive);
    }

    public void UpdateIndicator(PartyInformationSlots[] informationSlots, int selectedSlot)
    {
        Transform margin = informationSlots[selectedSlot].transform.Find("Margin").GetComponent<RectTransform>();
        List<Transform> children = informationSlots[selectedSlot].transform.GetChildren().ToList();
        Transform panel = children.Find(x => x != margin.transform);
        Vector3 position = informationSlots[0].transform.parent.name == "Active Moves" ? new Vector3(panel.position.x + (indicator.GetComponent<RectTransform>().sizeDelta.x / 2), panel.position.y) : panel.position;

        indicator.transform.position = position;
        indicator.GetComponent<RectTransform>().sizeDelta = panel.GetComponent<RectTransform>().sizeDelta;
    }

    public void UpdateArrows(int selectedSlot)
    {
        Transform margin = selectedPanel[selectedSlot].Find("Margin").GetComponent<RectTransform>();
        List<Transform> children = selectedPanel[selectedSlot].GetChildren().ToList();
        Transform panel = children.Find(x => x != margin.transform);

        arrows.transform.position = new Vector2(panel.position.x - (arrows.GetComponent<RectTransform>().sizeDelta.x / 2), panel.position.y);
    }

    public void AnimateArrows(bool isActive, float duration = 0.15f)
    {
        float position = isActive ? -100f : 100f;
        float opacity = isActive ? 1f : 0.7f;

        StartCoroutine(arrows.transform.LerpPosition(new Vector2(arrows.transform.position.x + position, arrows.transform.position.y), duration));
        StartCoroutine(arrows.FadeOpacity(opacity, duration));
    }

    public IEnumerator FadeIndicator(bool fadeIn, float duration = 0.075f)
    {
        if (fadeIn)
        {
            StartCoroutine(indicator.FadeOpacity(1f, duration));
            yield return new WaitForSecondsRealtime(duration);
            indicator.GetComponent<Animator>().enabled = true;
        }
        else
        {
            indicator.GetComponent<Animator>().enabled = false;
            StartCoroutine(indicator.FadeOpacity(0f, duration));
            yield return new WaitForSecondsRealtime(duration);
        }
    }

    /*
    public IEnumerator SwitchMode(bool isArrangingMoves, int selectedSlot, float duration = 0.15f)
    {
        StartCoroutine(AnimateLearnedMoves(isArrangingMoves));

        float opacity = isArrangingMoves ? 0 : 1;
        FindObjectOfType<PauseUserInterface>().FadeCharacterSprite(opacity, duration);
        StartCoroutine(informationPanel.gameObject.FadeOpacity(opacity, duration));
        StartCoroutine(transform.Find("Middle/Stats").gameObject.FadeOpacity(opacity, duration));

        yield return null;
        StartCoroutine(FadeIndicator(true));
        StartCoroutine(arrows.FadeOpacity(0, duration));

        UpdateArrows(selectedSlot);
        UpdateIndicator(selectedSlot);
    }

    public void RearrangeMove(bool isRearrangingMoves, int selectedSlot, float duration = 0.15f)
    {
        float opacity = isRearrangingMoves ? 1 : 0;

        StartCoroutine(FadeIndicator(!isRearrangingMoves));
        StartCoroutine(arrows.FadeOpacity(opacity, duration));

        UpdateArrows(selectedSlot);
        UpdateIndicator(selectedSlot);
    }

    private void UpdateSprite(Pokemon pokemon)
    {
        //PauseManager.instance.pauseContainer.transform.Find("Target Sprite/Pokémon/Sprite").GetComponent<Image>().sprite = pokemon.frontSprite;
        //PauseManager.instance.pauseContainer.transform.Find("Target Sprite/Pokémon/Sprite").GetComponent<Image>().SetNativeSize();
    }

    public void Fade(float opacity)
    {
        Transform[] children = partyContainer.transform.GetChildren();

        foreach (Transform child in children)
        {
            if (child != null)
            {
                //StartCoroutine(child.gameObject.FadeOpacity(opacity, 0.1f));
            }
        }

        StartCoroutine(PauseManager.instance.pauseContainer.transform.Find("Target Sprite").gameObject.FadeOpacity(opacity, 0.1f));

    }

    private void DrawStatChart(Pokemon.Stats stats)
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[7];
        Vector2[] uv = new Vector2[7];
        int[] triangles = new int[3 * 6];

        float radarChartSize = radarChartMesh.transform.parent.GetChildren()[0].GetComponent<RectTransform>().sizeDelta.x / 2;
        //Debug.Log(radarChartSize);
        float angleIncerement = 360 / 6;

        int hpVertexIndex = 1;
        Vector3 hpVertex = Quaternion.Euler(0, 0, -angleIncerement * (hpVertexIndex - 1)) * Vector3.up * radarChartSize * ((float)stats.hp / 10);
        int attackVertexIndex = 2;
        Vector3 attackVertex = Quaternion.Euler(0, 0, -angleIncerement * (attackVertexIndex - 1)) * Vector3.up * radarChartSize * ((float)stats.attack / 10);
        int defenceVertexIndex = 3;
        Vector3 defenceVertex = Quaternion.Euler(0, 0, -angleIncerement * (defenceVertexIndex - 1)) * Vector3.up * radarChartSize * ((float)stats.defence / 10);
        int spAttackVertexIndex = 4;
        Vector3 spAttackVertex = Quaternion.Euler(0, 0, -angleIncerement * (spAttackVertexIndex - 1)) * Vector3.up * radarChartSize * ((float)stats.spAttack / 10);
        int spDefenceVertexIndex = 5;
        Vector3 spDefenceVertex = Quaternion.Euler(0, 0, -angleIncerement * (spDefenceVertexIndex - 1)) * Vector3.up * radarChartSize * ((float)stats.spDefence / 10);
        int speedVertexIndex = 6;
        Vector3 speedVertex = Quaternion.Euler(0, 0, -angleIncerement * (speedVertexIndex - 1)) * Vector3.up * radarChartSize * ((float)stats.speed / 10);

        vertices[0] = Vector3.zero;
        vertices[hpVertexIndex] = hpVertex;
        vertices[attackVertexIndex] = attackVertex;
        vertices[defenceVertexIndex] = defenceVertex;
        vertices[spAttackVertexIndex] = spAttackVertex;
        vertices[spDefenceVertexIndex] = spDefenceVertex;
        vertices[speedVertexIndex] = speedVertex;

        triangles[0] = 0;
        triangles[1] = hpVertexIndex;
        triangles[2] = attackVertexIndex;

        triangles[3] = 0;
        triangles[4] = attackVertexIndex;
        triangles[5] = defenceVertexIndex;

        triangles[6] = 0;
        triangles[7] = defenceVertexIndex;
        triangles[8] = spAttackVertexIndex;

        triangles[9] = 0;
        triangles[10] = spAttackVertexIndex;
        triangles[11] = spDefenceVertexIndex;

        triangles[12] = 0;
        triangles[13] = spDefenceVertexIndex;
        triangles[14] = speedVertexIndex;

        triangles[15] = 0;
        triangles[16] = speedVertexIndex;
        triangles[17] = hpVertexIndex;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        radarChartMesh.SetMesh(mesh);
        radarChartMesh.SetMaterial(chartMaterial, null);
    }

    public IEnumerator AnimateMove(int currentMove, int increment)
    {
        Transform thisMove = movePanels[currentMove];

        indicator.SetActive(false);

        for (int i = 0; i < totalMoves; i++)
        {
            Transform move = movePanels[i];
            Transform positioner = movePositioners[i];
            if (Array.IndexOf(movePanels, move) == selectedMove)
            {
                positioner.Find("Stats").gameObject.SetActive(true);
                positioner.Find("Description").gameObject.SetActive(true);
                LayoutRebuilder.ForceRebuildLayoutImmediate(movesPanel.GetComponent<RectTransform>());
                move.Find("Stats").gameObject.SetActive(true);
                StartCoroutine(move.Find("Stats").gameObject.FadeOpacity(1f, 0.3f));
                move.Find("Description").gameObject.SetActive(true);
                StartCoroutine(move.Find("Description").gameObject.FadeOpacity(1f, 0.3f));
            }
            else
            {
                positioner.Find("Stats").gameObject.SetActive(false);
                positioner.Find("Description").gameObject.SetActive(false);
                LayoutRebuilder.ForceRebuildLayoutImmediate(movesPanel.GetComponent<RectTransform>());
                move.Find("Stats").gameObject.SetActive(false);
                move.Find("Stats").GetComponent<CanvasGroup>().alpha = 0;
                move.Find("Description").gameObject.SetActive(false);
                move.Find("Description").GetComponent<CanvasGroup>().alpha = 0;
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(movesPanel.GetComponent<RectTransform>());

        thisMove.GetComponent<Animator>().SetBool("isActive", true);
        Transform previousMove = movePanels[ExtensionMethods.IncrementInt(currentMove, 0, movePanels.Length, increment)];

        previousMove.GetComponent<Animator>().SetBool("isActive", false);

        yield return new WaitForSecondsRealtime(0.15f);

        if (!PauseManager.instance.inPartyMenu)
        {
            indicator.SetActive(true);
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        indicator = transform.Find("Indicator").gameObject;
        arrows = transform.Find("Arrows").gameObject;
        //scrollBar = learnedMovesPanel.transform.Find("Scrollbar").GetComponent<Scrollbar>();

        informationPanels = GetComponentsInChildren<PartyInformationPanel>();

        radarChartMesh = transform.Find("Middle/Stats/Chart/Radar Mesh").GetComponent<CanvasRenderer>();

        InitializeMenu(PartyManager.instance.party, 0);
    }

    #endregion
    */