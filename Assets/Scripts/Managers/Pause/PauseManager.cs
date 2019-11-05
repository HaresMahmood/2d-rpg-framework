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
    public int menuIndex, maxMenuIndex, selectedMenu = 0, currentMenuIndex = 0;

    public event EventHandler OnUserInput = delegate { };

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
        _UpdateMenu();
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

        OnPause();

        GetInput();

        if (inPartyMenu)
        {
            if (!PartyManager.instance.isActive)
            {
                for (int i = 0; i < PartyManager.instance.party.playerParty.Count; i++)
                {
                    if (i != selectedSlot)
                    {
                        StartCoroutine(AnimateSlots(i, false));
                    }
                    else
                    {
                        party[selectedSlot].Find("Information").gameObject.SetActive(true);
                        StartCoroutine(AnimateSlots(i, true));
                    }
                }
            }
            else
            {
                if (selectedSlot > (PartyManager.instance.party.playerParty.Count - 1))
                {
                    indicator.transform.Find("Party Indicator").gameObject.SetActive(false);
                    indicator.transform.Find("Round Indicator").gameObject.SetActive(false);
                    indicator.transform.position = sidePanel.transform.Find("Edit").position;
                    indicator.transform.Find("Edit Indicator").gameObject.SetActive(true);
                }
                else
                {
                    indicator.transform.Find("Edit Indicator").gameObject.SetActive(false);
                    indicator.transform.Find("Party Indicator").gameObject.SetActive(false);
                    indicator.transform.position = new Vector2(indicator.transform.position.x, party[selectedSlot].position.y);
                    indicator.transform.Find("Round Indicator").gameObject.SetActive(true);
                }
                indicator.SetActive(true);
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
                    indicator.transform.Find("Round Indicator").gameObject.SetActive(false);
                    indicator.transform.position = sidePanel.transform.Find("Edit").position;
                    indicator.transform.Find("Edit Indicator").gameObject.SetActive(true);
                }
                else
                {
                    indicator.transform.Find("Edit Indicator").gameObject.SetActive(false);
                    indicator.transform.Find("Round Indicator").gameObject.SetActive(false);
                    indicator.transform.position = new Vector2(indicator.transform.position.x, party[selectedSlot].position.y);
                    indicator.transform.Find("Party Indicator").gameObject.SetActive(true);
                }

                yield return new WaitForSecondsRealtime(0.15f);
                indicator.SetActive(true);
            }
        }
        isAnimating = false;
    }

    public void OnPause()
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

    public void GetInput()
    {
        totalSlots = PartyManager.instance.party.playerParty.Count + 1;

        if (Input.GetAxisRaw("Face Trigger") == 0)
        {
            if (inPartyMenu)
            {
                bool hasVertInput;
                (selectedSlot, hasVertInput) = InputManager.GetInput("Vertical", InputManager.Axis.Vertical, totalSlots, selectedSlot);
                if (hasVertInput)
                {
                    //GameManager.instance.transform.GetComponentInChildren<InputManager>().OnUserInput += PauseManager_OnUserInput;
                    isAnimating = true;
                }
                else
                {
                    //GameManager.instance.transform.GetComponentInChildren<InputManager>().OnUserInput -= PauseManager_OnUserInput;
                }

                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    if (InventoryManager.instance.isActive)
                    {
                        InventoryManager.instance.Fade(1f);
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
            }
            else if (!inPartyMenu)
            {
                if (Input.GetAxisRaw("Horizontal") != 0)
                {
                    isAnimating = true;
                    if (!isInteracting)
                    {
                        if (Input.GetAxisRaw("Horizontal") < 0)
                        {
                            if (InventoryManager.instance.isActive)
                            {
                                if (InventoryManager.instance.selectedSlot == 0)
                                {
                                    InventoryManager.instance.Fade(0.5f);
                                    inPartyMenu = true;
                                }
                            }
                            else if (PartyManager.instance.isActive)
                            {
                                PartyManager.instance.Fade(0.8f);
                                inPartyMenu = true;
                            }
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
        else
        {
            TriggerInput();
            OnUserInput += PauseManager_OnUserInput;
        }
    }

    private void TriggerInput()
    {
        if (Input.GetAxisRaw("Face Trigger") != 0)
        {
            
            if (!isInteracting)
            {

                currentMenuIndex = ExtensionMethods.IncrementCircularInt(currentMenuIndex, menus.Length, (int)Input.GetAxisRaw("Face Trigger"));
                currentMenu = menus[currentMenuIndex];
                isInteracting = true;
            }
        }
        else
        {
            isInteracting = false;
            
        }

        //InputManager.GetInput("Face Trigger", InputManager.Axis.Horizontal, menus.Length, currentMenuIndex);
        OnUserInput?.Invoke(this, EventArgs.Empty);
    }

    private void PauseManager_OnUserInput(object sender, EventArgs e)
    {
        #if DEBUG
                if (GameManager.Debug())
                {
                    Debug.Log("[PARTY MANAGER:] Event function called (OnUserInput).");
                }
        #endif

        if (Input.GetAxisRaw("Face Trigger") != 0)
        {
            currentMenu = menus[currentMenuIndex];
            StartCoroutine(AnimateText());
            if (inPartyMenu)
            {
                if (InventoryManager.instance.isActive)
                {
                    InventoryManager.instance.Fade(1f);
                    if (InventoryManager.instance.isGivingItem)
                    {
                        InventoryManager.instance.isGivingItem = false;
                    }
                }
                else if (PartyManager.instance.isActive)
                {
                    //StartCoroutine(PartyManager.instance.AnimateMove(PartyManager.instance.selectedMove));
                    PartyManager.instance.Fade(1f);

                }
                inPartyMenu = false;
                //selectedSlot = 0;
            }

            //if (InventoryManager.instance.isActive || PartyManager.instance.isActive)
            for (int i = 0; i < PartyManager.instance.party.playerParty.Count; i++)
            {
                Transform slot = party[i];
                GameObject heldItem = slot.Find("Held Item").gameObject;
                if (InventoryManager.instance.isActive)
                {
                    StartCoroutine(heldItem.FadeOpacity(0f, 0.15f));
                }
                else if (PartyManager.instance.isActive)
                {
                    StartCoroutine(heldItem.FadeOpacity(1f, 0.15f));
                }
            }
        }
    }

    private IEnumerator AnimateText()
    {
        Animator textAnimator = topPanel.transform.Find("Navigation/Current").GetComponentInChildren<Animator>();
        textAnimator.SetTrigger("isActive");
        yield return new WaitForSecondsRealtime(0.08f);
        textAnimator.Rebind();
    }

    public void _UpdateMenu()
    {
        Transform[] progressMarkers = topPanel.transform.Find("Navigation/Progress").GetChildren();
        foreach (Transform marker in progressMarkers)
        {
            if (Array.IndexOf(progressMarkers, marker) == currentMenuIndex)
            {
                marker.GetComponent<Image>().color = GameManager.GetAccentColor();
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

        topPanel.transform.Find("Navigation/Current").GetComponentInChildren<TextMeshProUGUI>().SetText(currentMenu);
        topPanel.transform.Find("Navigation/Previous").GetComponentInChildren<TextMeshProUGUI>().SetText(menus[previousMenu]);
        topPanel.transform.Find("Navigation/Next").GetComponentInChildren<TextMeshProUGUI>().SetText(menus[nextMenu]);

        
        if (currentMenu == menus[0])
        {
            //Debug.Log("PAUSE MANAGER: Mission screen active.");
            //InventoryManager.instance.inventoryContainer.SetActive(false);
            InventoryManager.instance.isActive = false;
            SystemManager.instance.isActive = false;
        }
        else if (currentMenu == menus[1])
        {
            //Debug.Log("PAUSE MANAGER: Pause active.");
            //InventoryManager.instance.inventoryContainer.SetActive(false);

            if (InventoryManager.instance.isActive)
            {
                spriteAnimator.SetBool("isInInventory", false);
                spriteAnimator.SetBool("isInParty", true);

                pauseAnimator.SetBool("isInInventory", false);
                pauseAnimator.SetBool("isInParty", true);
                InventoryManager.instance.isActive = false;
            }
            else
            {
                InventoryManager.instance.isActive = false;
                SystemManager.instance.isActive = false;
            }

            PartyManager.instance.isActive = true;
        }
        else if (currentMenu == menus[2])
        {
            //InventoryManager.instance.inventoryContainer.SetActive(true);
            
            if (PartyManager.instance.isActive)
            {
                spriteAnimator.SetBool("isInInventory", true);
                spriteAnimator.SetBool("isInParty", false);

                pauseAnimator.SetBool("isInInventory", true);
                pauseAnimator.SetBool("isInParty", false);
                PartyManager.instance.isActive = false;
            }
            else if (SystemManager.instance.isActive)
            {
                spriteAnimator.SetBool("isInInventory", true);
                spriteAnimator.SetBool("isInSystem", false);

                pauseAnimator.SetBool("isInInventory", true);
                pauseAnimator.SetBool("isInSystem", false);

                StartCoroutine(sidePanel.FadeOpacity(1f, 0.15f));
                SystemManager.instance.isActive = false;
            }
            else
            {
                PartyManager.instance.isActive = false;
                SystemManager.instance.isActive = false;
            }

            InventoryManager.instance.isActive = true;
        }
        else if (currentMenu == menus[3])
        {
            if (InventoryManager.instance.isActive)
            {
                spriteAnimator.SetBool("isInInventory", false);
                spriteAnimator.SetBool("isInSystem", true);

                pauseAnimator.SetBool("isInInventory", false);
                pauseAnimator.SetBool("isInSystem", true);

                StartCoroutine(sidePanel.FadeOpacity(0f, 0.15f));
                InventoryManager.instance.isActive = false;
            }
            else
            {
                PartyManager.instance.isActive = false;
                InventoryManager.instance.isActive = false;
            }

            SystemManager.instance.isActive = true;
        }
    }

    public void UpdateMenu(string currentMenu)
    {

    }
}
