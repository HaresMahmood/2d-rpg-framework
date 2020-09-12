using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/// <summary>
///
/// </summary>
public class ButtonPromptController : MonoBehaviour
{
    #region Variables

    [Header("Setup")]
    [SerializeField] private InputActionAsset actionAsset;

    private List<ButtonList.ButtonPrompt> promptGroups;
    private List<PromptSubComponent> buttons;

    private string device;

    #endregion

    #region Micellaneous Methods

    public void SetInformation(List<ButtonList.ButtonPrompt> promptGroups)
    {
        this.promptGroups = promptGroups;

        for (int i = 0; i < promptGroups.Count; i++)
        {
            buttons[i].gameObject.SetActive(true);

            buttons[i].SetInformation(promptGroups[i], device == "Gamepad" ? 1 : 0); // TODO: *[1]
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }

        for (int i = promptGroups.Count; i < buttons.Count; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        Controls controls = new Controls();

        // TODO: [1]
        //for (int i = 0; i < controls.controlSchemes.Count; i++)
        //Debug.Log(controls.controlSchemes[i]);

        buttons = GetComponentsInChildren<PromptSubComponent>().ToList();

        actionAsset.actionMaps[0].actionTriggered +=
        (InputAction.CallbackContext context) =>
        {
            var inputAction = context.action;
            var binding = inputAction.GetBindingForControl(inputAction.activeControl).Value;

            if (device != binding.groups) // TODO: *[i]
            {
                device = binding.groups;

                //SetInformation(promptGroups);
            }
        };
    }

    #endregion
}

