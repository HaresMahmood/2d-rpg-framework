using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
///
/// </summary>
public class ButtonPromptController : MonoBehaviour
{
    #region Variables

    [Header("Setup")]
    [SerializeField] private InputActionAsset actionAsset;

    private List<ButtonPrompt> buttonPrompts;

    private string device;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        buttonPrompts = GetComponentsInChildren<ButtonPrompt>().ToList();

        actionAsset.actionMaps[0].actionTriggered +=
    (InputAction.CallbackContext context) =>
    {
        var inputAction = context.action;
        var binding = inputAction.GetBindingForControl(inputAction.activeControl).Value;

        if (device != binding.groups)
        {
            device = binding.groups;

            foreach (ButtonPrompt buttonPrompt in buttonPrompts)
            {
                buttonPrompt.SetInformation(device == "Gamepad" ? 1 : 0); // TODO: Think of better way of doing this
                LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
            }
        }
    };
    }

    #endregion
}

