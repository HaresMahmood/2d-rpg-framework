using System.Collections;
using UnityEngine;

/// <summary>
///
/// </summary>
public class PartyInformationPanel : MonoBehaviour
{
    /*
    #region Variables

    [Header("Values")]
    [SerializeField] [ReadOnly] protected bool isActive;

    protected readonly TestInput input = new TestInput();

    protected PartyInformationSlots[] informationSlots;

    public int selectedSlot { get; set; }


    #endregion

    #region Miscellaneous Methods

    public void SetActive(bool isActive)
    {
        this.isActive = isActive;
        StartCoroutine(SetActive(isActive, selectedSlot));
    }

    protected virtual IEnumerator SetActive(bool isActive, int selectedSlot)
    {
        float duration = 0.15f;

        informationSlots[selectedSlot].SetActive(true);
        //StopAllCoroutines();
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

    protected void UpdateSlot(int selectedSlot, int previousSlot)
    {
        informationSlots[selectedSlot].SetActive(true);
        informationSlots[previousSlot].SetActive(false);
    }

    protected virtual IEnumerator AnimateSlot(int selectedSlot, int increment)
    {
        float duration = 0.15f;
        int previousSlot = ExtensionMethods.IncrementInt(selectedSlot, 0, informationSlots.Length, increment);

        //StopAllCoroutines();
        StartCoroutine(PartyManager.instance.GetUserInterface().FadeIndicator(false));
        yield return new WaitForSecondsRealtime(duration / 2);
        UpdateSlot(selectedSlot, previousSlot);
        yield return new WaitForSecondsRealtime(duration);
        PartyManager.instance.GetUserInterface().UpdateIndicator(informationSlots, selectedSlot);
        StartCoroutine(PartyManager.instance.GetUserInterface().FadeIndicator(true));
    }

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
}
