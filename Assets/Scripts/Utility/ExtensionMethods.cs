using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public static IEnumerator FadeOpacity(this GameObject gameObject, float targetOpacity, float duration)
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
    public static IEnumerator FadeColor(this GameObject gameObject, Color targetColor, float duration)
    {
        bool isLight = false, isImage = false, isText = false;
        Color color;

        if (gameObject.GetComponent<Light>() != null)
        {
            color = gameObject.GetComponent<Light>().color; // Caches the current color and initial opacity of image.
            isLight = true;
        }
        else if (gameObject.GetComponent<Image>() != null) // If the GameObject is an image, ...
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
            float blend = Mathf.Clamp01(t / duration); // Turns the time into an interpolation factor between 0 and 1. 

            color = Color.Lerp(startColor, targetColor, blend); // Blends to the corresponding opacity between start & target.

            if (isLight)
                gameObject.GetComponent<Light>().color = color;
            else if (isImage)
                gameObject.GetComponent<Image>().color = color;
            else if (isText)
                gameObject.GetComponent<TextMeshProUGUI>().color = color;
            else
                gameObject.GetComponent<Renderer>().material.color = color;

            yield return null; // Wait one frame, then repeat.
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetRateOverTime"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public static IEnumerator FadeParticleSystem(this GameObject targetObject, float targetRateOverTime, float duration, bool toggleParticleSystem = false)
    {
        ParticleSystem particleSystem = targetObject.GetComponent<ParticleSystem>();
        var emission = particleSystem.emission;
        float initialRateOverTime = emission.rateOverTime.constant;

        /*
        if (!targetObject.activeSelf)
        {
            targetObject.SetActive(true);
        }
        */
        float t = 0; // Tracks how many seconds we've been fading.
        while (t < duration) // While time is less than the duration of the fade, ...
        {
            if (Time.timeScale == 0)
                t += Time.unscaledDeltaTime;
            else
                t += Time.deltaTime;
            float blend = Mathf.Clamp01(t / duration); // Turns the time into an interpolation factor between 0 and 1. 

            initialRateOverTime = Mathf.Lerp(initialRateOverTime, targetRateOverTime, blend); // Blends to the corresponding opacity between start & target.
            emission.rateOverTime = initialRateOverTime;

            yield return null; // Wait one frame, then repeat.
        }
        /*
        if (toggleParticleSystem)
        {
            bool isActive = targetObject.activeSelf;
            isActive = !isActive;
            targetObject.SetActive(isActive);
        }
        */
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetValue"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public static IEnumerator LerpFloat(float initialValue, float targetValue, float duration)
    {
        float t = 0; // Tracks how many seconds we've been fading.
        while (t < duration) // While time is less than the duration of the fade, ...
        {
            if (Time.timeScale == 0)
                t += Time.unscaledDeltaTime;
            else
                t += Time.deltaTime;
            float blend = Mathf.Clamp01(t / duration); // Turns the time into an interpolation factor between 0 and 1. 

            initialValue = Mathf.Lerp(initialValue, targetValue, blend); // Blends to the corresponding opacity between start & target.

            yield return null; // Wait one frame, then repeat.
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="duration"></param>
    /// <param name="targetPosition"></param>
    /// <returns></returns>
    public static IEnumerator LerpPosition(this Transform transform, Vector2 targetPosition, float duration)
    {
        Vector3 initialPosition = transform.position;

        float t = 0; // Tracks how many seconds we've been fading.
        while (t < duration) // While time is less than the duration of the fade, ...
        {
            if (Time.timeScale == 0)
                t += Time.unscaledDeltaTime;
            else
                t += Time.deltaTime;
            float blend = Mathf.Clamp01(t / duration); // Turns the time into an interpolation factor between 0 and 1. 

            transform.position = Vector2.Lerp(initialPosition, targetPosition, blend);

            yield return null; // Wait one frame, then repeat.
        }
    }

    /*
    /// <summary>
    /// 
    /// </summary>
    /// <param name=""></param>
    /// <param name=""></param>
    /// <returns></returns>
    public static IEnumerator LerpMesh(this CanvasRenderer renderer, Mesh newMesh, float duration)
    {
        
    }
    */

    /// <summary>
    /// 
    /// </summary>
    /// <param name=""></param>
    /// <param name=""></param>
    /// <returns></returns>
    public static IEnumerator LerpScrollbar(this Scrollbar scrollbar, float targetValue, float duration)
    {
        float initialValue = scrollbar.value;

        float t = 0; // Tracks how many seconds we've been fading.
        while (t < duration) // While time is less than the duration of the fade, ...
        {
            if (Time.timeScale == 0)
                t += Time.unscaledDeltaTime;
            else
                t += Time.deltaTime;
            float blend = Mathf.Clamp01(t / duration); // Turns the time into an interpolation factor between 0 and 1. 

            scrollbar.value = Mathf.Lerp(initialValue, targetValue, blend);

            yield return null; // Wait one frame, then repeat.
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="targetSize"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public static IEnumerator LerpTextSize(this TextMeshProUGUI text, float targetSize, float duration)
    {
        float startSize = text.fontSize; // Creates a value of the initial opacity.

        float t = 0; // Tracks how many seconds we've been fading.
        while (t < duration) // While time is less than the duration of the fade, ...
        {
            if (Time.timeScale == 0)
                t += Time.unscaledDeltaTime;
            else
                t += Time.deltaTime;
            float blend = Mathf.Clamp01(t / duration); // Turns the time into an interpolation factor between 0 and 1. 

            text.fontSize = Mathf.Lerp(startSize, targetSize, blend); // Blends to the corresponding opacity between start & target.

            yield return null; // Wait one frame, then repeat.
        }
    }

    public static IEnumerator SetResetTrigger(this Animator animator, string trigger)
    {
        float waitTime = animator.GetAnimationTime();
        animator.SetTrigger(trigger);

        if (Time.timeScale == 0)
        {
            yield return new WaitForSecondsRealtime(waitTime);
        }
        else
        {
            yield return new WaitForSeconds(waitTime);
        }

        animator.ResetTrigger(trigger);
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

    public static Transform FindSibling(this Transform source, string target)
    {
        Transform parent = source.parent;
        Transform sibling = parent.Find(target);

        return sibling;
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    public static IEnumerator waitForInput(string button)
    {
        bool done = false;
        while (!done)
        {
            if (Input.GetButtonDown(button))
            {
                done = true; // breaks the loop
            }
            yield return null;
        }
    }

    public static int IncrementInt(int value, int min, int max, int increment, bool isBounded = false)
    {
        value += increment;

        if (!isBounded)
        {
            try
            {
                if (((value) % max) == 0)
                {
                    value = 0;
                }
                else if (value < 0)
                {
                    value = --max;
                }
            }
            catch (DivideByZeroException)
            {
                return 0;
            }
        }
        else
        {
            if (increment > max)
            {
                return value -= increment;
            }
        }

        max -= isBounded ? 1 : 0;
        value = Mathf.Clamp(value, min, max);

        return value;
    }

    public static string FirstToUpper(this string input)
    {
        switch (input)
        {
            case null: throw new ArgumentNullException(nameof(input));
            case "": throw new ArgumentException($"{nameof(input)} cannot be empty");
            default: return input.First().ToString().ToUpper() + input.Substring(1);
        }
    }

    /*
    public static T FindBaseObjectOfType<T>() where T : UnityEngine.Object
    {
        T destination = default;

        foreach (T child in Object.FindObjectsOfType<T>())
        {
            Debug.Log(child);
            if (!child.GetType().IsSubclassOf(typeof(T)))
            {
                destination = child;
            }
        }

        return destination;
    }
    */

    public static T[] RemoveAt<T>(this T[] source, int index)
    {
        T[] dest = new T[source.Length - 1];
        if (index > 0)
            Array.Copy(source, 0, dest, 0, index);

        if (index < source.Length - 1)
            Array.Copy(source, index + 1, dest, index, source.Length - index - 1);

        return dest;
    }

    public static void ReplaceKey<T, U>(this Dictionary<T, U> source, T key, T newKey)
    {
        if (!source.TryGetValue(key, out var value))
            throw new ArgumentException("Key does not exist", nameof(key));
        source.Remove(key);
        source.Add(newKey, value);
    }

    public static T ConvertValue<T>(string value)
    {
        return (T)Convert.ChangeType(value, typeof(T));
    }

    public static List<TSource> ToList<TSource>(this IEnumerable<TSource> source)
    {
        if (source == null)
        {
            //throw Error.ArgumentNull("source");
        }

        return new List<TSource>(source);
    }

    /*
    public static IEnumerator AnimateIndicator(this GameObject indicator, float waitTime)
    {
        Animator indicatorAnimator = indicator.GetComponent<Animator>();

        indicatorAnimator.enabled = false;
        StartCoroutine(indicator.FadeOpacity(0f, waitTime));
        yield return new WaitForSecondsRealtime(waitTime);

        indicator.transform.position = settings[selectedSetting].Find("Value").position;
        yield return null;

        indicatorAnimator.enabled = true;
    }
    */

    /*
    public static int GetLength<T>(this enum target) where T : struct, Icorn
    {
        if (!typeof(T).IsEnum)
        {
        #if DEBUG
            throw new Exception("Given type does not match required type (Enum).");
        #endif
        }

        int length = Enum.GetNames(typeof(T)).Length;
        return length;
    }

    public static T GetNextItem<T>(this CircularList<T> target, T item)
    {
        int index = target.IndexOf(item);
        index = IncrementCircularInt(index, target.Count, 1);
        return target[index];
    }

    public static T GetPreviousItem<T>(this CircularList<T> target, T item)
    {
        int index = target.IndexOf(item);
        index = IncrementCircularInt(index, target.Count, -1);
        return target[index];
    }
    */
}