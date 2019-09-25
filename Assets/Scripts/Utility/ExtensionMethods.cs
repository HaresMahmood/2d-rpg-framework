using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

/// <summary>
/// A collection of extension methods.
/// </summary>
public static class ExtensionMethods
{
    /// <summary>
    /// Defines an enumerator to perform fading on a GameObject.
    /// </summary>
    /// <param name="gameObject"> GameObject to fade. </param>
    /// <param name="targetOpacity"> Opacity (0 to 1) to fade GameObject to. </param>
    /// <param name="duration"> Duration of fade. </param>
    /// <returns></returns>
    public static IEnumerator FadeObject(this GameObject gameObject, float targetOpacity, float duration)
    {
        bool isImage = false, hasText = false;
        Color color;

        if (gameObject.GetComponent<Image>() != null) // If the GameObject is an image, ...
        {
            color = gameObject.GetComponent<Image>().color; // Caches the current color and initial opacity of image.
            isImage = true;
        }
        else
            color = gameObject.GetComponent<Renderer>().material.color; // Caches the current color of and initial opacity of material.

        if (gameObject.GetComponentInChildren<TextMeshProUGUI>() != null) // If the GameObject has a TextMeshPro object as child, ...
            hasText = true;

        float startOpacity = color.a; // Creates a value of the initial opacity.

        float t = 0; // Tracks how many seconds we've been fading.
        while (t < duration) // While time is less than the duration of the fade, ...
        {
            t += Time.deltaTime; // Steps the fade forward one frame.
            float blend = Mathf.Clamp01(t / duration); // Turns the time into an interpolation factor between 0 and 1. 

            color.a = Mathf.Lerp(startOpacity, targetOpacity, blend); // Blends to the corresponding opacity between start & target.

            if (isImage)
                gameObject.GetComponent<Image>().color = color;
            else
                gameObject.GetComponent<Renderer>().material.color = color;

            if (hasText)
                gameObject.GetComponentInChildren<TextMeshProUGUI>().color = color;

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