using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System;

public class PromptSubComponent : MonoBehaviour
{
    #region Variables

    private Button button;

    private TextMeshProUGUI promptText;
    private TextMeshProUGUI promptAction;
    private Image promptIcon;

    ButtonList.ButtonPrompt promptGroup;

    #endregion

    #region Miscellaneous Methods

    public void SetInformation(ButtonList.ButtonPrompt promptGroup, int index)
    {
        if (this.promptGroup != null)

        {
            this.promptGroup?.Action.action.Enable();
            this.promptGroup.Action.action.performed -= OnActionPerformed;
        }

        promptGroup.Action.action.Enable();
        promptGroup.Action.action.performed += OnActionPerformed;

        this.promptGroup = promptGroup;

        button.onClick.RemoveAllListeners(); // TODO: Might not be necessary
        button.onClick.AddListener(promptGroup.OnClick.Invoke);

        promptText.SetAutoTextWidth(promptGroup.Text);
        promptAction.SetAutoTextWidth(promptGroup.Prompts[index].Text);

        promptIcon.sprite = promptGroup.Prompts[index].Icon;
        promptIcon.gameObject.SetActive(promptIcon.sprite != null);
    }

    #endregion

    #region Event Methods

    public void OnClick()
    {
        Debug.Log(transform.name);
    }

    private void OnActionPerformed(InputAction.CallbackContext context)
    {
        if (promptGroup.Value == 0 || (promptGroup.Value != 0 && context.ReadValue<float>() == promptGroup.Value))
        {
            button.onClick.Invoke();
        }
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        button = GetComponent<Button>();

        promptText = transform.Find("Text").GetComponent<TextMeshProUGUI>();

        promptAction = transform.Find("Button/Binding").GetComponent<TextMeshProUGUI>();
        promptIcon = transform.Find("Button/Icon").GetComponent<Image>();
    }

    #endregion

    #region Nested Class

    [System.Serializable]
    internal class Prompt
    {
        #region Fields

        [SerializeField] internal string text;
        [SerializeField] internal Sprite icon;

        #endregion
    }

    #endregion
}
