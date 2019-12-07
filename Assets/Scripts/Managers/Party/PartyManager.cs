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

    [UnityEngine.Header("Setup")]
    public Party party;

    [UnityEngine.Header("Settings")]
    [SerializeField] private Material chartMaterial;

    [HideInInspector] public GameObject partyContainer, indicator;
    private GameObject informationContainer, movesContainer;
    private Transform[] movePanels, movePositioners;
    private CanvasRenderer radarChartMesh;
    private Pokemon currentPokemon;
    private Move currentMove;

    [HideInInspector] public int selectedMove, totalMoves, lastInput;

    [HideInInspector] public bool isActive, isDrawing = false;
    private bool isInteracting = false;

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
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        partyContainer = PauseManager.instance.pauseContainer.transform.Find("Party").gameObject;
        informationContainer = partyContainer.transform.Find("Information").gameObject;
        movesContainer = partyContainer.transform.Find("Moves").gameObject;
        indicator = partyContainer.transform.Find("Indicator").gameObject;

        Transform[] panelContainers = movesContainer.transform.GetChildren();
        movePanels = panelContainers.Where((x, i) => i % 2 == 0).ToArray();
        movePositioners = panelContainers.Where((x, i) => i % 2 != 0).ToArray();

        radarChartMesh = partyContainer.transform.Find("Stats/Chart/Radar Mesh").GetComponent<CanvasRenderer>();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
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

    private void DrawParty(Pokemon pokemon)
    {
        if (!isDrawing)
        {
            DrawInformation(pokemon);
            for (int i = 0; i < movePanels.Length; i++)
            {
                DrawMove(i, currentPokemon.learnedMoves[i]);
            }
            DrawSprite(currentPokemon);
            isDrawing = true;
        }
    }

    private void DrawInformation(Pokemon pokemon)
    {
        Transform generalInfo = informationContainer.transform.Find("General Information");
        Transform progress = informationContainer.transform.Find("Progress/Progress");
        Transform item = informationContainer.transform.Find("Held Item/Held Item");

        generalInfo.Find("Name/Name").GetComponent<TextMeshProUGUI>().SetText(pokemon.name);
        generalInfo.Find("Name/Dex Information/Information").GetComponent<TextMeshProUGUI>().SetText(pokemon.category.ToUpper() + " <color=#696969>POKÉMON  -  #</color>00" + pokemon.id);
        generalInfo.Find("Status/Status Ailment").GetComponent<TextMeshProUGUI>().SetText(pokemon.status.ToString());
        switch (pokemon.status)
        {
            case Pokemon.Status.Paralyzed:
                {
                    generalInfo.Find("Status/Status Ailment").GetComponent<TextMeshProUGUI>().color = "FCFF83".ToColor();
                    break;
                }
            case Pokemon.Status.Burned:
                {
                    generalInfo.Find("Status/Status Ailment").GetComponent<TextMeshProUGUI>().color = "FF9D83".ToColor();
                    break;
                }
            case Pokemon.Status.Frozen:
                {
                    generalInfo.Find("Status/Status Ailment").GetComponent<TextMeshProUGUI>().color = "A0FCFF".ToColor();
                    break;
                }
            case Pokemon.Status.Poisoned:
                {
                    generalInfo.Find("Status/Status Ailment").GetComponent<TextMeshProUGUI>().color = "F281FF".ToColor();
                    break;
                }
            case Pokemon.Status.Asleep:
                {
                    generalInfo.Find("Status/Status Ailment").GetComponent<TextMeshProUGUI>().color = "B0B0B0".ToColor();
                    break;
                }
            default:
                {
                    generalInfo.Find("Status/Status Ailment").GetComponent<TextMeshProUGUI>().color = Color.white;
                    break;
                }
        }
        progress.Find("Level/Level").GetComponent<TextMeshProUGUI>().SetText(pokemon.level.ToString());
        progress.Find("Experience/Experience/Exp").GetComponent<TextMeshProUGUI>().SetText(pokemon.exp.ToString());
        progress.Find("Experience/Experience/To Next").GetComponent<TextMeshProUGUI>().SetText("TO NEXT   " + (pokemon.totalExp - pokemon.exp).ToString());
        StartCoroutine(progress.Find("Experience/Experience/Bar/Amount").LerpScale(new Vector2((pokemon.exp / pokemon.totalExp), 1), 0.3f));
        item.Find("Item Name").GetComponent<TextMeshProUGUI>().SetText(pokemon.heldItem.name);
        item.Find("Item Sprite").GetComponent<Image>().sprite = pokemon.heldItem.sprite;
    }

    private void DrawMove(int selectedMove, Pokemon.LearnedMove learnedMove)
    {
        Transform info = movePanels[selectedMove].transform.Find("Information");
        Transform stats = movePanels[selectedMove].transform.Find("Stats");
        Transform description = movePanels[selectedMove].transform.Find("Description/Description");

        info.Find("Name").GetComponent<TextMeshProUGUI>().SetText(learnedMove.move.name);
        info.Find("Typing/Typing").GetComponent<TextMeshProUGUI>().SetText(learnedMove.move.type.ToString());
        info.Find("Typing/Typing").GetComponent<TextMeshProUGUI>().color = learnedMove.move.UIColor;
        info.Find("PP/PP").GetComponent<TextMeshProUGUI>().SetText(learnedMove.remainingPp.ToString() + "/" + learnedMove.move.pp);
        if (learnedMove.move.category == Move.Category.Physical)
        {
            stats.Find("Category/Category/Physical").gameObject.SetActive(true);
            stats.Find("Category/Category/Special").gameObject.SetActive(false);
        }
        else if (learnedMove.move.category == Move.Category.Special)
        {
            stats.Find("Category/Category/Special").gameObject.SetActive(true);
            stats.Find("Category/Category/Physical").gameObject.SetActive(false);
        }
        stats.Find("Accuracy/Accuracy").GetComponent<TextMeshProUGUI>().SetText(learnedMove.move.accuracy.ToString());
        stats.Find("Power/Power").GetComponent<TextMeshProUGUI>().SetText(learnedMove.move.power.ToString());
        description.GetComponent<TextMeshProUGUI>().SetText(learnedMove.move.description);

        movePanels[selectedMove].GetComponent<Image>().color = learnedMove.move.UIColor;
    }

    private void DrawSprite(Pokemon pokemon)
    {
        PauseManager.instance.pauseContainer.transform.Find("Target Sprite/Pokémon/Sprite").GetComponent<Image>().sprite = pokemon.frontSprite;
        PauseManager.instance.pauseContainer.transform.Find("Target Sprite/Pokémon/Sprite").GetComponent<Image>().SetNativeSize();
    }

    public void Fade(float opacity)
    {
        /*
        Transform[] children = partyContainer.transform.GetChildren();

        foreach (Transform child in children)
        {
            if (child != null)
            {
                //StartCoroutine(child.gameObject.FadeOpacity(opacity, 0.1f));
            }
        }

        StartCoroutine(PauseManager.instance.pauseContainer.transform.Find("Target Sprite").gameObject.FadeOpacity(opacity, 0.1f));
        */
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
        Vector3 hpVertex = Quaternion.Euler(0, 0, -angleIncerement * (hpVertexIndex - 1)) * Vector3.up  * radarChartSize * ((float)stats.hp / 10);
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

        vertices[0]                     = Vector3.zero;
        vertices[hpVertexIndex]         = hpVertex;
        vertices[attackVertexIndex]     = attackVertex;
        vertices[defenceVertexIndex]    = defenceVertex;
        vertices[spAttackVertexIndex]   = spAttackVertex;
        vertices[spDefenceVertexIndex]  = spDefenceVertex;
        vertices[speedVertexIndex]      = speedVertex;

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

        mesh.vertices   = vertices;
        mesh.uv         = uv;
        mesh.triangles  = triangles;

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
                LayoutRebuilder.ForceRebuildLayoutImmediate(movesContainer.GetComponent<RectTransform>());
                move.Find("Stats").gameObject.SetActive(true);
                StartCoroutine(move.Find("Stats").gameObject.FadeOpacity(1f, 0.3f));
                move.Find("Description").gameObject.SetActive(true);
                StartCoroutine(move.Find("Description").gameObject.FadeOpacity(1f, 0.3f));
            }
            else
            {
                positioner.Find("Stats").gameObject.SetActive(false);
                positioner.Find("Description").gameObject.SetActive(false);
                LayoutRebuilder.ForceRebuildLayoutImmediate(movesContainer.GetComponent<RectTransform>());
                move.Find("Stats").gameObject.SetActive(false);
                move.Find("Stats").GetComponent<CanvasGroup>().alpha = 0;
                move.Find("Description").gameObject.SetActive(false);
                move.Find("Description").GetComponent<CanvasGroup>().alpha = 0;
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(movesContainer.GetComponent<RectTransform>());

        thisMove.GetComponent<Animator>().SetBool("isActive", true);
        Transform previousMove = movePanels[ExtensionMethods.IncrementInt(currentMove, 0, movePanels.Length, increment)];

        previousMove.GetComponent<Animator>().SetBool("isActive", false);

        yield return new WaitForSecondsRealtime(0.15f);

        if (!PauseManager.instance.inPartyMenu)
        {
            indicator.SetActive(true);
        }
    }

    private void GetInput()
    {
        if (!PauseManager.instance.inPartyMenu && isActive)
        {
            totalMoves = movePanels.Length;
            bool hasInput;
            (selectedMove, hasInput) = InputManager.GetInput("Vertical", InputManager.Axis.Vertical, totalMoves, selectedMove);
            if ((int)Input.GetAxisRaw("Vertical") != lastInput)
            {
                lastInput = (int)Input.GetAxisRaw("Vertical");
            }
            if (hasInput)
            {
                GameManager.instance.transform.GetComponentInChildren<InputManager>().OnUserInput += PartyManager_OnUserInput;
            }
            else
            {
                //GameManager.instance.transform.GetComponentInChildren<InputManager>().OnUserInput -= PartyManager_OnUserInput;
            }
        }
    }

    private void PartyManager_OnUserInput(object sender, EventArgs e)
    {
        StartCoroutine(AnimateMove(selectedMove, lastInput));
        GameManager.instance.transform.GetComponentInChildren<InputManager>().OnUserInput -= PartyManager_OnUserInput;
    }
}
