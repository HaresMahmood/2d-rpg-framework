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

    private GameObject sidePanel, topPanel, indicator;
    private Transform[] party;
    private Animator pauseAnimator, spriteAnimator;

    [HideInInspector] public bool isPaused, inPartyMenu = false, isInteracting = false;

    [HideInInspector] public int selectedSlot, totalSlots;

    private bool isDrawingParty = false, isResetingInventory = false, isAnimating = false;
    private string[] menus = new string[] { "Missions", "Party", "Inventory", "System"};
    private string currentMenu;
    private int menuIndex, maxMenuIndex, selectedMenu = 0, currentMenuIndex = 0;

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
        sidePanel = pauseContainer.transform.Find("Side Panel").gameObject;
        topPanel = pauseContainer.transform.Find("Top Panel").gameObject;
        indicator = sidePanel.transform.Find("Indicators").gameObject;

        pauseAnimator = pauseContainer.GetComponent<Animator>();
        spriteAnimator = pauseContainer.transform.Find("Target Sprite").GetComponent<Animator>();

        party = sidePanel.transform.Find("Party").transform.GetChildren();

        currentMenu = menus[2];
        currentMenuIndex = 2;
        maxMenuIndex = menus.Length - 1;
        isPaused = false;
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        TogglePause();

        UpdateMenu();
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

        CheckForInput();

        if (inPartyMenu)
        {
            for (int i = 0; i < PartyManager.instance.party.playerParty.Count; i++)
            {
                if (i != selectedSlot)
                {
                    StartCoroutine(AnimateSlots(i, false));
                }
                else
                {
                    StartCoroutine(AnimateSlots(i, true));
                    party[selectedSlot].Find("Information").gameObject.SetActive(true);
                }
            }
        }
        else
        {
            isAnimating = true;
            party[selectedSlot].GetComponent<Animator>().SetBool("isSelected", false);
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
                if (selectedSlot > (PartyManager.instance.party.playerParty.Count - 1))
                {
                    indicator.transform.Find("Party Indicator").gameObject.SetActive(false);
                    indicator.transform.position = sidePanel.transform.Find("Edit").position;
                    indicator.transform.Find("Edit Indicator").gameObject.SetActive(true);
                }
                else
                {
                    indicator.transform.Find("Edit Indicator").gameObject.SetActive(false);
                    indicator.transform.position = new Vector2(indicator.transform.position.x, party[selectedSlot].position.y);
                    indicator.transform.Find("Party Indicator").gameObject.SetActive(true);
                }

                yield return new WaitForSecondsRealtime(0.15f);
                indicator.SetActive(true);
            }
        }
        isAnimating = false;
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

            for (int i = 0; i < PartyManager.instance.party.playerParty.Count; i++)
            {
                Pokemon pokemon = PartyManager.instance.party.playerParty[i];
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

            if (PartyManager.instance.party.playerParty.Count < 6)
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
        foreach (Transform category in InventoryManager.instance.categoryContainer)
        {
            category.GetComponent<Animator>().Rebind();
        }
    }

    public void CheckForInput()
    {
        totalSlots = PartyManager.instance.party.playerParty.Count;

        if (Input.GetAxisRaw("Vertical") != 0)
        {
            if (inPartyMenu)
            {
                isAnimating = true;
                if (!isInteracting)
                {
                    if (PartyManager.instance.isActive)
                    {
                        if (PartyManager.instance.selectedMove != 0)
                        {
                            PartyManager.instance.selectedMove = 0;
                            PartyManager.instance.SelectMove();
                        }
                    }
                    
                    if (Input.GetAxisRaw("Vertical") < 0)
                    {
                        if (selectedSlot < totalSlots)
                            selectedSlot++;
                        else
                            selectedSlot = 0;
                    }
                    else if (Input.GetAxisRaw("Vertical") > 0)
                    {
                        if (selectedSlot > 0)
                            selectedSlot--;
                        else
                        {
                            selectedSlot = totalSlots;
                        }
                    }
                    isInteracting = true;
                }
            }
        }
        else if (Input.GetAxisRaw("Horizontal") != 0)
        {
            isAnimating = true;
            if (!isInteracting)
            {
                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    if (InventoryManager.instance.isActive)
                    {
                        InventoryManager.instance.FadeInventory(1f);
                        if (InventoryManager.instance.isGivingItem)
                        {
                            InventoryManager.instance.isGivingItem = false;
                        }
                    }
                    else if (PartyManager.instance.isActive)
                    {
                        PartyManager.instance.Fade(1f);
                        PartyManager.instance.indicator.SetActive(true);
                    }
                    inPartyMenu = false;
                    //selectedSlot = 0;
                }
                else if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    if (InventoryManager.instance.isActive)
                    {
                        if (InventoryManager.instance.selectedSlot == -1)
                        {
                            InventoryManager.instance.FadeInventory(0.5f);
                            inPartyMenu = true;
                        }
                    }
                    else if (PartyManager.instance.isActive)
                    {
                        PartyManager.instance.Fade(0.5f);
                        inPartyMenu = true;
                    }
                }
                isInteracting = true;
            }
        }
        else if (Input.GetAxisRaw("Face Trigger") != 0)
        {
            if (!isInteracting)
            {
                if (inPartyMenu)
                {
                    if (InventoryManager.instance.isActive)
                    {
                        InventoryManager.instance.FadeInventory(1f);
                        if (InventoryManager.instance.isGivingItem)
                        {
                            InventoryManager.instance.isGivingItem = false;
                        }
                    }
                    else if (PartyManager.instance.isActive)
                    {
                        PartyManager.instance.Fade(1f);
                    }
                    inPartyMenu = false;
                    //selectedSlot = 0;
                }

                if (Input.GetAxisRaw("Face Trigger") > 0)
                {
                    StartCoroutine(AnimateText());
                    if (currentMenuIndex < (menus.Length - 1))
                    {
                        currentMenu = menus[Array.IndexOf(menus, currentMenu) + 1];
                        currentMenuIndex++;
                    }
                    else
                    {
                        currentMenu = menus[0];
                        currentMenuIndex = 0;
                    }
                }
                else if (Input.GetAxisRaw("Face Trigger") < 0)
                {
                    StartCoroutine(AnimateText());
                    if (currentMenuIndex > 0)
                    {
                        currentMenu = menus[Array.IndexOf(menus, currentMenu) - 1];
                        currentMenuIndex--;
                    }
                    else
                    {
                        currentMenu = menus[menus.Length - 1];
                        currentMenuIndex = menus.Length - 1;
                    }
                }
                isInteracting = true;
            }
        }
        else
        {
            isInteracting = false;
            RebindText();
        }
    }

    private IEnumerator AnimateText()
    {
        Animator textAnimator = topPanel.transform.Find("Navigation/Current").GetComponentInChildren<Animator>();
        textAnimator.SetTrigger("isActive");
        yield return new WaitForSecondsRealtime(textAnimator.GetAnimationTime());
    }

    private void RebindText()
    {
        Animator textAnimator = topPanel.transform.Find("Navigation/Current").GetComponentInChildren<Animator>();
        textAnimator.Rebind();
    }

    public void UpdateMenu()
    {
        Transform[] progressMarkers = topPanel.transform.Find("Navigation/Progress").GetChildren();
        foreach (Transform marker in progressMarkers)
        {
            if (Array.IndexOf(progressMarkers, marker) == currentMenuIndex)
            {
                marker.GetComponent<Image>().color = GameManager.instance.accentColor;
            }
            else
            {
                marker.GetComponent<Image>().color = "696969".ToColor();
            }
        }
        
        int previousMenu = currentMenuIndex - 1;
        int nextMenu = currentMenuIndex + 1;

        if (previousMenu < 0)
        {
            previousMenu = Array.IndexOf(menus, menus[menus.Length - 1]);
        }
        
        if (nextMenu > menus.Length - 1)
        {
            nextMenu = 0;
        }

        //Debug.Log(previousMenu);
        //Debug.Log(nextMenu);

        topPanel.transform.Find("Navigation/Current").GetComponentInChildren<TextMeshProUGUI>().SetText(currentMenu);
        topPanel.transform.Find("Navigation/Previous").GetComponentInChildren<TextMeshProUGUI>().SetText(menus[previousMenu]);
        topPanel.transform.Find("Navigation/Next").GetComponentInChildren<TextMeshProUGUI>().SetText(menus[nextMenu]);

        if (currentMenu == menus[0])
        {
            //Debug.Log("PAUSE MANAGER: Mission screen active.");
            //InventoryManager.instance.inventoryContainer.SetActive(false);
            InventoryManager.instance.isActive = false;
            PartyManager.instance.isActive = false;
        }
        else if (currentMenu == menus[1])
        {
            //Debug.Log("PAUSE MANAGER: Pause active.");
            //InventoryManager.instance.inventoryContainer.SetActive(false);
            InventoryManager.instance.isActive = false;
            PartyManager.instance.isActive = true;

            pauseAnimator.SetBool("isInInventory", false);
            pauseAnimator.SetBool("isInParty", true);

            spriteAnimator.SetBool("isInInventory", false);
            spriteAnimator.SetBool("isInParty", true);
        }
        else if (currentMenu == menus[2])
        {
            //InventoryManager.instance.inventoryContainer.SetActive(true);
            PartyManager.instance.isActive = false;
            InventoryManager.instance.isActive = true;

            spriteAnimator.SetBool("isInInventory", true);
            spriteAnimator.SetBool("isInParty", false);

            pauseAnimator.SetBool("isInInventory", true);
            pauseAnimator.SetBool("isInParty", false);

        }
        else if (currentMenu == menus[3])
        {
            //Debug.Log("PAUSE MANAGER: System screen active.");
            //InventoryManager.instance.inventoryContainer.SetActive(false);
            InventoryManager.instance.isActive = false;
            PartyManager.instance.isActive = false;
        }
    }
}
