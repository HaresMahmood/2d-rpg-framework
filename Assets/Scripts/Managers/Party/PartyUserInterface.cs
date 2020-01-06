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

    private GameObject indicator, movesPanel;
    private Transform[] informationPanels, movePanels;
    private CanvasRenderer radarChartMesh;

    private MemberInformation informationPanel;

    #endregion

    #region Miscellaneous Methods

    public void UpdateInformation(Party party, int selectedMember)
    {
        Pokemon member = party.playerParty[selectedMember];

        informationPanel.UpdateInformation(member);
        informationPanels[0].GetComponent<InformationContainer>().UpdatePanel(true);
        informationPanels[1].GetComponent<InformationContainer>().UpdatePanel(false);
        informationPanels[2].GetComponent<InformationContainer>().UpdatePanel(false);
        informationPanels[3].GetComponent<InformationContainer>().UpdatePanel(false);

        for (int i = 0; i < movePanels.Length; i++)
        {
            movePanels[i].GetComponent<MoveSlot>().UpdateInformation(member.learnedMoves[i]);
            movePanels[i].GetComponent<InformationContainer>().UpdatePanel(false);
        }

        movePanels[0].GetComponent<InformationContainer>().UpdatePanel(true);

        DrawSprite(member);
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
        movesPanel = transform.Find("Middle/Moves").gameObject;
        informationPanel = transform.Find("Middle/Information").GetComponent<MemberInformation>();

        informationPanels = informationPanel.transform.GetChildren();
        movePanels = movesPanel.transform.GetChildren();

        radarChartMesh = transform.Find("Middle/Stats/Chart/Radar Mesh").GetComponent<CanvasRenderer>();

        UpdateInformation(PartyManager.instance.party, 0);
    }

    #endregion
}
