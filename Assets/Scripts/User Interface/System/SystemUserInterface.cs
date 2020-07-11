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
public class SystemUserInterface : PauseUserInterfaceBase
{
    #region Constants

    public override int MaxObjects => navigation.Count;

    #endregion

    #region Variables

    private List<Transform> navigation;
    private List<SystemUserInterfaceBase> menus;
    private Animator animator;

    #endregion

    #region Miscellaneous Methods

    public override void SetActive(bool isActive, bool condition = true)
    {
        StartCoroutine(SystemUserInterfaceController.Instance.SetActive(isActive, condition));
    }

    public override void UpdateSelectedObject(int selectedValue, int increment)
    {
        int previousValue = ExtensionMethods.IncrementInt(selectedValue, 0, MaxObjects, increment);

        UpdateNavigationTextColor(selectedValue, previousValue);
    }

    // TODO: Debug
    public void UpdateSelectedCategory(int selectedValue, bool isActive, float animationDuration = 0.1f)
    {
       selectedValue = Mathf.Clamp(selectedValue, 0, 2);

        Debug.Log(menus[selectedValue].name);

        animator.SetBool($"isIn{menus[selectedValue].name}", isActive);
        animator.SetBool("isInHighlevel", !isActive);
        menus[selectedValue].SetActive(isActive);
        StartCoroutine(menus[selectedValue].gameObject.FadeOpacity(isActive ? 1f : 0f, animationDuration));
    }

    public void UpdateNavigationTextColor(int selectedValue, bool isActive, float animationDuration = 0.1f)
    {
        StartCoroutine(navigation[selectedValue].Find("Text").gameObject.FadeColor(isActive ? GameManager.GetAccentColor() : Color.white, animationDuration));
    }

    public IEnumerator SetIdle()
    {
        yield return null;

        yield return new WaitForSecondsRealtime(animator.GetAnimationTime());

        animator.SetBool("isInHighlevel", false);
    }

    private void UpdateNavigationTextColor(int selectedValue, int previousValue, float animationDuration = 0.1f)
    {
        StartCoroutine(navigation[selectedValue].Find("Text").gameObject.FadeColor(GameManager.GetAccentColor(), animationDuration));
        StartCoroutine(navigation[previousValue].Find("Text").gameObject.FadeColor(Color.white, animationDuration));
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        navigation = transform.Find("Navigation").GetChildren().ToList();
        menus = transform.GetComponentsInChildren<SystemUserInterfaceBase>().ToList();

        animator = transform.Find("Navigation").GetComponent<Animator>();
    }

    #endregion

    /*
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
        #endregion
    */
}
