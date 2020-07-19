using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FormManager : MonoBehaviour
{
    public static FormManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public GameObject blurOverlay;

    public bool isInteracting;
    public int buttonIndex, selectedButton;
    public int maxButtonIndex = 1;
    public Dialog dialog;

    // Start is called before the first frame update
    void Start()
    {
        selectedButton = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();

        /*
        if (Input.GetButtonDown("Interact") && !DialogManager.instance.isActive)
        {
            Debug.Log("Selected button: " + selectedButton);
            StartCoroutine(SetAlpha(blurOverlay.GetComponent<Image>().material, 1f, 0.3f));

            //UnityEventHandler choiceEvent = this.transform.GetChild(selectedButton).GetComponent<UnityEventHandler>();
            //choiceEvent.eventHandler.Invoke();

            //DialogManager.instance.StartDialog(dialog);
        }
        */
    }


    public IEnumerator SetAlpha(Material material, float targetOpacity, float duration)
    {
        // Cache the current color of the material, and its initial opacity.
        Color color = material.GetColor("_OverlayColor");
        float startOpacity = color.a;

        // Track how many seconds we've been fading.
        float t = 0;

        while (t < duration)
        {
            // Step the fade forward one frame.
            t += Time.deltaTime;
            // Turn the time into an interpolation factor between 0 and 1.
            float blend = Mathf.Clamp01(t / duration);

            // Blend to the corresponding opacity between start & target.
            color.a = Mathf.Lerp(startOpacity, targetOpacity, blend);

            // Apply the resulting color to the button.
            material.SetColor("_OverlayColor", color);

            // Wait one frame, and repeat.
            yield return null;
        }
    }


    private void CheckInput()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            if (!isInteracting)
            {
                if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    if (buttonIndex < maxButtonIndex)
                    {
                        buttonIndex++;
                    }
                    else
                        buttonIndex = 0;
                }
                else if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    if (buttonIndex > 0)
                        buttonIndex--;
                    else
                        buttonIndex = maxButtonIndex;
                }
                isInteracting = true;
            }
        }
        else
            isInteracting = false;
    }
}
