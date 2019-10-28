using System;
using System.Collections;
using System.Collections.Generic;
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

    [HideInInspector] public GameObject partyContainer, indicator;
    private GameObject informationContainer, movesContainer;
    private Transform[] movePanels;
    private Pokemon currentPokemon;
    private Move currentMove;

    private int selectedMove, totalMoves;

    [HideInInspector] public bool isActive;
    private bool isInteracting = false, isDrawing = false;

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

        movePanels = movesContainer.transform.GetChildren();
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
        if (PauseManager.instance.inPartyMenu)
        {
            indicator.SetActive(false);
        }

        CheckForInput();
    }

    #endregion

    private void DrawParty(Pokemon pokemon)
    {
        //if (!isDrawing)
        //{
        DrawInformation(pokemon);
        for (int i = 0; i < movePanels.Length; i++)
        {
            DrawMove(i, currentPokemon.learnedMoves[i]);
        }
        DrawSprite(currentPokemon);
        //    isDrawing = true;
        //}
    }

    private void DrawInformation(Pokemon pokemon)
    {
        Transform generalInfo = informationContainer.transform.Find("General Information");
        Transform progress = informationContainer.transform.Find("Progress");
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
        Transform[] children = partyContainer.transform.GetChildren();

        foreach (Transform child in children)
        {
            if (child != null)
            {
                StartCoroutine(child.gameObject.FadeOpacity(opacity, 0.1f));
            }
        }

        StartCoroutine(PauseManager.instance.pauseContainer.transform.Find("Target Sprite").gameObject.FadeOpacity(opacity, 0.1f));
    }

    private void SelectMove()
    {
        indicator.SetActive(false);

        foreach (Transform move in movePanels)
        {
            if (Array.IndexOf(movePanels, move) == selectedMove)
            {
                move.Find("Stats").gameObject.SetActive(true);
                move.Find("Description").gameObject.SetActive(true);
            }
            else
            {
                move.Find("Stats").gameObject.SetActive(false);
                move.Find("Description").gameObject.SetActive(false);
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(movesContainer.GetComponent<RectTransform>());

        if (!PauseManager.instance.inPartyMenu)
        {
            indicator.SetActive(true);
        }
    }

    private void CheckForInput()
    {
        totalMoves = 3;

        if (!PauseManager.instance.inPartyMenu)
        {
            if (Input.GetAxisRaw("Vertical") != 0)
            {
                if (!isInteracting)
                {
                    if (Input.GetAxisRaw("Vertical") < 0)
                    {
                        if (selectedMove < totalMoves)
                            selectedMove++;
                        else
                            selectedMove = 0;

                        SelectMove();
                    }
                    else if (Input.GetAxisRaw("Vertical") > 0)
                    {
                        if (selectedMove > 0)
                            selectedMove--;
                        else
                            selectedMove = totalMoves;

                        SelectMove();
                    }
                    isInteracting = true;
                }
            }
            else
                isInteracting = false;
        }
    }
}
