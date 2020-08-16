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

    #region Unity Methods

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GameManager.PlayerTag())) // If the entering object's tag is the player tag ,...
        {
            if (root) // If the currentSceneRoot is not null ,...
            {
                SceneStreamManager.SetActive(root.name);
            }
        }
    }

    #endregion
}
