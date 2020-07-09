using System.Collections;
using UnityEngine;

/// <summary>
///
/// </summary>
public class SettingsUserInterfaceController : UserInterfaceController
{
    #region Fields

    private static SettingsUserInterfaceController instance;

    [SerializeField] private SettingsUserInterface userInterface;

    #endregion

    #region Properties

    /// <summary>
    /// Singleton pattern.
    /// </summary>
    public static SettingsUserInterfaceController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SettingsUserInterfaceController>();
            }

            return instance;
        }
    }

    public override UserInterface UserInterface
    {
        get { return userInterface; }
    }

    #endregion

    #region Miscellaneous Methods

    public override IEnumerator SetActive(bool isActive, bool condition = true)
    {
        if (condition)
        {
            UpdateSelectedObject(selectedValue, 0);
            selectedValue = 0;
            UpdateSelectedObject(selectedValue, 1);

            yield return new WaitForSecondsRealtime(0.1f);
        }

        Flags.isActive = isActive;
    }

    protected override void GetInput(string axisName)
    {
        base.GetInput(axisName);

        if (Input.GetButtonDown("Toggle"))
        {
            Debug.Log("Pressed Toggle.");
        }

        if (Input.GetButtonDown("Interact"))
        {
            Flags.isActive = false;
            userInterface.ActivateCategory(selectedValue);
        }

        if (Input.GetButtonDown("Cancel"))
        {
            StartCoroutine(SetActive(false));
            StartCoroutine(GetComponent<SystemUserInterfaceController>().SetActive(true, false));
        }
    }

    #endregion
}

/*
#region Variables

public static SettingsManager instance;

[Header("Setup")]
[SerializeField] private SettingsUserInterface userInterface;
[SerializeField] public List<PanelButton> buttons = new List<PanelButton>();


[Header("Values")]
[ReadOnly] public ViewingMode viewingMode;

private TestInput input = new TestInput();
public Flags flags = new Flags(false, false);

public string[] navigationNames { get; private set; } = new string[] { "General", "Battle", "Customization", "Accessibility", "Controls" };

public int selectedSetting { get; private set; } = 0;
public int selectedNavOption { get; private set; } = 0;

#endregion

#region Structs

public struct Flags
{
    public bool isActive { get; set; }
    public bool isSettingSelected { get; set; }
    /*
    {
        get { return isSettingSelected; }
        set { if (isSettingSelected) isSettingSelected = value; }
    }

    public Flags(bool isActive, bool isSettingSelected)
    {
        this.isActive = isActive;
        this.isSettingSelected = isSettingSelected;
    }
}

#endregion

#region Enums

public enum ViewingMode
{
    Basic,
    Intermediate,
    Advanced
}

#endregion

#region Event Methods

private void SettingsManager_OnUserInput(object sender, EventArgs e)
{
    if (flags.isSettingSelected)
    {

    }
    input.OnUserInput -= SettingsManager_OnUserInput;
}

#endregion

#region Miscellaneous Methods

public IEnumerator OnActive()
{
    userInterface.SelectSetting(0.3f, false);
    userInterface.UpdateNavigationOptions();
    selectedNavOption = 0;
    userInterface.AnimateNavigationOption(selectedNavOption, -1);
    userInterface.UpdateSettingList(selectedNavOption, -1);
    UpdateSetting(0, true);
    AnimateModeText();
    StartCoroutine(FindObjectOfType<BottomPanelUserInterface>().ChangePanelButtons(buttons));

    yield return null;

    flags.isActive = true;
}

public List<ViewingMode> GetPreviousMode(ViewingMode category)
{
    ViewingMode previousCategory = (ViewingMode)ExtensionMethods.IncrementInt((int)category, 0, Enum.GetNames(typeof(ViewingMode)).Length, -1);
    List<ViewingMode> previousCategories = new List<ViewingMode>();
    if (previousCategory < category)
    {
        previousCategories.AddRange(GetPreviousMode(previousCategory));
    }
    previousCategories.Add(category);
    return previousCategories;
}

public IEnumerator DisableSettings()
{
    userInterface.SelectSetting(0f, false);
    yield return new WaitForSecondsRealtime(0.15f);

    flags.isActive = false;

    StartCoroutine(SystemManager.instance.DisableSettings());

    yield return new WaitForSecondsRealtime(0.1f);

    userInterface.UpdateSettingList(selectedNavOption, 0, false);

    if (selectedNavOption != 0)
    {
        userInterface.AnimateNavigationOption(selectedNavOption, 0);
    }
}

private void ToggleSetting(float containerOpcatity, bool setActive)
{
    userInterface.SelectSetting(containerOpcatity, setActive);
    flags.isSettingSelected = setActive;
    selectedSetting = 0;
}

private void UpdateSetting(int increment, bool resetScrollbar = false)
{
    if (!resetScrollbar)
    {
        userInterface.UpdateScrollbar(selectedSetting);
    }
    else
    {
        selectedSetting = 0;
        userInterface.UpdateScrollbar();
    }
    userInterface.UpdateSelectedSetting(selectedSetting, increment);
    StartCoroutine(userInterface.UpdateIndicator(selectedSetting, 0.1f));
}

private void AnimateModeText() // TODO: To user interface.
{
    StartCoroutine(FindObjectOfType<BottomPanelUserInterface>().AnimateValue(viewingMode.ToString(), 1f));
}

private void GetInput()
{
    if (!flags.isSettingSelected)
    {
        bool hasInput;
        (selectedNavOption, hasInput) = input.GetInput("Vertical", TestInput.Axis.Vertical, userInterface.navOptions.Length, selectedNavOption);
        if (hasInput)
        {
            userInterface.AnimateNavigationOption(selectedNavOption, (int)Input.GetAxisRaw("Vertical"));
            userInterface.UpdateSettingList(selectedNavOption, (int)Input.GetAxisRaw("Vertical"));
            UpdateSetting(0, true);
        }
        if (Input.GetButtonDown("Interact"))
        {
            ToggleSetting(1f, true);
            userInterface.UpdateSelectedSetting(0, -1);

        }
        if (Input.GetButtonDown("Cancel"))
        {
            StartCoroutine(DisableSettings());
        }
    }
    else
    {
        bool hasInput;
        (selectedSetting, hasInput) = input.GetInput("Vertical", TestInput.Axis.Vertical, userInterface.settings.Length, selectedSetting);
        if (hasInput)
        {
            UpdateSetting((int)Input.GetAxisRaw("Vertical"));

        }
        if (Input.GetButtonDown("Cancel"))
        {
            userInterface.UpdateSelectedSetting(selectedSetting, 0);
            ToggleSetting(0.3f, false);
        }
    }

    if (Input.GetButtonDown("Toggle"))
    {
        viewingMode = (ViewingMode)ExtensionMethods.IncrementInt((int)viewingMode, 0, Enum.GetValues(typeof(ViewingMode)).Length, 1);
        userInterface.UpdateSettingList(selectedNavOption, 0);
        UpdateSetting(-1);
        AnimateModeText();
    }
}

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

}

/// <summary>
/// Update is called once per frame.
/// </summary>
private void Update()
{
    if (PauseManager.instance.flags.isActive && SystemManager.instance.flags.isActive && flags.isActive)
    {
        GetInput();
    }
}

#endregion
*/
