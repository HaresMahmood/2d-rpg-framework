using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class SystemUserInterface : MonoBehaviour
{
    #region Variables

    public event EventHandler OnSettingSelected = delegate { };

    private GameObject navContainer;
    public Transform[] navOptions;

    #endregion

    #region Miscellaneous Methods

    private void UpdateNavigation()
    {
        foreach (Transform option in navOptions)
        {
            option.GetComponentInChildren<TextMeshProUGUI>().SetText(SystemManager.instance.navigationNames[Array.IndexOf(navOptions, option)]);
            option.name = SystemManager.instance.navigationNames[Array.IndexOf(navOptions, option)];
        }
    }

    public void AnimateNavigationOption(int selectedOption, int increment)
    {
        int previousOption = ExtensionMethods.IncrementCircularInt(selectedOption, navOptions.Length, increment);

        navOptions[selectedOption].GetComponent<Animator>().SetBool("isSelected", true);
        StartCoroutine(navOptions[selectedOption].GetComponentInChildren<TextMeshProUGUI>().gameObject.FadeColor(GameManager.GetAccentColor(), 0.1f));
        navOptions[previousOption].GetComponent<Animator>().SetBool("isSelected", false);
        StartCoroutine(navOptions[previousOption].GetComponentInChildren<TextMeshProUGUI>().gameObject.FadeColor(Color.white, 0.1f));
    }

    public IEnumerator AnimateNavigation(string selectedOption, bool toNavigation)
    {
        if (toNavigation)
        {
            navContainer.transform.Find("Options").GetComponent<Animator>().SetBool("isInHighlevel", true);
            navContainer.transform.Find("Options").GetComponent<Animator>().SetBool(selectedOption, false);
            //StartCoroutine(descriptionText.gameObject.FadeOpacity(0f, 0.3f));
        }
        else
        {
            navContainer.transform.Find("Options").GetComponent<Animator>().SetBool("isInHighlevel", false);
            navContainer.transform.Find("Options").GetComponent<Animator>().SetBool(selectedOption, true);
            //StartCoroutine(descriptionText.gameObject.FadeOpacity(1f, 0.3f));
        }

        yield return new WaitForSecondsRealtime(navContainer.transform.Find("Options").GetComponent<Animator>().GetAnimationTime() / 2);
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        navContainer = transform.Find("Navigation").gameObject;
        navOptions = navContainer.transform.Find("Options").GetChildren();

        AnimateNavigationOption(0, -1);
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {

    }
}

    #endregion
