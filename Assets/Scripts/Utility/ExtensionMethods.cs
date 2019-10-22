using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A collection of extension methods.
/// </summary>
public static class ExtensionMethods
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static float SmoothStep(float t)
    {
        return t * t * t * (t * (6f * t - 15f) + 10f);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="light"></param>
    /// <param name="targetIntensity"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public static IEnumerator FadeLight(this Light light, float targetIntensity, float duration)
    {
        float startIntensity = light.intensity; // Creates a value of the initial opacity.

        float t = 0; // Tracks how many seconds we've been fading.
        while (t < duration) // While time is less than the duration of the fade, ...
        {
            if (Time.timeScale == 0)
                t += Time.unscaledDeltaTime;
            else
                t += Time.deltaTime;
            float blend = Mathf.Clamp01(t / duration); // Turns the time into an interpolation factor between 0 and 1. 

            light.intensity = Mathf.Lerp(startIntensity, targetIntensity, blend); // Blends to the corresponding opacity between start & target.

            yield return null; // Wait one frame, then repeat.
        }
    }

    /// <summary>
    /// Defines an enumerator to perform fading on a GameObject.
    /// </summary>
    /// <param name="gameObject"> GameObject to fade. </param>
    /// <param name="targetOpacity"> Opacity (0 to 1) to fade GameObject to. </param>
    /// <param name="duration"> Duration of fade. </param>
    /// <returns></returns>
    public static IEnumerator FadeObject(this GameObject gameObject, float targetOpacity, float duration)
    {
        bool isImage = false, isCanvas = false, isText = false, hasText = false;
        Color color = new Color();

        if (gameObject.GetComponent<CanvasGroup>() != null)
        {
            color.a = gameObject.GetComponent<CanvasGroup>().alpha;
            isCanvas = true;
        }
        else if (gameObject.GetComponent<TextMeshProUGUI>() != null)
        {
            color = gameObject.GetComponent<TextMeshProUGUI>().color;
            isText = true;
        }
        else if (gameObject.GetComponent<Image>() != null) // If the GameObject is an image, ...
        {
            color = gameObject.GetComponent<Image>().color; // Caches the current color and initial opacity of image.
            isImage = true;
        }
        else
        {
            color = gameObject.GetComponent<Renderer>().material.color; // Caches the current color of and initial opacity of material.
        }

        if (gameObject.GetComponentInChildren<TextMeshProUGUI>() != null && gameObject.GetComponent<CanvasGroup>() == null) // If the GameObject has a TextMeshPro object as child, ...
            hasText = true;

        float startOpacity = color.a; // Creates a value of the initial opacity.

        float t = 0; // Tracks how many seconds we've been fading.
        while (t < duration) // While time is less than the duration of the fade, ...
        {
            if (Time.timeScale == 0)
                t += Time.unscaledDeltaTime;
            else
                t += Time.deltaTime;
            float blend = Mathf.Clamp01(t / duration); // Turns the time into an interpolation factor between 0 and 1. 

            color.a = Mathf.Lerp(startOpacity, targetOpacity, blend); // Blends to the corresponding opacity between start & target.

            if (isImage)
                gameObject.GetComponent<Image>().color = color;
            else if (isCanvas)
                gameObject.GetComponent<CanvasGroup>().alpha = color.a;
            else if (isText)
                gameObject.GetComponent<TextMeshProUGUI>().color = color;
            else
                gameObject.GetComponent<Renderer>().material.color = color;

            if (hasText)
                gameObject.GetComponentInChildren<TextMeshProUGUI>().color = color;

            yield return null; // Wait one frame, then repeat.
        }
    }

    /// <summary>
    /// Defines an enumerator to perform fading on a GameObject.
    /// </summary>
    /// <param name="gameObject"> GameObject to fade. </param>
    /// <param name="targetColor"> Color to change GameObject to. </param>
    /// <param name="duration"> Duration of color change. </param>
    /// <returns></returns>
    public static IEnumerator ColorObject(this GameObject gameObject, Color targetColor, float duration)
    {
        bool isImage = false, isText = false;
        Color color;

        if (gameObject.GetComponent<Image>() != null) // If the GameObject is an image, ...
        {
            color = gameObject.GetComponent<Image>().color; // Caches the current color and initial opacity of image.
            isImage = true;
        }
        else if (gameObject.GetComponent<TextMeshProUGUI>() != null)
        {
            color = gameObject.GetComponent<TextMeshProUGUI>().color;
            isText = true;
        }
        else
            color = gameObject.GetComponent<Renderer>().material.color; // Caches the current color of and initial opacity of material. 

        Color startColor = color; // Creates a value of the initial opacity.

        float t = 0; // Tracks how many seconds we've been fading.
        while (t < duration) // While time is less than the duration of the fade, ...
        {
            if (Time.timeScale == 0)
                t += Time.unscaledDeltaTime;
            else
                t += Time.deltaTime;
            float blend = Mathf.Clamp01(t / duration); Debug.Log(t); // Turns the time into an interpolation factor between 0 and 1. 

            color = Color.Lerp(startColor, targetColor, blend); // Blends to the corresponding opacity between start & target.

            if (isImage)
                gameObject.GetComponent<Image>().color = color;
            else if (isText)
                gameObject.GetComponent<TextMeshProUGUI>().color = color;
            else
                gameObject.GetComponent<Renderer>().material.color = color;

            yield return null; // Wait one frame, then repeat.
        }
    }

    /// <summary>
    /// Gets the time of the current clip playing in any Animator.
    /// </summary>
    /// <param name="animator"> The Animator attached to the GameObject. </param>
    /// <returns></returns>
    public static float GetAnimationTime(this Animator animator)
    {
        AnimatorClipInfo[] currentClip = null;
        float waitTime = 0;

        currentClip = animator.GetCurrentAnimatorClipInfo(0);
        if (currentClip.Length > 0)
            waitTime = currentClip[0].clip.length;

        return waitTime;
    }

    /// <summary>
    /// Checks for and clears any missing (null) entries in a List.
    /// </summary>
    /// <typeparam name="T"> Generic The element type of the List. </typeparam>
    /// <param name="list"> Generic List with missing (null) entries. </param>
    public static void ClearNullReferences<T>(this List<T> list)
    {
        List<T> templist = new List<T>(); // Creates
        for (int i = 0; i < list.Count - 1; i++)
            if (list[i] != null) templist.Add(list[i]);

        list = templist;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static Color ToColor(this string color)
    {
        if (color.StartsWith("#", StringComparison.InvariantCulture))
            color = color.Substring(1); // strip #

        if (color.Length == 6)
            color += "FF"; // add alpha if missing

        var hex = Convert.ToUInt32(color, 16);
        var r = ((hex & 0xff000000) >> 0x18) / 255f;
        var g = ((hex & 0xff0000) >> 0x10) / 255f;
        var b = ((hex & 0xff00) >> 8) / 255f;
        var a = ((hex & 0xff)) / 255f;

        return new Color(r, g, b, a);
    }

    /// <summary>
    /// Removes parent Transform from 
    /// MonoBehaviour.GetComonentsInChildren<>() and returns an array 
    /// containing only the parent's children.
    /// </summary>
    /// <param name="parent"> Transform of GameObject with children. </param>
    /// <returns> An array containing the children of the parent Transform. </returns>
    public static Transform[] GetChildren(this Transform parent)
    {
        Transform[] children = parent.GetComponentsInChildren<Transform>();
        Transform[] firstChildren = new Transform[parent.childCount];
        int index = 0;
        foreach (Transform child in children)
        {
            if (child.parent == parent)
            {
                firstChildren[index] = child;
                index++;
            }
        }

        return firstChildren;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="color"></param>
    /// <param name="thickness"></param>
    /// <param name="margin"></param>
    /// <param name="padding"></param>
    public static void DrawUILine(Color color, int thickness = 2, int margin = 0, int padding = 5)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness; r.width -= margin;
        r.y += padding / 2;
        EditorGUI.DrawRect(r, color);
    }


}