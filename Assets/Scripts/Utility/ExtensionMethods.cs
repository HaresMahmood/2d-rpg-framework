using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

//It is common to create a class to contain all of your
//extension methods. This class must be static.
public static class ExtensionMethods
{
    //Even though they are used like normal methods, extension
    //methods must be declared static. Notice that the first
    //parameter has the 'this' keyword followed by a Transform
    //variable. This variable denotes which class the extension
    //method becomes a part of.
    public static void ResetTransformation(this Transform trans)
    {
        trans.position = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = new Vector3(1, 1, 1);
    }

    // Define an enumerator to perform our fading.
    // Pass it the material to fade, the opacity to fade to (0 = transparent, 1 = opaque),
    // and the number of seconds to fade over.
    public static IEnumerator FadeObject(this GameObject gameObject, float targetOpacity, float duration)
    {

        // Cache the current color of the material, and its initiql opacity.
        Color color;
        bool hasImage = false;
        bool hasText = false;
        Color textColor;

        if (gameObject.GetComponent<Image>() != null)
        {
            color = gameObject.GetComponent<Image>().color;
            hasImage = true;
        }
        else
            color = gameObject.GetComponent<Renderer>().material.color;

        if (gameObject.GetComponentInChildren<TextMeshProUGUI>() != null)
        {
            textColor = gameObject.GetComponentInChildren<TextMeshProUGUI>().color;
            hasText = true;
        }
        
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
            if (hasImage)
                gameObject.GetComponent<Image>().color = color;
            else
                gameObject.GetComponent<Renderer>().material.color = color;

            if (hasText)
            {
                // Blend to the corresponding opacity between start & target.
                textColor.a = Mathf.Lerp(startOpacity, targetOpacity, blend);
                // Apply the resulting color to the text.
                gameObject.GetComponentInChildren<TextMeshProUGUI>().color = color;
            }

            // Wait one frame, then repeat.
            yield return null;
        }
    }

    public static float GetAnimationInfo(this Animator animator)
    {
        AnimatorClipInfo[] currentClip = null;
        float waitTime = 0;

        currentClip = animator.GetCurrentAnimatorClipInfo(0);
        if (currentClip.Length > 0)
            waitTime = currentClip[0].clip.length;

        return waitTime;
    }

    public static void ClearNullReferences<T>(this List<T> list)
    {
        for (var i = list.Count - 1; i > -1; i--)
        {
            if (list[i] == null)
                list.RemoveAt(i);
        }
    }
}