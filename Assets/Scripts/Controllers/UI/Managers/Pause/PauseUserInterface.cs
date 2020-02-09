using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class PauseUserInterface : MonoBehaviour
{
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

                    FindObjectOfType<MissionManager>().flags.isActive = state;
                    StartCoroutine(sidePanel.FadeOpacity(opacity, 0.2f)); // TODO: Debug
                    break;
                }
            case (1):
                {
                    float opacity = state ? 1f : 0f;

                    FindObjectOfType<PartyManager>().flags.isActive = state;
                    StartCoroutine(sidePanel.FadeOpacity(opacity, 0.2f)); // TODO: Debug
                    break;
                }
            case (2):
                {
                    float opacity = state ? 1f : 0f;

                    FindObjectOfType<InventoryManager>().flags.isActive = state;
                    StartCoroutine(sidePanel.FadeOpacity(opacity, 0.2f)); // TODO: Debug
                    break;
                }
            case (3):
                {
                    float opacity = state ? 0f : 1f;

                    FindObjectOfType<SystemManager>().flags.isActive = state;
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
        foreach (Pokemon pokemon in party.playerParty)
        {
            partySlots[party.playerParty.IndexOf(pokemon)].GetComponent<PartySlot>().PopulateSlot(pokemon);
        }
    }

    private void SetMenuText(int selectedMenu, int increment, bool animate)
    {
        TextMeshProUGUI currentText = menuNavigation.transform.Find("Middle/Current").GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI previousText = menuNavigation.transform.Find("Left/Previous").GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI nextText = menuNavigation.transform.Find("Right/Next").GetComponentInChildren<TextMeshProUGUI>();

        int previousMenu = ExtensionMethods.IncrementInt(selectedMenu, 0, PauseManager.instance.menuNames.Length, increment);
        int nextMenu = ExtensionMethods.IncrementInt(selectedMenu, 0, PauseManager.instance.menuNames.Length, -increment);

        currentText.SetText(PauseManager.instance.menuNames[selectedMenu]);
        previousText.SetText(PauseManager.instance.menuNames[previousMenu]);
        nextText.SetText(PauseManager.instance.menuNames[nextMenu]);
    }

    private void UpdateNavigationProgress(int selectedMenu, int increment, float animationDuration)
    {
        Transform[] progress = menuNavigation.transform.Find("Middle/Progress").GetChildren();

        int previousMenu = ExtensionMethods.IncrementInt(selectedMenu, 0, PauseManager.instance.menuNames.Length, increment);

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
        int previousMenu = ExtensionMethods.IncrementInt(selectedMenu, 0, PauseManager.instance.menuNames.Length, increment);

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
        string activeMenu = $"isIn{PauseManager.instance.menuNames[selectedMenu]}"; ;
        string inactiveMenu = $"isIn{PauseManager.instance.menuNames[previousMenu]}";

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

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        
    }

    #endregion
}
