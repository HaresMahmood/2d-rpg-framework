using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FormHandler : MonoBehaviour
{
    public int buttonIndex;
    private BlurEvent bevent;

    // Update is called once per frame
    private void Update()
    {
        if (FormManager.instance.buttonIndex == buttonIndex)
            FormManager.instance.selectedButton = buttonIndex;

        if (FormManager.instance.selectedButton == buttonIndex)
        {
            StopAllCoroutines();
            StartCoroutine(FadeButton(this.gameObject, 0.3f, 0.3f));
            //Debug.Log("FADING");
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(FadeButton(this.gameObject, 1f, 0.3f));
            //Debug.Log("FADING");
        }

    }

    // Define an enumerator to perform our fading.
    // Pass it the material to fade, the opacity to fade to (0 = transparent, 1 = opaque),
    // and the number of seconds to fade over.
    public IEnumerator FadeButton(GameObject button, float targetOpacity, float duration)
    {
        // Cache the current color of the material, and its initial opacity.
        Color buttonColor = button.GetComponent<Image>().color;
        float startOpacity = buttonColor.a;

        // Track how many seconds we've been fading.
        float t = 0;

        while (t < duration)
        {
            // Step the fade forward one frame.
            t += Time.deltaTime;
            // Turn the time into an interpolation factor between 0 and 1.
            float blend = Mathf.Clamp01(t / duration);

            // Blend to the corresponding opacity between start & target.
            buttonColor.a = Mathf.Lerp(startOpacity, targetOpacity, blend);

            // Apply the resulting color to the button.
            button.GetComponent<Image>().color = buttonColor;

            // Wait one frame, and repeat.
            yield return null;
        }
    }
}
