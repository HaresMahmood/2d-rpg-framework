using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
///
/// </summary>
public class PauseUserInterface : MonoBehaviour
{
    #region Variables

    public GameObject pauseContainer { get; private set; }
    private GameObject menuNavigation, targetSprite, sidePanel, indicator;
    private Animator indicatorAnimator, pauseAnimator, spriteAnimator;

    private Transform[] menus;
    private Transform[] partySlots;

    private int blend;

    #endregion

    #region Miscellaneous Methods

    public void TogglePauseMenu(bool state)
    {
        pauseContainer.SetActive(state);
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
        TextMeshProUGUI currentText = menuNavigation.transform.Find("Current").GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI previousText = menuNavigation.transform.Find("Previous").GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI nextText = menuNavigation.transform.Find("Next").GetComponentInChildren<TextMeshProUGUI>();

        int previousMenu = ExtensionMethods.IncrementInt(selectedMenu, 0, PauseManager.instance.menuNames.Length, increment);
        int nextMenu = ExtensionMethods.IncrementInt(selectedMenu, 0, PauseManager.instance.menuNames.Length, -increment);

        currentText.SetText(PauseManager.instance.menuNames[selectedMenu]);
        previousText.SetText(PauseManager.instance.menuNames[previousMenu]);
        nextText.SetText(PauseManager.instance.menuNames[nextMenu]);
    }

    private void UpdateNavigationProgress(int selectedMenu, int increment, float animationDuration)
    {
        Transform[] progress = menuNavigation.transform.Find("Progress").GetChildren();

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
        UpdateNavigationProgress(selectedMenu, increment, animationDuration);
        SetMenuText(selectedMenu, increment, animate);
        if (animate)
        {
            StartCoroutine(AnimateMenus(selectedMenu, increment));
        }
    }

    public void UpdateSidePanel(int selectedSlot, int increment, float animationDuration)
    {
        AnimatePartySlot(selectedSlot, increment);
        StartCoroutine(UpdateIndicator(selectedSlot, increment, animationDuration));
    }

    private IEnumerator AnimateMenus(int selectedMenu, int increment)
    {
        selectedMenu--; // Debug;
        int previousMenu = ExtensionMethods.IncrementInt(selectedMenu, 0, PauseManager.instance.menuNames.Length, increment);

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

    private void AnimateTargetSprite(int selectedMenu, int increment)
    {

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

    #region Unity Methods#

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {

    }

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        pauseContainer = transform.gameObject;
        menuNavigation = transform.Find("Top Panel").Find("Navigation").gameObject;
        targetSprite = transform.Find("Target Sprite").gameObject;
        sidePanel = transform.Find("Side Panel").gameObject;
        indicator = sidePanel.transform.Find("Indicators").gameObject;

        indicatorAnimator = indicator.GetComponent<Animator>();
        pauseAnimator = pauseContainer.GetComponent<Animator>();
        spriteAnimator = pauseContainer.transform.Find("Target Sprite").GetComponent<Animator>();

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
