using UnityEngine;

/// <summary>
/// Sets the active scene in SceneStreamManager to the neighboring
/// scene of this scene.
/// </summary>
public class SceneEdgeController : MonoBehaviour
{
    /// <summary>
    /// The current scene root.
    /// </summary>
    [Tooltip("The root GameObject of this scene")]
    public GameObject currentSceneRoot;

    /// <summary>
    /// The name of the neighboring scene.
    /// </summary>
    [Tooltip("The name of the neighboring scene.")]
    [TextArea()]
    public string nextScene;

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this object (2D physics only).
    /// </summary>
    /// <param name="other"> The Collider2D attached to the entering object. </param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GameManager.PlayerTag())) // If the entering object's tag is the player tag ,...
            SetCurrentScene();
    }

    /// <summary>
    /// Sets the root object of this scene to the 
    /// active scene in SceneStreamManager.
    /// </summary>
    private void SetCurrentScene()
    {
        if (currentSceneRoot) // If the currentSceneRoot is not null ,...
            SceneStreamManager.SetActive(currentSceneRoot.name);
    }

}
