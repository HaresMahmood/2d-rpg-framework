using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// </summary>
public class PartyInformationSlot : Slot
{
    #region Variables

    [Header("Values")]
    [SerializeField] private bool isActive;

    private bool IsActive
    {
        get { return IsActive; }
        set { IsActive = value; SetActive(value); }
    }

    private RectTransform margin;
    private Transform[] informationContainers;

    #endregion

    #region Miscellaneous Methods

    public void SetActive(bool isActive)
    {
        this.isActive = isActive;

        if (!gameObject.activeSelf && isActive)
        {
            gameObject.SetActive(true);
        }

        informationContainers[1].gameObject.SetActive(isActive);
        informationContainers[2].gameObject.SetActive(isActive);

        AnimateSlot(isActive);
    }

    public void AnimateSlot(bool isSelected)
    {
        float width = isSelected ? 100f : 0f;
        StartCoroutine(ExpandMargin(width));
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetComponent<RectTransform>());
    }

    private IEnumerator ExpandMargin(float targetWidth, float duration = 0.15f)
    {
        Vector2 startWidth = margin.sizeDelta; // Creates a value of the initial opacity.

        float t = 0; // Tracks how many seconds we've been fading.
        while (t < duration) // While time is less than the duration of the fade, ...
        {
            if (Time.timeScale == 0)
                t += Time.unscaledDeltaTime;
            else
                t += Time.deltaTime;
            float blend = Mathf.Clamp01(t / duration); // Turns the time into an interpolation factor between 0 and 1. 

            margin.sizeDelta = Vector2.Lerp(startWidth, new Vector2(targetWidth, margin.sizeDelta.y), blend); // Blends to the corresponding opacity between start & target.

            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetComponent<RectTransform>());

            yield return null; // Wait one frame, then repeat.
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        margin = transform.Find("Margin").GetComponent<RectTransform>();
        informationContainers = transform.Find("Information Panel").GetChildren();
    }

    #endregion
}
