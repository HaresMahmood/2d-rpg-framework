using System.Collections.Generic;
using UnityEngine;

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
    public string playerTag = "Player";
    public Transform activePlayer;
    public Color accentColor = "51C2FC".ToColor();

    [UnityEngine.Header("Global Data")]
    public bool playerInRange;

    [UnityEngine.Header("Player Data")]
    public string playerName = "Hilliard";

    public GameObject[] customizableElements;

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
    }

    private void Update()
    {
        ApplySettings();
    }

    private void ApplySettings()
    {
        foreach (GameObject element in customizableElements)
        {
            StartCoroutine(element.FadeColor(accentColor, 0.000000001f));
        }
    }

    public static Transform Player()
    {
        return instance.activePlayer;
    }
}
