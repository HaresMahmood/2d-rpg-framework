using System;
using System.Collections;
using TMPro;
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

    [HideInInspector] public int slotIndex, maxSlotIndex, selectedSlot = 0;

    private bool isInteracting = false, isDrawingParty = false, isResetingInventory = false, isAnimating = false;

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
        //StartCoroutine(TogglePause());
        TogglePause();
    }

    #endregion

    public void TogglePause()
    {

        if (Input.GetButtonDown("Start") && !DialogManager.instance.isActive)
        {
            isPaused = !isPaused;
            isResetingInventory = !isResetingInventory;

            if (!isResetingInventory && !isPaused)
            {
                foreach (Item item in InventoryManager.instance.inventory.items)
                {
                    if (item.isNew)
                    {
                        item.isNew = false;
                    }
                }
            }
        }

        InPause();

        if (inPartyMenu)
        {
            for (int i = 0; i < GameManager.instance.party.playerParty.Count; i++)
            {
                if (i != slotIndex)
                {
                    StartCoroutine(AnimateSlots(i, false));
                }
                else
                {
                    StartCoroutine(AnimateSlots(i, true));
                    party[slotIndex].Find("Information").gameObject.SetActive(true);

                }
            }
        }
        else
        {
            isAnimating = true;
            party[slotIndex].GetComponent<Animator>().SetBool("isSelected", false);
            indicator.SetActive(false);
        }
    }

    private IEnumerator AnimateSlots(int slot, bool state)
    {
        if (isAnimating)
        {
            indicator.SetActive(false);

            party[slot].GetComponent<Animator>().SetBool("isSelected", state);


            if (inPartyMenu)
            {
                if (slotIndex > (GameManager.instance.party.playerParty.Count - 1))
                {
                    indicator.transform.Find("Party Indicator").gameObject.SetActive(false);
                    indicator.transform.position = partyContainer.transform.Find("Edit").position;
                    indicator.transform.Find("Edit Indicator").gameObject.SetActive(true);
                }
                else
                {
                    indicator.transform.Find("Edit Indicator").gameObject.SetActive(false);
                    indicator.transform.position = new Vector2(indicator.transform.position.x, party[slotIndex].position.y);
                    indicator.transform.Find("Party Indicator").gameObject.SetActive(true);
                }

                yield return new WaitForSecondsRealtime(0.15f);
                indicator.SetActive(true);
            }
        }
        isAnimating = false;
        yield return new WaitForSecondsRealtime(0.05f);
        CheckForInput();
    }

    public void InPause()
    {
        if (isPaused)
        {
            pauseContainer.SetActive(true);
            CameraController.instance.GetComponent<PostprocessingBlur>().enabled = true;
            Time.timeScale = 0f;

            //if (isDrawingParty)
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
                party[currentSlot].Find("Health Bar").gameObject.SetActive(true);
                party[currentSlot].Find("Information/Name").GetComponent<TextMeshProUGUI>().SetText(pokemon.name);
                party[currentSlot].Find("Information/Level/Level").GetComponent<TextMeshProUGUI>().SetText(pokemon.level.ToString());
            }

            if (GameManager.instance.party.playerParty.Count < 6)
            {
                for (int i = currentSlot++; i < party.Length; i++)
                {
                    party[currentSlot].Find("Pokémon").gameObject.SetActive(false);
                    party[currentSlot].Find("Held Item").gameObject.SetActive(false);
                    party[currentSlot].Find("Health Bar").gameObject.SetActive(false);
                    party[currentSlot].Find("Information").gameObject.SetActive(false);
                }
            }

            //isDrawingParty = false;
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
            isDrawingParty = false;
        }
    }

    private void ResetInventory()
    {
        InventoryManager.instance.categoryAnim.Rebind();
    }

    public void CheckForInput()
    {
        maxSlotIndex = GameManager.instance.party.playerParty.Count;

        if (Input.GetAxisRaw("Vertical") != 0)
        {
            isAnimating = true;
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
            isAnimating = true;
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
        {
            isInteracting = false;
        }
            
    }
}
