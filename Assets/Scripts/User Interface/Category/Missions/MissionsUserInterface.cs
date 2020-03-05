using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class MissionsUserInterface : CategoryUserInterface
{
    #region Variables

    public List<Mission> categoryMissions { get; private set; } = new List<Mission>();

    #endregion

    #region Miscellaneous Methods


    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        Transform leftPanel = transform.Find("Left");
        Transform rightPanel = transform.Find("Right");

        selector = leftPanel.Find("List/Selector").gameObject;

        scrollbar = leftPanel.Find("List/Scrollbar").GetComponent<Scrollbar>();

        categorizablePanel = rightPanel.Find("Information Panel").gameObject;

        emptyPanel = rightPanel.Find("Empty Panel").gameObject;

        categorizableSlots = leftPanel.Find("List/Mission List").GetComponentsInChildren<CategorizableSlot>().ToList();

        //informationPanel = rightPanel.Find("Information Panel").GetComponent<MissionsInformationUserInterface>();

        //StartCoroutine(FindObjectOfType<BottomPanelUserInterface>().ChangePanelButtons(InventoryController.instance.buttons));

        base.Awake();

        foreach (MissionSlot slot in categorizableSlots)
        {
            slot.DeactivateSlot();
        }
    }

    #endregion


    /*
    /// <summary>
    /// Animates and updates the position of the indicator. Dynamically changes position and size of indicator depending on what situation it is used for. If no value is selected, the indicator completely fades out.
    /// </summary>
    /// <param name="selectedValue"> Index of the value currently selected. </param>
    /// <param name="duration"> Duration of the animation/fade. </param>
    /// <returns> Co-routine. </returns>
    private IEnumerator UpdateIndicator(int selectedValue = -1, float duration = 0.1f)
    {
        indicatorAnimator.enabled = false;
        StartCoroutine(indicator.FadeOpacity(0f, duration));

        if (selectedValue > -1)
        {
            yield return new WaitForSecondsRealtime(duration);

            indicator.transform.position = missionSlots[selectedValue].transform.position;

            yield return null;
            indicatorAnimator.enabled = true;
        }
    }

    private void UpdateScrollbar(int selectedSlot = -1)
    {
        if (scrollbar.gameObject.activeSelf)
        {
            scrollbar.transform.parent.gameObject.SetActive(true);

            if (selectedSlot > -1)
            {
                float totalSlots = categoryMissions.Count;
                float targetValue = 1.0f - selectedSlot / (totalSlots - 1);
                StartCoroutine(scrollbar.LerpScrollbar(targetValue, 0.08f));
            }
            else
            {
                scrollbar.value = 1;
            }
        }
        else
        {
            scrollbar.transform.parent.gameObject.SetActive(false);
        }
    }

    private void UpdatePanels(int selectedSlot)
    {
        if (categoryMissions.Count >= 0)
        {
            rightPanel.GetComponentInChildren<MissionMainPanel>().UpdateInformation(categoryMissions[selectedSlot]);
            rightPanel.GetComponentInChildren<MissionOtherPanel>().UpdateInformation(categoryMissions[selectedSlot]);
        }
    }

    public void UpdateSelectedSlot(int selectedSlot)
    {
        UpdateScrollbar(selectedSlot);
        StartCoroutine(UpdateIndicator(selectedSlot));
        StartCoroutine(AnimateInformationPanels(selectedSlot));
    }

    /// <summary>
    /// Updates the text and position of the selected category's name.
    /// </summary>
    /// <param name="selectedCategory">  </param>
    /// <param name="animationTime">  </param>
    /// <returns> Co-routine. </returns>
    private IEnumerator UpdateCategoryName(int selectedCategory, float animationTime = 0.1f)
    {
        StartCoroutine(categoryText.gameObject.FadeOpacity(0f, animationTime));

        yield return new WaitForSecondsRealtime(animationTime);

        //categoryText.GetComponentInChildren<TextMeshProUGUI>().SetText($"{MissionManager.instance.categoryNames[selectedCategory]} Missions"); yield return null;
        categoryText.position = new Vector2(categoryIcons[selectedCategory].position.x, categoryText.position.y);
        StartCoroutine(categoryText.gameObject.FadeOpacity(1f, animationTime));
    }

    /// <summary>
    /// Updates the items being displayed in the selected category.
    /// </summary>
    /// <param name="missions"> Inventory of items. </param>
    /// <param name="selectedCategory"> Index of the category currently selected. </param>
    /// <param name="duration"> The duration of the animations. </param>
    /// <param name="delay"> The delay at which certain animations should occur. </param>
    /// <returns> Co-routine. </returns>
    private IEnumerator UpdateCategoryMissions(Missions missions, int selectedCategory, float duration = 0.15f, float delay = 0.03f)
    {
        //categoryMissions = missions.mission.Where(mission => mission.category.ToString().Equals(MissionManager.instance.categoryNames[selectedCategory])).ToList();
        //categoryMissions.Sort((mission1, mission2) => string.Compare(mission1.Name, mission2.Name));
        categoryMissions = categoryMissions.OrderBy(mission => mission.IsCompleted).ToList();

        if (categoryMissions.Count > 0)
        {
            int max = categoryMissions.Count > 7 ? 7 : categoryMissions.Count;

            //StartCoroutine(transform.Find("Middle/Grid/Item Grid").gameObject.FadeOpacity(1f, duration));
            //StartCoroutine(emptyGrid.FadeOpacity(0f, duration));

            indicator.SetActive(false); //StartCoroutine(UpdateIndicator());

            //yield return new WaitForSecondsRealtime(duration);

            for (int i = 0; i < categoryMissions.Count; i++)
            {
                missionSlots[i].gameObject.SetActive(true);
                UpdateScrollbar();
            }

            for (int i = 0; i < max; i++)
            {
                missionSlots[i].UpdateInformation(categoryMissions[i], duration);
                UpdateScrollbar();

                yield return new WaitForSecondsRealtime(delay);
            }

            if (max < categoryMissions.Count)
            {
                for (int i = max; i < categoryMissions.Count; i++)
                {
                    missionSlots[i].UpdateInformation(categoryMissions[i]);
                    UpdateScrollbar();
                }
            }

            indicator.SetActive(true);
            UpdateSelectedSlot(0);
        }
        else
        {
            indicator.SetActive(false); //StartCoroutine(UpdateIndicator());
            StartCoroutine(AnimateInformationPanels());
            //StartCoroutine(emptyGrid.FadeOpacity(1f, duration));
            //StartCoroutine(transform.Find("Middle/Grid/Item Grid").gameObject.FadeOpacity(0f, duration));
        }

        yield return null;  StartCoroutine(UpdateIndicator(0));
    }

    /// <summary>
    /// Resets the opacity of all items in the selected category.
    /// </summary>
    private void ResetCategoryMissions()
    {
        foreach (MissionSlot slot in missionSlots)
        {
            slot.AnimateSlot(0f);
        }
    }

    public void UpdateSelectedCategory(Missions missions, int selectedCategory, int increment)
    {
        int previousCategory = ExtensionMethods.IncrementInt(selectedCategory, 0, MissionManager.instance.categoryNames.Count, -increment);

        AnimateCategoryPosition(selectedCategory, previousCategory);
        AnimateCategoryColor(selectedCategory, previousCategory);
        //StartCoroutine(AnimateCategoryIcon(selectedCategory, previousCategory, -increment));

        StartCoroutine(UpdateCategoryName(selectedCategory));

        ResetCategoryMissions();
        StartCoroutine(UpdateCategoryMissions(missions, selectedCategory));

        UpdateSelectedSlot(0);
        UpdateScrollbar();
    }

    private void AnimateCategoryPosition(int selectedCategory, int previousCategory)
    {
        categoryIcons[selectedCategory].GetComponent<Animator>().SetBool("isSelected", true);
        categoryIcons[previousCategory].GetComponent<Animator>().SetBool("isSelected", false);
    }

    private void AnimateCategoryColor(int selectedCategory, int previousCategory, float animationTime = 0.1f)
    {
        StartCoroutine(categoryIcons[selectedCategory].GetComponentInChildren<Image>().gameObject.FadeColor(GameManager.GetAccentColor(), animationTime));
        StartCoroutine(categoryIcons[previousCategory].GetComponentInChildren<Image>().gameObject.FadeColor(Color.white, animationTime));
    }

    private IEnumerator AnimateInformationPanels(int selectedSlot = -1, float duration = 0.1f, float delay = 0.07f)
    {
        rightPanel.GetComponentInChildren<MissionMainPanel>().FadePanel(0f);
        yield return new WaitForSecondsRealtime(delay);
        rightPanel.GetComponentInChildren<MissionOtherPanel>().FadePanel(0f);

        if (selectedSlot > -1)
        {
            float opacity = categoryMissions[selectedSlot].IsCompleted ? 0.5f : 1f;

            yield return new WaitForSecondsRealtime(duration);

            UpdatePanels(selectedSlot);

            rightPanel.GetComponentInChildren<MissionMainPanel>().FadePanel(opacity);
            yield return new WaitForSecondsRealtime(delay);
            rightPanel.GetComponentInChildren<MissionOtherPanel>().FadePanel(opacity);
        }
    }
    */
}
