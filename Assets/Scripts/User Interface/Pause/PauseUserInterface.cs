using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class PauseUserInterface : UserInterface
{
    #region Constants

    public override int MaxObjects => menus.Count;

    #endregion

    #region Variables

    private List<PauseUserInterfaceBase> menus;

    private TextMeshProUGUI currentMenuText;
    private TextMeshProUGUI previousMenuText;
    private TextMeshProUGUI nextMenuText;

    #endregion

    #region Miscellaneous Methods

    public override void UpdateSelectedObject(int selectedValue, int increment)
    {
        int previousValue = ExtensionMethods.IncrementInt(selectedValue, 0, MaxObjects, -increment);
        int nextValue = ExtensionMethods.IncrementInt(selectedValue, 0, MaxObjects, increment);

        AnimateMenus(selectedValue, previousValue, -increment);
        UpdateNavigationText(selectedValue, previousValue, nextValue);

        menus[selectedValue].SetActive(true);
        menus[previousValue].SetActive(false);
    }

    public void ActivateMenu(int selectedValue, bool isActive)
    {
        UpdateSelectedObject(selectedValue, isActive ? 1 : 0);
        StartCoroutine(AnimatePanel(isActive));
    }

    public UserInterfaceController GetActiveMenu(int selectedValue)
    {
        return menus[selectedValue].Controller;
    }

    private void AnimateMenus(int selectedValue, int previousValue, int increment)
    {
        StartCoroutine(AnimateMenu(selectedValue, increment, true));
        StartCoroutine(AnimateMenu(previousValue, increment, false));
    }

    /*
    private IEnumerator AnimateMenus(int selectedMenu, int previousMenu, int increment)
    {
        menus[selectedMenu].gameObject.SetActive(true); yield return null;

        menus[selectedMenu].GetComponent<Animator>().SetFloat(blend, increment);
        menus[previousMenu].GetComponent<Animator>().SetFloat(blend, increment);

        menus[selectedMenu].GetComponent<Animator>().SetBool("isSelected", true);
        StartCoroutine(menus[selectedMenu].gameObject.FadeOpacity(1f, 0.1f));

        menus[previousMenu].GetComponent<Animator>().SetBool("isSelected", false);
        StartCoroutine(menus[previousMenu].gameObject.FadeOpacity(0f, 0.1f));
        yield return new WaitForSecondsRealtime(0.2f);
        menus[previousMenu].gameObject.SetActive(false);
    }
    */

    private IEnumerator AnimateMenu(int selectedValue, int increment, bool isActive)
    {
        if (isActive)
        {
            //menus[selectedValue].gameObject.SetActive(true);
        }

        menus[selectedValue].GetComponent<Animator>().SetFloat("Blend", increment);
        menus[selectedValue].GetComponent<Animator>().SetBool("isSelected", isActive);

        yield return null;

        StartCoroutine(menus[selectedValue].gameObject.FadeOpacity(isActive ? 1f : 0f, menus[selectedValue].GetComponent<Animator>().GetAnimationTime() / 2));

        if (!isActive)
        {
            yield return new WaitForSecondsRealtime(menus[selectedValue].GetComponent<Animator>().GetAnimationTime());

            //menus[selectedValue].gameObject.SetActive(false);
        }
    }

    private IEnumerator AnimatePanel(bool isActive, float animationDuration = 0.15f)
    {
        if (isActive)
        {
            //gameObject.SetActive(true);
        }

        StartCoroutine(gameObject.FadeOpacity(isActive ? 1f : 0f, animationDuration));

        if (!isActive)
        {
            yield return new WaitForSecondsRealtime(animationDuration);

            //gameObject.SetActive(false);
        }
    }

    private void UpdateNavigationText(int selectedValue, int previousValue, int nextValue)
    {
        currentMenuText.SetText(menus[selectedValue].name);
        previousMenuText.SetText(menus[previousValue].name);
        nextMenuText.SetText(menus[nextValue].name);
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        menus = GetComponentsInChildren<PauseUserInterfaceBase>().ToList();

        currentMenuText = transform.Find("Top Panel/Navigation/Middle/Current/Text").GetComponent<TextMeshProUGUI>();
        previousMenuText = transform.Find("Top Panel/Navigation/Left/Previous/Text").GetComponent<TextMeshProUGUI>();
        nextMenuText = transform.Find("Top Panel/Navigation/Right/Next/Text").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        foreach (PauseUserInterfaceBase menu in menus)
        {
            //menu.gameObject.SetActive(false);
        }
    }

    #endregion

    /*
    #region Variables

    public GameObject pauseContainer { get; private set; }
    private GameObject menuNavigation, characterSprite, sidePanel, indicator;
    private Animator indicatorAnimator, pauseAnimator, spriteAnimator;

    private RectTransform topPanel;

    private Transform[] menus;
    private Transform[] partySlots;

    private int blend;

    #endregion

    #region Miscellaneous Methods

    private void ActivateMenus(int selectedValue, bool state)
    {
        // Debug
        switch (selectedValue)
        {
            default: { break; }
            case (0):
                {
                    float opacity = state ? 0f : 1f;

                    StartCoroutine(FindObjectOfType<MissionsController>().SetActive(state, true));
                    StartCoroutine(sidePanel.FadeOpacity(opacity, 0.2f)); // TODO: Debug
                    break;
                }
            case (1):
                {
                    float opacity = state ? 1f : 0f;

                    StartCoroutine(PartyController.Instance.SetActive(state));
                    StartCoroutine(sidePanel.FadeOpacity(opacity, 0.2f)); // TODO: Debug
                    break;
                }
            case (2):
                {
                    float opacity = state ? 1f : 0f;
                    StartCoroutine(InventoryController.Instance.SetActive(state));
                    StartCoroutine(sidePanel.FadeOpacity(opacity, 0.2f)); // TODO: Debug
                    break;
                }
            case (3):
                {
                    float opacity = state ? 0f : 1f;

                    StartCoroutine(FindObjectOfType<SystemUserInterfaceController>().SetActive(state));
                    StartCoroutine(sidePanel.FadeOpacity(opacity, 0.2f)); // TODO: Debug
                    break;
                }
        }
    }

    public void FadeCharacterSprite(float opacity, float duration)
    {
        bool isSpriteActive = opacity == 1f ? true : false;
        characterSprite.GetComponent<Animator>().enabled = isSpriteActive;
        StartCoroutine(characterSprite.FadeOpacity(opacity, duration));
    }

    public void FadeSidePanel(float opacity, float duration)
    {
        StartCoroutine(sidePanel.FadeOpacity(opacity, duration));
    }

    public void TogglePauseMenu(bool state)
    {
        pauseContainer.SetActive(state);
        if (state)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(topPanel.GetComponent<RectTransform>());
        }
    }

    // TODO: Move to PartyManager?
    public void PopulateSideBar(Party party)
    {
        /*
        foreach (Pokemon pokemon in party.playerParty)
        {
            partySlots[party.playerParty.IndexOf(pokemon)].GetComponent<SidebarSlot>().PopulateSlot(pokemon);
        }
    }

    private void SetMenuText(int selectedMenu, int increment, bool animate)
    {
        TextMeshProUGUI currentText = menuNavigation.transform.Find("Middle/Current").GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI previousText = menuNavigation.transform.Find("Left/Previous").GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI nextText = menuNavigation.transform.Find("Right/Next").GetComponentInChildren<TextMeshProUGUI>();

        int previousMenu = ExtensionMethods.IncrementInt(selectedMenu, 0, PauseUserInterfaceController.instance.menuNames.Length, increment);
        int nextMenu = ExtensionMethods.IncrementInt(selectedMenu, 0, PauseUserInterfaceController.instance.menuNames.Length, -increment);

        currentText.SetText(PauseUserInterfaceController.instance.menuNames[selectedMenu]);
        previousText.SetText(PauseUserInterfaceController.instance.menuNames[previousMenu]);
        nextText.SetText(PauseUserInterfaceController.instance.menuNames[nextMenu]);
    }

    private void UpdateNavigationProgress(int selectedMenu, int increment, float animationDuration)
    {
        Transform[] progress = menuNavigation.transform.Find("Middle/Progress").GetChildren();

        int previousMenu = ExtensionMethods.IncrementInt(selectedMenu, 0, PauseUserInterfaceController.instance.menuNames.Length, increment);

        StartCoroutine(progress[selectedMenu].gameObject.FadeColor(GameManager.GetAccentColor(), animationDuration));
        StartCoroutine(progress[previousMenu].gameObject.FadeColor("#696969".ToColor(), animationDuration));
    }

    private IEnumerator UpdateIndicator(int selectedSlot, int increment, float animationDuration)
    {
        indicatorAnimator.enabled = false;
        StartCoroutine(indicator.FadeOpacity(0f, animationDuration));

        if (increment != 0)
        {
            yield return new WaitForSecondsRealtime(animationDuration);

            indicator.transform.position = partySlots[selectedSlot].position;

            if (selectedSlot >= PartyManager.instance.party.playerParty.Count)
            {
                indicator.transform.Find("Party Indicator").gameObject.SetActive(false);
                indicator.transform.position = sidePanel.transform.Find("Edit").position;
                indicator.transform.Find("Edit Indicator").gameObject.SetActive(true);

                int previousSlot = ExtensionMethods.IncrementInt(selectedSlot, 0, (PartyManager.instance.party.playerParty.Count + 1), increment);
                AnimatePartySlot(previousSlot, 0);
            }
            else
            {
                indicator.transform.Find("Edit Indicator").gameObject.SetActive(false);
                indicator.transform.position = new Vector2(indicator.transform.position.x, partySlots[selectedSlot].position.y);
                indicator.transform.Find("Party Indicator").gameObject.SetActive(true);
            }

            yield return null;
            indicatorAnimator.enabled = true;
        }
    }

    public void UpdateMenus(int selectedMenu, int increment, float animationDuration, bool animate = true)
    {
        int previousMenu = ExtensionMethods.IncrementInt(selectedMenu, 0, PauseUserInterfaceController.instance.menuNames.Length, increment);

        UpdateNavigationProgress(selectedMenu, increment, animationDuration);
        SetMenuText(selectedMenu, increment, animate);
        if (animate)
        {
            StartCoroutine(AnimateMenus(selectedMenu, previousMenu, increment));
            AnimateCharacterSprite(selectedMenu, previousMenu, increment);
        }

        ActivateMenus(selectedMenu, true);
        ActivateMenus(previousMenu, false);
    }

    public void UpdateSidePanel(int selectedSlot, int increment, float animationDuration)
    {
        AnimatePartySlot(selectedSlot, increment);
        StartCoroutine(UpdateIndicator(selectedSlot, increment, animationDuration));
    }

    private IEnumerator AnimateMenus(int selectedMenu, int previousMenu, int increment)
    {
        menus[selectedMenu].gameObject.SetActive(true); yield return null;

        menus[selectedMenu].GetComponent<Animator>().SetFloat(blend, increment);
        menus[previousMenu].GetComponent<Animator>().SetFloat(blend, increment);

        menus[selectedMenu].GetComponent<Animator>().SetBool("isSelected", true);
        StartCoroutine(menus[selectedMenu].gameObject.FadeOpacity(1f, 0.1f));

        menus[previousMenu].GetComponent<Animator>().SetBool("isSelected", false);
        StartCoroutine(menus[previousMenu].gameObject.FadeOpacity(0f, 0.1f));
        yield return new WaitForSecondsRealtime(0.2f);
        menus[previousMenu].gameObject.SetActive(false);
    }

    private void AnimateCharacterSprite(int selectedMenu, int previousMenu, int increment)
    {
        string activeMenu = $"isIn{PauseUserInterfaceController.instance.menuNames[selectedMenu]}"; ;
        string inactiveMenu = $"isIn{PauseUserInterfaceController.instance.menuNames[previousMenu]}";

        characterSprite.GetComponent<CharacterSpriteController>().SetAnimation(activeMenu, inactiveMenu);
    }

    private void AnimatePartySlot(int selectedSlot, int increment)
    {
        if (selectedSlot < PartyManager.instance.party.playerParty.Count)
        {
            int previousSlot = ExtensionMethods.IncrementInt(selectedSlot, 0, PartyManager.instance.party.playerParty.Count, increment);

            partySlots[selectedSlot].GetComponent<Animator>().SetBool("isSelected", true);
            partySlots[previousSlot].GetComponent<Animator>().SetBool("isSelected", false);
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        pauseContainer = transform.gameObject;
        menuNavigation = transform.Find("Top Panel").Find("Navigation").gameObject;
        characterSprite = transform.Find("Character Sprite").gameObject;
        sidePanel = transform.Find("Side Panel").gameObject;
        indicator = sidePanel.transform.Find("Indicators").gameObject;

        topPanel = transform.Find("Top Panel").GetComponent<RectTransform>();

        indicatorAnimator = indicator.GetComponent<Animator>();
        pauseAnimator = pauseContainer.GetComponent<Animator>();
        spriteAnimator = characterSprite.GetComponent<Animator>();

        menus = transform.Find("Menus").transform.GetChildren();
        partySlots = sidePanel.transform.Find("Party").transform.GetChildren();

        blend = Animator.StringToHash("Blend");

        pauseContainer.SetActive(false);
    }

    #endregion
    */
}
