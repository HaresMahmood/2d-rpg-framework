﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class MissionUserInterface : MonoBehaviour
{
    #region Variables

    private MissionPanel[] missionsPanel;
    private Transform[] categoryIcons;

    private GameObject leftPanel;
    private GameObject rightPanel;
    private GameObject indicator;

    private Animator indicatorAnimator;

    private Scrollbar scrollbar;

    #endregion

    #region Miscellaneous Methods

    /// <summary>
    /// Animates and updates the position of the indicator. Dynamically changes position and size of indicator depending on what situation it is used for. If no value is selected, the indicator completely fades out.
    /// </summary>
    /// <param name="selectedValue"> Index of the value currently selected. </param>
    /// <param name="duration"> Duration of the animation/fade. </param>
    /// <returns> Co-routine. </returns>
    private IEnumerator UpdateIndicator(int selectedValue, float duration = 0.1f)
    {
        indicatorAnimator.enabled = false;
        StartCoroutine(indicator.FadeOpacity(0f, duration));
        yield return new WaitForSecondsRealtime(duration);

        indicator.transform.position = missionsPanel[selectedValue].transform.position; 
        
        yield return null;
        indicatorAnimator.enabled = true;
    }

    private void UpdateScrollbar(int selectedSlot = -1)
    {
        if (selectedSlot > -1)
        {
            float totalMoves = (float)missionsPanel.Length;
            float targetValue = 1.0f - (float)selectedSlot / (totalMoves - 1);
            StartCoroutine(scrollbar.LerpScrollbar(targetValue, 0.08f));
        }
        else
        {
            scrollbar.value = 1;
        }
    }

    private void UpdatePanels(int selectedSlot)
    {
        rightPanel.GetComponentInChildren<MissionMainPanel>().UpdateInformation(MissionManager.instance.missions.mission[selectedSlot]);
        rightPanel.GetComponentInChildren<MissionOtherPanel>().UpdateInformation(MissionManager.instance.missions.mission[selectedSlot]);
    }

    public void UpdateSelectedSlot(int selectedSlot)
    {
        UpdateScrollbar(selectedSlot);
        StartCoroutine(UpdateIndicator(selectedSlot));
        UpdatePanels(selectedSlot);
    }

    public void UpdateSelectedCategory(int selectedCategory, int increment)
    {
        int previousCategory = ExtensionMethods.IncrementInt(selectedCategory, 0, MissionManager.instance.categoryNames.Count, -increment);

        AnimateCategoryPosition(selectedCategory, previousCategory);
        AnimateCategoryColor(selectedCategory, previousCategory);
        //StartCoroutine(AnimateCategoryIcon(selectedCategory, previousCategory, -increment));

        UpdateSelectedSlot(0);
        UpdateIndicator(0);
        UpdateScrollbar();
    }

    private void AnimateCategoryPosition(int selectedCategory, int previousCategory)
    {
        categoryIcons[selectedCategory].GetComponent<Animator>().SetBool("isSelected", true);
        categoryIcons[previousCategory].GetComponent<Animator>().SetBool("isSelected", false);
    }

    private void AnimateCategoryColor(int selectedCategory, int previousCategory)
    {
        StartCoroutine(categoryIcons[selectedCategory].GetComponentInChildren<Image>().gameObject.FadeColor(GameManager.GetAccentColor(), 0.1f));
        StartCoroutine(categoryIcons[previousCategory].GetComponentInChildren<Image>().gameObject.FadeColor(Color.white, 0.1f));
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        leftPanel = transform.Find("Left").gameObject;
        rightPanel = transform.Find("Right").gameObject;
        indicator = leftPanel.transform.Find("List/Indicator").gameObject;

        indicatorAnimator = indicator.GetComponent<Animator>();

        scrollbar = leftPanel.transform.Find("List/Scrollbar").GetComponent<Scrollbar>();

        missionsPanel = leftPanel.transform.Find("List/Mission List").GetComponentsInChildren<MissionPanel>();
        categoryIcons = leftPanel.transform.Find("Categories/Category Icons").GetChildren();

        UpdatePanels(0);
        for (int i = 0; i < MissionManager.instance.missions.mission.Count; i++)
        {
            missionsPanel[i].UpdateInformation(MissionManager.instance.missions.mission[i]);
        }

        UpdateScrollbar();
        StartCoroutine(UpdateIndicator(0));
        UpdateSelectedCategory(0, -1);
    }

    #endregion
    
}
