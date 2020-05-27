using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [UnityEngine.Header("General Settings")]
    [SerializeField] private const string playerTag = "Player";

    [UnityEngine.Header("General Settings")]
    public Transform player;

    [UnityEngine.Header("Global Settings")]
    [SerializeField] private Transform activePlayer;
    [SerializeField] private Color accentColor = "51C2FC".ToColor();
    [SerializeField] private TMP_FontAsset dyslexiaFont;
    [SerializeField] private bool dyslexiaMode;

    [UnityEngine.Header("Global Data")]
    public bool playerInRange;

    [UnityEngine.Header("Player Data")]
    [SerializeField] private string playerName = "Hilliard";

    [UnityEngine.Header("Debug")]
    [SerializeField] private bool debug;

    private Color initialColor;
    [SerializeField] private Color oppositeColor;

    [System.Serializable]
    public class Settings
    {
        public static Color accentColor;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;



        if (player != null)
        {
            for (int i = 0; i < player.childCount; i++)
            {
                if (player.GetChild(i).gameObject.activeSelf == true)
                {
                    activePlayer = player.GetChild(i);
                    return;
                }
            }
        }

        ApplySettings();
    }

    private void Update()
    {
        ApplySettings();
    }

    private void ApplySettings()
    {
        if (initialColor != accentColor)
        {
            initialColor = accentColor;
            TextMeshProUGUI[] textElements = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();
            foreach (TextMeshProUGUI element in textElements)
            {
                if (element.CompareTag("Customizable"))
                {
                    element.color = accentColor;
                }
            }
            Image[] imageElements = Resources.FindObjectsOfTypeAll<Image>();
            foreach (Image element in imageElements)
            {
                if (element.CompareTag("Customizable"))
                {
                    element.color = accentColor;
                }
            }
            CustomRenderer[] canvasElements = Resources.FindObjectsOfTypeAll<CustomRenderer>();
            foreach (CustomRenderer element in canvasElements)
            {
                if (element.CompareTag("Customizable"))
                {
                    element.GetComponent<CanvasRenderer>().SetColor(accentColor);
                }
            }
        }

        Color.RGBToHSV(accentColor, out float h, out float s, out float v);
        h = (h + 180) % 360;
        //oppositeColor = Color.HSVToRGB(h, s, v);

        TextMeshProUGUI[] text = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();
        foreach (TextMeshProUGUI element in text)
        {
            if (element.CompareTag("Opposite"))
            {
                element.color = oppositeColor;
            }
        }
        Image[] image = Resources.FindObjectsOfTypeAll<Image>();
        foreach (Image element in image)
        {
            if (element.CompareTag("Opposite"))
            {
                element.color = oppositeColor;
            }
        }
        CustomRenderer[] canvas = Resources.FindObjectsOfTypeAll<CustomRenderer>();
        foreach (CustomRenderer element in canvas)
        {
            if (element.CompareTag("Opposite"))
            {
                element.GetComponent<CanvasRenderer>().SetColor(oppositeColor);
            }
        }

        if (dyslexiaMode)
        {
            TextMeshProUGUI[] textElements = FindObjectsOfType<TextMeshProUGUI>();
            foreach (TextMeshProUGUI element in textElements)
            {
                element.font = dyslexiaFont;
            }
        }
    }

    public static Transform Player()
    {
        return instance.activePlayer;
    }

    public static string PlayerTag()
    {
        return playerTag;
    }

    public static string GetPlayerName()
    {
        return instance.playerName;
    }

    public static Color GetAccentColor()
    {
        return instance.accentColor;
    }

    public static void SetAccentColor(Color accentColor)
    {
        instance.accentColor = accentColor;
    }

    public static bool Debug()
    {
        return instance.debug;
    }
}
