using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class SaveUserInterface : SystemUserInterfaceBase
{
    #region Constants

    public override int MaxObjects => buttons.Count;

    #endregion

    #region Variables

    private List<MenuButton> buttons;
    private Animator animator;

    #endregion

    #region Miscellenaous Methods

    public override void SetActive(bool isActive)
    {
        StartCoroutine(SaveUserInterfcaeController.Instance.SetActive(isActive));
    }

    public override void UpdateSelectedObject(int selectedValue, int increment = -1)
    {
        int previousValue = ExtensionMethods.IncrementInt(selectedValue, 0, MaxObjects, -increment);

        buttons[selectedValue].AnimateButton(true);
        buttons[previousValue].AnimateButton(false);

        StartCoroutine(UpdateSelector(buttons[selectedValue].transform));
    }

    public void AcivatePanel(bool isActive)
    {
        animator.SetBool("isActive", isActive);
        UpdateSelectedObject(0, 1);
    }

    #endregion

        #region Unity Methods

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
    protected override void Awake()
    {
        buttons = transform.Find("Confirmation").GetComponentsInChildren<MenuButton>().ToList();
        animator = transform.Find("Confirmation").GetComponent<Animator>();

        selectorI = transform.Find("Confirmation/Selector").gameObject;

        base.Awake();
    }

    #endregion

    /*
    #region Variables

    private GameObject indicator;

    public Transform[] navOptions { get; private set; }
    private Transform saveOption;

    private Slider progressBar;

    #endregion

    #region Miscellaneous Methods

    public IEnumerator InitializeSave()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        foreach (Transform option in navOptions)
        {
            StartCoroutine(option.gameObject.FadeOpacity(1f, 0.25f));
            yield return new WaitForSecondsRealtime(0.4f);
        }
        indicator.gameObject.SetActive(true);
    }

    public void ExitSave()
    {
        progressBar.gameObject.SetActive(false); progressBar.value = 0;
        indicator.gameObject.SetActive(false);
    }

    public IEnumerator AnimateOptions()
    {
        indicator.gameObject.SetActive(false);
        foreach (Transform option in navOptions)
        {
            StartCoroutine(option.gameObject.FadeOpacity(0f, 0.25f));
            yield return new WaitForSecondsRealtime(0.4f);
        }
    }

    public IEnumerator AnimateProgress()
    {
        StartCoroutine(saveOption.gameObject.FadeOpacity(0f, 0.1f));
        saveOption.GetComponentInChildren<TextMeshProUGUI>().SetText("Saving...");
        StartCoroutine(saveOption.gameObject.FadeOpacity(1f, 0.1f));

        progressBar.gameObject.SetActive(true);
        StartCoroutine(progressBar.LerpSlider(1f, 2f));
        yield return new WaitForSecondsRealtime(2.15f);

        /*
        StartCoroutine(saveOption.gameObject.FadeOpacity(0f, 0.1f));
        yield return new WaitForSecondsRealtime(0.1f);
        saveOption.GetComponentInChildren<TextMeshProUGUI>().SetText("Saved!");
        StartCoroutine(saveOption.gameObject.FadeOpacity(1f, 0.1f));
        yield return new WaitForSecondsRealtime(0.1f);

        StartCoroutine(saveOption.gameObject.FadeOpacity(0f, 0.1f));
        saveOption.GetComponentInChildren<TextMeshProUGUI>().SetText("Save");
        StartCoroutine(saveOption.gameObject.FadeOpacity(1f, 0.1f));

        //yield return new WaitForSecondsRealtime(0.2f);

        saveOption.parent.GetComponent<Animator>().SetBool("isInHighlevel", true);
        saveOption.parent.GetComponent<Animator>().SetBool("isInSave", false);

        yield return new WaitForSecondsRealtime(0.2f);

        SaveManager.instance.ExitSave();
    }

    public void UpdateIndicator()
    {
        if (indicator.gameObject.activeSelf)
        {
            indicator.transform.position = navOptions[SaveManager.instance.selectedOption].position;
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        indicator = transform.Find("Indicator").gameObject; indicator.gameObject.SetActive(false);

        progressBar = transform.Find("Progress Bar").GetComponent<Slider>(); progressBar.gameObject.SetActive(false); progressBar.value = 0;

        saveOption = transform.parent.transform.Find("Navigation/Options/Save");
        navOptions = transform.transform.Find("Confirmation").GetChildren();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        // Debug
        UpdateIndicator();
    }

    #endregion
    */
}
