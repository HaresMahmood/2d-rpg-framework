using UnityEngine;

/// <summary>
/// Add this to the scene's root object. It lists the scene's neighbors.
/// SceneStreamer uses it to determine which neighbors to load and unload.
/// If the scene's root object doesn't have this component, SceneStreamer 
/// will generate it automatically at load time, which takes a little time.
/// </summary>
public class NeighboringScenes : MonoBehaviour
{
    /// <summary>
    /// The scenes neighboring this scene.
    /// </summary>
    [Tooltip("The scenes neighboring this scene.")]
    public string[] sceneNames;
}