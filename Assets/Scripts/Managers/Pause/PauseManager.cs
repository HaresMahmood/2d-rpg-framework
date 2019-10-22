using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// </summary>
public class PauseManager : MonoBehaviour
{
    #region Variables

    public static PauseManager instance;

    [UnityEngine.Header("Setup")]
    public GameObject pauseContainer;

    private GameObject partyContainer, indicator;
    private Transform[] party;

    [HideInInspector] public bool isPaused, inPartyMenu = false;

    private int slotIndex, maxSlotIndex, selectedSlot = 0;

    private bool isInteracting = false, isDrawingParty = false;

    #endregion

    #region Unity Methods

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
        pauseContainer.SetActive(false);
        partyContainer = pauseContainer.transform.Find("Side Panel").gameObject;
        indicator = partyContainer.transform.Find("Indicators").gameObject;

        party = partyContainer.transform.Find("Party").transform.GetChildren();

        isPaused = false;
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        TogglePause();
    }

    #endregion

    public void TogglePause()
    {
        if (Input.GetButtonDown("Start") && !DialogManager.instance.isActive)
        {
            isPaused = !isPaused;
        }

        InPause();

        if (inPartyMenu)
        {
            indicator.SetActive(true);

            CheckForInput();

            if (slotIndex > (GameManager.instance.party.playerParty.Count - 1))
            {
                indicator.transform.Find("Party Indicator").gameObject.SetActive(false);
                indicator.transform.Find("Edit Indicator").gameObject.SetActive(true);
                indicator.transform.position = partyContainer.transform.Find("Edit").position;
            }
            else
            {
                indicator.transform.Find("Party Indicator").gameObject.SetActive(true);
                indicator.transform.Find("Edit Indicator").gameObject.SetActive(false);
                indicator.transform.position = party[slotIndex].position;
            }
        }
        else
        {
            indicator.SetActive(false);
        }
    }

    public void InPause()
    {
        if (isPaused)
        {
            pauseContainer.SetActive(true);
            CameraController.instance.GetComponent<PostprocessingBlur>().enabled = true;
            Time.timeScale = 0f;

            //if (!isDrawingParty)
            //{
                int currentSlot = 0;

                for (int i = 0; i < GameManager.instance.party.playerParty.Count; i++)
                {
                    Pokemon pokemon = GameManager.instance.party.playerParty[i];
                    currentSlot = i;

                    party[currentSlot].Find("Pokémon").GetComponent<Image>().sprite = pokemon.menuSprite;
                    party[currentSlot].Find("Pokémon").gameObject.SetActive(true);
                    if (pokemon.heldItem != null)
                    {
                        party[currentSlot].Find("Held Item").GetComponent<Image>().sprite = pokemon.heldItem.sprite;
                        party[currentSlot].Find("Held Item").gameObject.SetActive(true);
                    }
                }

                if (GameManager.instance.party.playerParty.Count < 6)
                {
                    for (int i = currentSlot++; i < party.Length; i++)
                    {
                        party[currentSlot].Find("Pokémon").gameObject.SetActive(false);
                        party[currentSlot].Find("Held Item").gameObject.SetActive(false);
                    }
                }

                //isDrawingParty = true;
            //}

            if (Input.GetButtonDown("Cancel"))
            {
                isPaused = false;
            }
        }
        else
        {
            ResetInventory();

            pauseContainer.SetActive(false);
            CameraController.instance.GetComponent<PostprocessingBlur>().enabled = false;
            Time.timeScale = 1f;
        }
    }

    private void ResetInventory()
    {
        InventoryManager.instance.categoryAnim.Rebind();
    }

    private void CheckForInput()
    {
        maxSlotIndex = GameManager.instance.party.playerParty.Count;

        if (Input.GetAxisRaw("Vertical") != 0)
        {
            if (!isInteracting)
            {
                if (Input.GetAxisRaw("Vertical") < 0)
                {
                    if (slotIndex < maxSlotIndex)
                        slotIndex++;
                    else
                        slotIndex = 0;
                }
                else if (Input.GetAxisRaw("Vertical") > 0)
                {
                    if (slotIndex > 0)
                        slotIndex--;
                    else
                    {
                        slotIndex = maxSlotIndex;
                    }
                }
                isInteracting = true;
            }
        }
        else if (Input.GetAxisRaw("Horizontal") != 0 && !InventoryManager.instance.givingItem)
        {
            if (!isInteracting)
            {
                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    StartCoroutine(InventoryManager.instance.inventoryContainer.FadeObject(1f, 0.1f));
                    inPartyMenu = false;
                }
                else if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    //Debug.Log("Reached end of screen in PAUSEMANAGER");
                }
                isInteracting = true;
            }
        }
        else
            isInteracting = false;
    }

}
