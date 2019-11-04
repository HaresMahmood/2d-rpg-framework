using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    [UnityEngine.Header("General Settings")]
    public Transform player;

    [UnityEngine.Header("Global Settings")]
    [SerializeField] private string playerTag = "Player";
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

    [System.Serializable]
    public class Settings
    {
        public static Color accentColor;
    }

    private void Start()
    {
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
        return instance.playerTag;
    }

    public static string PlayeName()
    {
        return instance.playerName;
    }

    public static Color AccentColor()
    {
        return instance.accentColor;
    }

    public static bool Debug()
    {
        return instance.debug;
    }
}
