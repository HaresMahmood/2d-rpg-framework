using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
///
/// </summary>
public class SystemManager : MonoBehaviour
{
    #region Variables

    public static SystemManager instance;

    [UnityEngine.Header("Setup")]
    public Party party;

    [UnityEngine.Header("Settings")]
    [SerializeField] private Material chartMaterial;

    [HideInInspector] public GameObject systemContainer, navContainer, indicator;

    private Transform[] navOptions;
    private string[] highlevelText = new string[] { "Save", "Settings", "Tutorials", "Controls", "Quit" };
    private string[] settingsText = new string[] { "General", "Battle", "Customization", "Accessability", "Test" };


    [HideInInspector] public int selectedNavOption, totalNavOptions;

    [HideInInspector] public bool isActive = false, isDrawing = false, isInSettings = false;
    private bool isInteracting = false;

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
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
        systemContainer = PauseManager.instance.pauseContainer.transform.Find("System").gameObject;
        navContainer = systemContainer.transform.Find("Navigation").gameObject;

        navOptions = navContainer.transform.Find("Options").GetChildren();

        selectedNavOption = 0;
        AnimateNavigation();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        if (PauseManager.instance.isPaused)
        {
            CheckForInput();
        }
    }

    #endregion

    private void AnimateNavigation()
    {
        foreach (Transform option in navOptions)
        {
            if (Array.IndexOf(navOptions, option) == selectedNavOption)
            {
                option.GetComponent<Animator>().SetBool("isSelected", true);
                StartCoroutine(option.GetComponentInChildren<TextMeshProUGUI>().gameObject.FadeColor(GameManager.AccentColor(), 0.1f));
            }
            else
            {
                option.GetComponent<Animator>().SetBool("isSelected", false);
                StartCoroutine(option.GetComponentInChildren<TextMeshProUGUI>().gameObject.FadeColor(Color.white, 0.1f));
            }
        }
    }

    private IEnumerator AnimateOptions()
    {
        if (!isInSettings)
        {
            navContainer.transform.Find("Options").GetComponent<Animator>().SetBool("isInHighlevel", false);
            navContainer.transform.Find("Options").GetComponent<Animator>().SetBool("isInSettings", true);
            isInSettings = true;
        }
        else
        {
            navContainer.transform.Find("Options").GetComponent<Animator>().SetBool("isInHighlevel", true);
            navContainer.transform.Find("Options").GetComponent<Animator>().SetBool("isInSettings", false);
            isInSettings = false;
        }

        yield return new WaitForSecondsRealtime(navContainer.transform.Find("Options").GetComponent<Animator>().GetAnimationTime() / 2);
        selectedNavOption = 0;
        AnimateNavigation();
        DrawSettings();
    }

    private void DrawSettings()
    {
        string[] newText;
        if (isInSettings)
        {
            newText = settingsText;
        }
        else
        {
            newText = highlevelText;
        }

        foreach (Transform option in navOptions)
        {
            option.GetComponentInChildren<TextMeshProUGUI>().SetText(newText[Array.IndexOf(navOptions, option)]);
        }
    }

    private void CheckForInput()
    {
        totalNavOptions = navOptions.Length - 1;

        if (!PauseManager.instance.inPartyMenu)
        {
            if (Input.GetAxisRaw("Vertical") != 0)
            {
                if (!isInteracting)
                {
                    if (Input.GetAxisRaw("Vertical") < 0)
                    {
                        if (selectedNavOption < totalNavOptions)
                        {
                            selectedNavOption++;
                            AnimateNavigation();
                        }
                        else
                        {
                            selectedNavOption = 0;
                            AnimateNavigation();
                        }
                    }
                    else if (Input.GetAxisRaw("Vertical") > 0)
                    {
                        if (selectedNavOption > 0)
                        {
                            selectedNavOption--;
                            AnimateNavigation();
                        }
                        else
                        {
                            selectedNavOption = totalNavOptions;
                            AnimateNavigation();
                        }
                    }
                    isInteracting = true;
                }
            }
            else if (Input.GetButtonDown("Interact"))
            {
                StartCoroutine(AnimateOptions());
            }
            else
                isInteracting = false;
        }
    }
}
