using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// </summary>
public class PartyUserInterface : MonoBehaviour
{
    #region Variables

    private GameObject indicator, arrows, movesPanel;
    private Transform[] informationPanels, movesPanels, selectedPanel;
    private CanvasRenderer radarChartMesh;

    private MemberInformation informationPanel;

    #endregion

    #region Miscellaneous Methods

    public void InitializeMenu(Party party, int selectedMember)
    {
        Pokemon member = party.playerParty[selectedMember];

        informationPanel.UpdateInformation(member);
        informationPanels[0].GetComponent<InformationContainer>().UpdatePanel(true);
        informationPanels[1].GetComponent<InformationContainer>().UpdatePanel(false);
        informationPanels[2].GetComponent<InformationContainer>().UpdatePanel(false);
        informationPanels[3].GetComponent<InformationContainer>().UpdatePanel(false);

        UpdateMoveInformation(member);

        movesPanels[0].GetComponent<InformationContainer>().UpdatePanel(true);
        movesPanels[0].GetComponent<InformationContainer>().AnimatePanel(false);

        LayoutRebuilder.ForceRebuildLayoutImmediate(informationPanel.transform.parent.GetComponent<RectTransform>());

        UpdateSelectedPanel(0, 0);
        StartCoroutine(UpdateSelectedSlot(0, -1, 0.2f));
        DrawSprite(member);
    }

    public void UpdateMoveInformation(Pokemon member)
    {
        for (int i = 0; i < movesPanels.Length; i++)
        {
            movesPanels[i].GetComponentInChildren<MoveSlot>().UpdateInformation(member.learnedMoves[i]);
            movesPanels[i].GetComponent<InformationContainer>().UpdatePanel(false);
        }
    }

    public void UpdateIndicator(int selectedSlot)
    {
        Transform margin = selectedPanel[selectedSlot].Find("Margin").GetComponent<RectTransform>();
        List<Transform> children = selectedPanel[selectedSlot].GetChildren().ToList();
        Transform panel = children.Find(x => x != margin.transform);

        indicator.transform.position = panel.position;
        indicator.GetComponent<RectTransform>().sizeDelta = panel.GetComponent<RectTransform>().sizeDelta;
    }

    public void UpdateArrows(int selectedSlot)
    {
        Transform margin = selectedPanel[selectedSlot].Find("Margin").GetComponent<RectTransform>();
        List<Transform> children = selectedPanel[selectedSlot].GetChildren().ToList();
        Transform panel = children.Find(x => x != margin.transform);

        arrows.transform.position = new Vector2(arrows.transform.position.x, panel.position.y);
    }

    private IEnumerator FadeIndicator(bool fadeIn, float duration = 0.075f)
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

    public void UpdateSelectedPanel(int selectedPanel, int selectedSlot, float duration = 0.3f)
    {
        StartCoroutine(this.selectedPanel[0].parent.gameObject.FadeOpacity(0.7f, duration));

        this.selectedPanel = selectedPanel == 0 ? informationPanels : movesPanels;
        StartCoroutine(this.selectedPanel[0].parent.gameObject.FadeOpacity(1f, duration));
    }

    private void UpdateSelectedSlot(int selectedSlot, int increment)
    {
        int previousSlot = ExtensionMethods.IncrementInt(selectedSlot, 0, 4, increment);

        selectedPanel[selectedSlot].GetComponent<InformationContainer>().UpdatePanel(true);
        selectedPanel[previousSlot].GetComponent<InformationContainer>().UpdatePanel(false);
    }

    public void AnimateSlot(int selectedSlot, bool isSelected)
    {
        selectedPanel[selectedSlot].GetComponent<InformationContainer>().AnimatePanel(isSelected);
    }

    public IEnumerator UpdateSelectedSlot(int selectedSlot, int increment, float duration = 0.15f)
    {
        StartCoroutine(FadeIndicator(false));
        yield return new WaitForSecondsRealtime(duration / 2);
        UpdateSelectedSlot(selectedSlot, increment); yield return new WaitForSecondsRealtime(duration);
        UpdateIndicator(selectedSlot);
        StartCoroutine(FadeIndicator(true));
    }

    public void UpdateMovePosition(Party party, int selectedMember, int selectedSlot, int increment)
    {
        int previousSlot = ExtensionMethods.IncrementInt(selectedSlot, 0, 4, increment);
        Pokemon member = party.playerParty[selectedMember];
        List<Pokemon.LearnedMove> moves = member.learnedMoves;

        Pokemon.LearnedMove move = moves[previousSlot];
        moves.Remove(move);
        moves.Insert(selectedSlot, move);

        UpdateMoveInformation(member);

        UpdateSelectedSlot(selectedSlot, increment);
    }

    public void SwitchMode(bool isArrangingMoves, int selectedSlot, float duration = 0.15f)
    {
        float opacity = isArrangingMoves ? 1 : 0;

        StartCoroutine(FadeIndicator(!isArrangingMoves));
        StartCoroutine(arrows.FadeOpacity(opacity, duration));

        UpdateArrows(selectedSlot);
        UpdateIndicator(selectedSlot);
    }

    private void DrawSprite(Pokemon pokemon)
    {
        PauseManager.instance.pauseContainer.transform.Find("Target Sprite/Pokémon/Sprite").GetComponent<Image>().sprite = pokemon.frontSprite;
        PauseManager.instance.pauseContainer.transform.Find("Target Sprite/Pokémon/Sprite").GetComponent<Image>().SetNativeSize();
    }

    /*
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
    */

    #endregion

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        indicator = transform.Find("Indicator").gameObject;
        arrows = transform.Find("Arrows").gameObject;
        movesPanel = transform.Find("Middle/Moves").gameObject;
        informationPanel = transform.Find("Middle/Information").GetComponent<MemberInformation>();

        informationPanels = informationPanel.transform.GetChildren();
        movesPanels = movesPanel.transform.GetChildren();
        selectedPanel = movesPanels;

        radarChartMesh = transform.Find("Middle/Stats/Chart/Radar Mesh").GetComponent<CanvasRenderer>();

        InitializeMenu(PartyManager.instance.party, 0);
    }

    #endregion
}
