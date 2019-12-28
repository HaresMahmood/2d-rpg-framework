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

    public void UpdateNavigation()
    {
        foreach (Transform option in navOptions)
        {
            option.GetComponentInChildren<TextMeshProUGUI>().SetText(SystemManager.instance.navigationNames[Array.IndexOf(navOptions, option)]);
            option.name = SystemManager.instance.navigationNames[Array.IndexOf(navOptions, option)];
        }
    }

    public void AnimateNavigationText(int selectedOption, float fontSize, float duration)
    {
        StartCoroutine(navOptions[selectedOption].GetComponentInChildren<TextMeshProUGUI>().LerpTextSize(fontSize, duration));
    }

    public void AnimateNavigationOption(int selectedOption, int increment)
    {
        int previousOption = ExtensionMethods.IncrementInt(selectedOption, 0, navOptions.Length, increment);

        AnimateNavigationText(selectedOption, 120f, 0.1f);
        StartCoroutine(navOptions[selectedOption].GetComponentInChildren<TextMeshProUGUI>().gameObject.FadeColor(GameManager.GetAccentColor(), 0.1f));
        AnimateNavigationText(previousOption, 110f, 0.1f);
        StartCoroutine(navOptions[previousOption].GetComponentInChildren<TextMeshProUGUI>().gameObject.FadeColor(Color.white, 0.1f));
    }

    public IEnumerator AnimateNavigation(string selectedOption, bool toNavigation)
    {
        if (toNavigation)
        {
            navContainer.transform.Find("Options").GetComponent<Animator>().SetBool("isInHighlevel", true);
            navContainer.transform.Find("Options").GetComponent<Animator>().SetBool(selectedOption, false);
        }
        else
        {
            navContainer.transform.Find("Options").GetComponent<Animator>().SetBool("isInHighlevel", false);
            navContainer.transform.Find("Options").GetComponent<Animator>().SetBool(selectedOption, true);
        }

        // Debug
        yield return null;
    }

    public float GetAnimationTime()
    {
        float waitTime = navContainer.transform.Find("Options").GetComponent<Animator>().GetAnimationTime();
        return waitTime;
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

        StartCoroutine(FindObjectOfType<BottomPanelUserInterface>().ChangePanelButtons(SystemManager.instance.buttons));
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {

    }
}

    #endregion
