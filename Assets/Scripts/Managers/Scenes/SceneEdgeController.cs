using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class SceneEdgeController : MonoBehaviour
{
    public Scene activeScene;
    public Scene nextScene;
    public string nextSceneName;

    /// <summary>
    /// The current scene root.
    /// </summary>
    [Tooltip("The root GameObject of this scene")]
    public GameObject currentSceneRoot;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GameManager.instance.playerTag))
        {
            Debug.Log("ENTERED SCENE EDGE");
            SetCurrentScene();
        }
    }

    private void SetCurrentScene()
    {
        if (currentSceneRoot)
            SceneStreamManager.SetCurrentScene(currentSceneRoot.name);
    }

}
