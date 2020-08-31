using UnityEngine;

public class StartScene : MonoBehaviour
{
    #region Variables

    [Tooltip("The scene loaded at the start of the game.")]
    [SerializeField] private string sceneName = "SampleScene";

    #endregion

    #region Unity Methods

    public void Start()
    {
        GetComponent<SceneStreamManager>().SetActiveScene(sceneName);
        Destroy(this);
    }

    #endregion
}
