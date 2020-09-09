using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class InputPrompt : MonoBehaviour
{
    #region Variables

    //[SerializeField] private InputActionReference inputActionReference;

    [SerializeField] private List<Prompt> prompts = new List<Prompt>();
    [SerializeField] InputDevice device;

    private TextMeshProUGUI text;
    private Image icon;

    #endregion

    #region Miscellaneous Methods

    private void OnInputDeviceChange(int i)
    {
        text.SetText(prompts[i].text);

        icon.sprite = prompts[i].icon;
        icon.gameObject.SetActive(prompts[i].icon != null);
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        text = transform.Find("Button/Binding").GetComponent<TextMeshProUGUI>();
        icon = transform.Find("Button/Icon").GetComponent<Image>();

        /*
        foreach (InputDevice device in InputSystem.devices)
        {
            Debug.Log(device.);
        }

        InputSystem.onDeviceChange +=
        (device, change) =>
        {
            switch (change)
            {
                case InputDeviceChange.Enabled:
                    Debug.Log(device);
                    this.device = device;
                    break;
            }
        };
        */
    }

    private void Start()
    {
        OnInputDeviceChange(0);
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
