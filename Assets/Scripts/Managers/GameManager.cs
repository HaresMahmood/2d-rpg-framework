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

    [UnityEngine.Header("Global Data")]
    public bool playerInRange;

    [UnityEngine.Header("Player Data")]
    public string playerName = "Hilliard";
    public Party party;

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

    public static Transform Player()
    {
        return instance.activePlayer;
    }
}
