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

    public static Transform Player()
    {
        return instance.player;
    }
}
