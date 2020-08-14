using System;
using UnityEngine;

/// <summary>
/// Sets the active scene in SceneStreamManager to the neighboring
/// scene of this scene.
/// </summary>
public class SceneEdgeController : MonoBehaviour
{
    #region Fields

    [Header("Setup")]
    [Tooltip("The root GameObject of this scene")]
    [SerializeField] private GameObject root;

    [Tooltip("The name of the neighboring scene.")]
    [TextArea()]
    [SerializeField] private string nextScene;

    #endregion

    #region Properties

    public string NextScene
    {
        get { return nextScene; }
    }

    #endregion

    #region Miscellaneous Methods

    /// <summary>
    /// Sets the root object of this scene to the 
    /// active scene in SceneStreamManager.
    /// </summary>
    private void SetCurrentScene()
    {
        if (root) // If the currentSceneRoot is not null ,...
        {
            //SceneStreamManager.SetActive(root.name);
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this object (2D physics only).
    /// </summary>
    /// <param name="other"> The Collider2D attached to the entering object. </param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GameManager.PlayerTag())) // If the entering object's tag is the player tag ,...
        {
            SetCurrentScene();
        }
    }

    #endregion
}
