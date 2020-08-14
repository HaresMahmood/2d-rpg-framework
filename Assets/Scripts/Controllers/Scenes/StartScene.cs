using UnityEngine;

public class StartScene : MonoBehaviour
{
    /// <summary>
    /// The scene loaded at the start of the game.
    /// </summary>
    [Tooltip("The scene loaded at the start of the game.")]
    [SerializeField] private string sceneName = "SampleScene";

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    public void Start()
    {
        //SceneStreamManager.SetActive(sceneName);
        Destroy(this);
    }
}
