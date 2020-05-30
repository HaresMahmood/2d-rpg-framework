using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class PartyInformationUserInterface : UserInterface
{
    #region Constants

    public override int MaxObjects => informationSlots != null ? informationSlots.Count : 0;

    #endregion

    #region Variables

    protected List<PartyInformationSlot> informationSlots;

    #endregion

    #region Miscellaneous Methods

    /*
    public void SetActive(bool isActive, int selectedSlot)
    {
        //this.isActive = isActive;
        StartCoroutine(SetActive(isActive, selectedSlot));
    }

    public virtual void InitializePanel()
    {
        foreach (PartyInformationSlots slot in informationSlots)
        {
            slot.SetActive(false);
        }

        informationSlots[0].SetActive(true);
    }

    public void FadePanel(bool isActive, float opacity = 0.5f, float duration = 0.25f)
    {
        float targetOpacity = isActive ? 1 : opacity;

        StartCoroutine(gameObject.FadeOpacity(targetOpacity, duration));
    }

    protected virtual IEnumerator SetActive(bool isActive, int selectedSlot)
    {
        float duration = 0.15f;

        informationSlots[selectedSlot].SetActive(true);
        
        if (isActive)
        {
            StartCoroutine(PartyManager.instance.GetUserInterface().FadeIndicator(false));
            yield return new WaitForSecondsRealtime(duration / 2);
            FadePanel(isActive);
            informationSlots[selectedSlot].AnimateSlot(isActive);
            yield return new WaitForSecondsRealtime(duration);
            PartyManager.instance.GetUserInterface().UpdateIndicator(informationSlots, selectedSlot);
            StartCoroutine(PartyManager.instance.GetUserInterface().FadeIndicator(true));
        }
        else
        {
            FadePanel(isActive);
            informationSlots[selectedSlot].AnimateSlot(isActive);
        }

        yield return null;
    }

    protected virtual IEnumerator AnimateSlot(int selectedSlot, int increment)
    {
        float duration = 0.15f;
        int previousSlot = ExtensionMethods.IncrementInt(selectedSlot, 0, MaxObjects, increment);

        yield return null;

        //StopAllCoroutines();
        StartCoroutine(PartyManager.instance.GetUserInterface().FadeIndicator(false));
        yield return new WaitForSecondsRealtime(duration / 2);
        UpdateSlot(selectedSlot, previousSlot);
        yield return new WaitForSecondsRealtime(duration);
        PartyManager.instance.GetUserInterface().UpdateIndicator(informationSlots, selectedSlot);
        StartCoroutine(PartyManager.instance.GetUserInterface().FadeIndicator(true)); 
    }

    protected void UpdateSlot(int selectedSlot, int previousSlot)
    {
        informationSlots[selectedSlot].SetActive(true);
        informationSlots[previousSlot].SetActive(false);
    }
    */

    public virtual void SetInformation(PartyMember member)
    {   }

    public override void UpdateSelectedObject(int selectedValue, int increment)
    {
        int previousValue = ExtensionMethods.IncrementInt(selectedValue, 0, MaxObjects, increment);

        StartCoroutine(AnimeSelectedObject(selectedValue, previousValue));
    }

    public virtual void ActivateSlot(int selectedSlot, bool isActive)
    {
        informationSlots[selectedSlot].AnimateSlot(isActive);
    }

    private IEnumerator AnimeSelectedObject(int selectedValue, int previousValue, float animationDuration = 0.15f)
    {
        StartCoroutine(UpdateSelector());

        informationSlots[selectedValue].SetActive(true);
        informationSlots[previousValue].SetActive(false);

        yield return new WaitForSecondsRealtime(animationDuration); // TODO: No longer needed

        //StartCoroutine(UpdateSelector(informationSlots[selectedValue].transform.Find("Information Panel")));
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        informationSlots = GetComponentsInChildren<PartyInformationSlot>().ToList();

        selector = transform.parent.Find("Selector").gameObject;

        base.Awake();
    }

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    protected virtual void Start()
    {
        if (informationSlots != null)
        {
            for (int i = 1; i < informationSlots.Count; i++)
            {
                informationSlots[i].SetActive(false);
            }
        }
    }

    #endregion
}

/*
    #region Variables

    [Header("Values")]
    [SerializeField] [ReadOnly] protected bool isActive;

    protected readonly TestInput input = new TestInput();

    protected PartyInformationSlots[] informationSlots;

    public int selectedSlot { get; set; }


    #endregion

    #region Miscellaneous Methods

    protected void RegularInput()
    {
        bool hasInput;

        (selectedSlot, hasInput) = input.GetInput("Vertical", TestInput.Axis.Vertical, informationSlots.Length, selectedSlot);
        if (hasInput)
        {
            StartCoroutine(AnimateSlot(selectedSlot, (int)Input.GetAxisRaw("Vertical")));
        }
    }

    protected virtual void GetInput()
    {
        bool hasInput;

        (selectedSlot, hasInput) = input.GetInput("Vertical", TestInput.Axis.Vertical, informationSlots.Length, selectedSlot);
        if (hasInput)
        {
            StartCoroutine(AnimateSlot(selectedSlot, (int)Input.GetAxisRaw("Vertical")));
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    protected virtual void Start()
    {
        informationSlots = transform.GetComponentsInChildren<PartyInformationSlots>();

        InitializePanel();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    protected void Update()
    {
        if (PauseManager.instance.flags.isActive && isActive) // && PartyManager.instance.flags.isActive
        {
            GetInput();
        }
    }

    #endregion
    */
