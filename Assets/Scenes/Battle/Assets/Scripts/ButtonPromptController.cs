using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;

/// <summary>
///
/// </summary>
public class ButtonPromptController : MonoBehaviour
{
    #region Variables

    [Header("Setup")]
    [SerializeField] private InputActionAsset actionAsset;

    [Header("Settings")]
    [SerializeField, Range(0.01f, 1f)] private float animationDelay = 0.03f;
    [SerializeField, Range(0.1f, 5f)] private float animationTime = 0.15f;

    private List<ButtonList.ButtonPrompt> promptGroups;
    private List<PromptSubComponent> components;

    private string device;

    private Sequence sequence;

    #endregion

    #region Micellaneous Methods

    public void SetInformation(List<ButtonList.ButtonPrompt> promptGroups)
    {
        List<PromptSubComponent> components = this.components.Where(c => c.gameObject.activeSelf).ToList();

        sequence = DOTween.Sequence();
        this.promptGroups = promptGroups;

        for (int i = 0; i < components.Count; i++)
        {
            float timeOffset = i * animationDelay;
            Sequence componentSequence = DOTween.Sequence();

            components[i].GetComponent<Button>().interactable = false;

            componentSequence.Append(components[i].GetComponent<CanvasGroup>().DOFade(0f, animationTime));
            sequence.Insert(timeOffset, componentSequence);
        }

        sequence.OnComplete(() =>
        {
            for (int i = 0; i < promptGroups.Count; i++)
            {
                this.components[i].gameObject.SetActive(true);
            }

            for (int i = promptGroups.Count; i < this.components.Count; i++)
            {
                this.components[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < promptGroups.Count; i++)
            {
                float timeOffset = i * animationDelay;
                Sequence componentSequence = DOTween.Sequence();

                this.components[i].SetInformation(promptGroups[i], device == "Gamepad" ? 1 : 0); // TODO: *[1]
                LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());

                componentSequence.Append(this.components[i].GetComponent<CanvasGroup>().DOFade(1f, animationTime));
                sequence.Insert(timeOffset, componentSequence);
            }

            for (int i = 0; i < promptGroups.Count; i++)
            {
                this.components[i].GetComponent<Button>().interactable = true;
            }
        });
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        Controls controls = new Controls();

        // TODO: [1]
        //for (int i = 0; i < controls.controlSchemes.Count; i++)
        //Debug.Log(controls.controlSchemes[i]);

        components = GetComponentsInChildren<PromptSubComponent>().ToList();

        actionAsset.actionMaps[0].actionTriggered +=
        (InputAction.CallbackContext context) =>
        {
            var inputAction = context.action;
            var binding = inputAction.GetBindingForControl(inputAction.activeControl).Value;

            if (device != binding.groups) // TODO: *[i]
            {
                device = binding.groups;

                if (promptGroups != null)
                {
                    SetInformation(promptGroups);
                }
            }
        };
    }

    #endregion
}

