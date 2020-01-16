using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class PartyInformationPanel : MonoBehaviour
{
    #region Variables

    [Header("Values")]
    [SerializeField] [ReadOnly] private bool isActive;

    private readonly TestInput input = new TestInput();

    private PartyInformationSlots[] informationSlots;

    private int selectedSlot;


    #endregion

    #region Miscellaneous Methods

    public void SetActive(bool isActive)
    {
        this.isActive = isActive;

        AnimatePanel(isActive);
        informationSlots[selectedSlot].AnimateSlot(isActive);
    }

    private void InitializePanel()
    {
        foreach (PartyInformationSlots slot in informationSlots)
        {
            slot.SetActive(false);
        }

        informationSlots[0].SetActive(true);
    }

    private void AnimatePanel(bool isActive, float opacity = 0.7f, float duration = 0.25f)
    {
        float targetOpacity = isActive ? 1 : opacity;

        StartCoroutine(gameObject.FadeOpacity(targetOpacity, duration));
    }

    private void UpdateSlot(int selectedSlot, int previousSlot)
    {
        informationSlots[selectedSlot].SetActive(true);
        informationSlots[previousSlot].SetActive(false);
    }

    private IEnumerator AnimateSlot(int selectedSlot, int increment)
    {
        float duration = 0.15f;
        int previousSlot = ExtensionMethods.IncrementInt(selectedSlot, 0, informationSlots.Length, increment);

        StopAllCoroutines();
        StartCoroutine(PartyManager.instance.GetUserInterface().FadeIndicator(false));
        yield return new WaitForSecondsRealtime(duration / 2);
        UpdateSlot(selectedSlot, previousSlot);
        yield return new WaitForSecondsRealtime(duration);
        PartyManager.instance.GetUserInterface().UpdateIndicator(informationSlots, selectedSlot);
        StartCoroutine(PartyManager.instance.GetUserInterface().FadeIndicator(true));
    }

    private void GetInput()
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
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        informationSlots = transform.GetComponentsInChildren<PartyInformationSlots>();
        
        InitializePanel();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        if (PauseManager.instance.flags.isActive && isActive) // && PartyManager.instance.flags.isActive
        {
            GetInput();
        }
    }

    #endregion
}
