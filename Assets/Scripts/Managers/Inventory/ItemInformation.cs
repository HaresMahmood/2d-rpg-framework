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
public class ItemInformation : MonoBehaviour
{
    #region Variables

    private GameObject informationPanel;

    private GameObject verticalInformation;
    private GameObject horizontalInformation;

    private Animator informationAnimator;

    private TextMeshProUGUI nameText;
    private TextMeshProUGUI descriptionText;
    private TextMeshProUGUI valueText;
    private Image spriteImage;

    private RectTransform buttonPanel;
    public Transform[] buttons { get; private set; }

    #endregion

    #region Miscellaneous Methods

    private void SetObjectDefinitions(Transform panel)
    {
        nameText = panel.Find("Name/Value").GetComponent<TextMeshProUGUI>();
        descriptionText = panel.Find("Description/Value").GetComponent<TextMeshProUGUI>();

        if (panel.name.Contains("horizontal"))
        {
            valueText = panel.Find("Amount/Value").GetComponent<TextMeshProUGUI>();
            spriteImage = panel.Find("Name/Icon").GetComponent<Image>();
        }
    }

    /// <summary>
    /// Sets the desciption of the selected item.
    /// </summary>
    /// <param name="item"> Item currently selected. </param>
    /// <param name="duration"> Duration of the animation. Defaults to 0.1f. </param>
    /// <returns> Co-routine. </returns>
    public IEnumerator SetInformation(Item item = null, float duration = 0.1f)
    {
        if (item != null)
        {
            StartCoroutine(verticalInformation.FadeOpacity(0f, duration));

            yield return new WaitForSecondsRealtime(duration);

            nameText.SetText(item.Name);
            descriptionText.SetText(item.description);

            StartCoroutine(verticalInformation.FadeOpacity(1f, duration));
        }
        else
        {
            StartCoroutine(verticalInformation.FadeOpacity(0f, duration));
        }
    }

    /// <summary>
    /// Sets correct information for and animates sub-menu buttons.
    /// </summary>
    /// <param name="item"> The selected item. </param>
    /// <param name="duration"> The duration of the animations. </param>
    /// <param name="delay"> The delay at which certain animations should occur. </param>
    /// <returns></returns>
    public IEnumerator AnimateMenuButtons(Item item = null, float duration = 0.2f, float delay = 0.03f)
    {
        if (item != null)
        {
            List<ItemBehavior> itemButtons = item.GenerateButtons(); 
            FindObjectOfType<InventoryUserInterface>().itemButtons = itemButtons;

            for (int i = 0; i < itemButtons.Count; i++)
            {
                buttons[i].gameObject.SetActive(true);

                if (buttons[i].GetComponent<CanvasGroup>().alpha > 0) buttons[i].GetComponent<CanvasGroup>().alpha = 0;

                if (Array.IndexOf(buttons, buttons[i]) < itemButtons.Count)
                {
                    buttons[i].GetComponentInChildren<TextMeshProUGUI>().SetText(itemButtons[i].buttonName);
                    StartCoroutine(buttons[i].gameObject.FadeOpacity(1f, duration));

                    yield return new WaitForSecondsRealtime(delay);
                }
                else
                {
                    buttons[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; (i < buttons.Length || buttons[i].GetComponent<CanvasGroup>().alpha < 1); i++)
            {
                StartCoroutine(buttons[i].gameObject.FadeOpacity(0f, duration));

                yield return new WaitForSecondsRealtime(delay);
            }

            FindObjectOfType<InventoryUserInterface>().itemButtons.Clear();
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        informationPanel = transform.gameObject;

        informationAnimator = informationPanel.GetComponent<Animator>();

        verticalInformation = informationPanel.transform.Find("Information (Vertical)").gameObject;
        horizontalInformation = informationPanel.transform.Find("Information (Horizontal)").gameObject;

        buttonPanel = horizontalInformation.transform.Find("Buttons").GetComponent<RectTransform>();
        buttons = buttonPanel.GetChildren();

        SetObjectDefinitions(verticalInformation.transform);
    }

    #endregion
}
