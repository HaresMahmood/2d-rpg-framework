using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// SceneStreamer is a singleton MonoBehavior used to load and unload scenes that contain
/// pieces of the game world. You can use it to implement continuous worlds. The piece
/// of the world containing the player is called the "current scene." SceneStreamer 
/// automatically loads neighboring scenes up to a distance you specify and unloads 
/// scenes beyond that distance.
/// </summary>
public class SceneStreamManager : MonoBehaviour
{
    #region Fields

    private static SceneStreamManager instance;

    #endregion

    #region Properties

    /// <summary>
    /// Singleton pattern.
    /// </summary>
    public static SceneStreamManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SceneStreamManager>();
            }

            return instance;
        }
    }

    #endregion

    #region Variables

    [Header("Settings")]
    [Tooltip("Max number of neighbors to load out from the current scene.")]
    [SerializeField] private int drawDistance = 1;

    [Tooltip("If scene doesn't load after specified amount of seconds, stop waiting.")]
    [SerializeField] private float loadTime = 10f;

    [System.Serializable] public class StringEvent : UnityEvent<string> { }
    public StringEvent onLoaded = new StringEvent();
    [System.Serializable] public class StringAsyncEvent : UnityEvent<string, AsyncOperation> { }
    public StringAsyncEvent onLoading = new StringAsyncEvent();

    [Tooltip("Tick to log debug info to the Console window.")]
    [SerializeField] private bool debug = false;

    public bool logDebugInfo { get { return debug && Debug.isDebugBuild; } }

    /// <summary>
    /// // The name of the scene the Player is currently in.
    /// </summary>
    private string activeScene = null;

    /// <summary>
    /// The names of all loaded scenes.
    /// </summary>
    private HashSet<string> loadedScenes = new HashSet<string>();

    /// <summary>
    /// The names of all scenes that are in the process of being loaded.
    /// </summary>
    private HashSet<string> loadingScenes = new HashSet<string>();

    /// <summary>
    /// The names of all scenes within drawDistance of the current scene.
    /// This is used when determining which neighboring scenes to load or unload.
    /// </summary>
    private HashSet<string> neighboringScenes = new HashSet<string>();

    #endregion

    #region Events

    private delegate void InternalLoadedHandler(string sceneName, int distance);

    #endregion

    #region Miscsellaneous Methods

    /// <summary>
    /// Determines whether a scene is loaded.
    /// </summary>
    /// <returns><c>true</c> if loaded; otherwise, <c>false</c>.</returns>
    /// <param name="sceneName">Scene name.</param>
    private bool IsLoaded(string sceneName)
    {
        return loadedScenes.Contains(sceneName);
    }

    /// <summary>
    /// Sets the current scene, loads it, and manages neighbors. The scene must be in your
    /// project's build settings.
    /// </summary>
    /// <param name="sceneName">Scene name.</param>
    public void SetActiveScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName) || string.Equals(sceneName, activeScene))
        {
            return;
        }

        StartCoroutine(LoadActiveScene(sceneName));
    }

    /// <summary>
    /// Loads neighbor scenes within drawDistance, adding them to the near list.
    /// </summary>
    /// <param name="sceneName">Scene name.</param>
    /// <param name="distance">Distance.</param>
    private void LoadNeighbors(string sceneName, int distance)
    {
        if (this.neighboringScenes.Contains(sceneName))
        {
            return;
        }

        this.neighboringScenes.Add(sceneName);

        if (distance >= drawDistance)
        {
            return;
        }

        GameObject scene = GameObject.Find(sceneName);
        NeighboringScenes neighboringScenes = scene ? scene.GetComponent<NeighboringScenes>() : null;

        if (!neighboringScenes)
        {
            neighboringScenes = GetNeighboringScenes(scene);
            return;
        }

        for (int i = 0; i < neighboringScenes.Names.Count; i++)
        {
            Load(neighboringScenes.Names[i], LoadNeighbors, distance + 1);
        }
    }

    /// <summary>
    /// Creates the neighboring scenes list. It's faster to manually add a
    /// NeighboringScenes script to your scene's root object; this method
    /// builds it manually if it's missing, but requires the scene to have
    /// SceneEdge components.
    /// </summary>
    /// <returns> The neighboring scenes list. </returns>
    /// <param name="scene"> Root GameObject of scene. </param>
    private NeighboringScenes GetNeighboringScenes(GameObject scene)
    {
        if (!scene)
        {
            return null;
        }

        NeighboringScenes neighboringScenes = scene.AddComponent<NeighboringScenes>();
        HashSet<string> neighbors = new HashSet<string>();
        SceneEdgeController[] sceneEdges = scene.GetComponentsInChildren<SceneEdgeController>();

        for (int i = 0; i < sceneEdges.Length; i++)
        {
            neighbors.Add(sceneEdges[i].NextScene);
        }

        neighboringScenes.Names = new string[neighbors.Count].ToList();
        neighbors.CopyTo(neighboringScenes.Names.ToArray());

        return neighboringScenes;
    }

    /// <summary>
    /// Loads a scene as the current scene and manages neighbors, loading scenes
    /// within drawDistance and unloading scenes beyond it.
    /// </summary>
    /// <returns>The current scene.</returns>
    /// <param name="sceneName">Scene name.</param>
    private IEnumerator LoadActiveScene(string sceneName)
    {
        activeScene = sceneName;

        if (!IsLoaded(activeScene)) Load(sceneName);

        float failsafeTime = Time.realtimeSinceStartup + loadTime;
        while ((loadingScenes.Count > 0) && (Time.realtimeSinceStartup < failsafeTime))
            yield return null;

        if (Time.realtimeSinceStartup >= failsafeTime && Debug.isDebugBuild) Debug.LogWarning("Scene Streamer: Timed out waiting to load " + sceneName + ".");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

        // Next load neighbors up to drawDistance, keeping track
        // of them in the near list:
        if (logDebugInfo) Debug.Log("Scene Streamer: Loading " + drawDistance + " closest neighbors of " + sceneName + ".");
        neighboringScenes.Clear();
        LoadNeighbors(sceneName, 0);
        failsafeTime = Time.realtimeSinceStartup + loadTime;
        while ((loadingScenes.Count > 0) && (Time.realtimeSinceStartup < failsafeTime))
        {
            yield return null;
        }
        if (Time.realtimeSinceStartup >= failsafeTime && Debug.isDebugBuild) Debug.LogWarning("Scene Streamer: Timed out waiting to load neighbors of " + sceneName + ".");

        // Finally unload any scenes not in the near list:
        UnloadFarScenes();
    }

    /// <summary>
    /// Unloads scenes beyond drawDistance. Assumes the near list has already been populated.
    /// </summary>
    private void UnloadFarScenes()
    {
        HashSet<string> far = new HashSet<string>(loadedScenes);

        far.ExceptWith(neighboringScenes);

        foreach (string sceneName in far)
        {
            Unload(sceneName);
        }
    }

    /// <summary>
    /// Unloads a scene.
    /// </summary>
    /// <param name="sceneName">Scene name.</param>
    public void Unload(string sceneName)
    {
        Destroy(GameObject.Find(sceneName));
        loadedScenes.Remove(sceneName);

        SceneManager.UnloadSceneAsync(sceneName);
    }

    /// <summary>
    /// Loads a scene and calls an internal delegate when done. The delegate is
    /// used by the LoadNeighbors() method.
    /// </summary>
    /// <param name="sceneName">Scene name.</param>
    /// <param name="loadedHandler">Loaded handler.</param>
    /// <param name="distance">Distance from the current scene.</param>
    private void Load(string sceneName, InternalLoadedHandler loadedHandler = null, int distance = 0)
    {
        if (IsLoaded(sceneName))
        {
            loadedHandler?.Invoke(sceneName, distance);
            return;
        }
        loadingScenes.Add(sceneName);
        if (logDebugInfo && distance > 0) Debug.Log("Scene Streamer: Loading " + sceneName + ".");

        StartCoroutine(LoadAdditiveAsync(sceneName, loadedHandler, distance));
    }

    /// <summary>
    /// (Unity Pro) Runs Application.LoadLevelAdditiveAsync() and calls FinishLoad() when done.
    /// </summary>
    /// <param name="sceneName">Scene name.</param>
    /// <param name="loadedHandler">Loaded handler.</param>
    /// <param name="distance">Distance.</param>
    private IEnumerator LoadAdditiveAsync(string sceneName, InternalLoadedHandler loadedHandler, int distance)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        onLoading.Invoke(sceneName, asyncOperation);
        yield return asyncOperation;

        FinishLoad(sceneName, loadedHandler, distance);
    }

    /// <summary>
    /// Called when a level is done loading. Updates the loaded and loading lists, and 
    /// calls the loaded handler.
    /// </summary>
    /// <param name="sceneName">Scene name.</param>
    /// <param name="loadedHandler">Loaded handler.</param>
    /// <param name="distance">Distance.</param>
    private void FinishLoad(string sceneName, InternalLoadedHandler loadedHandler, int distance)
    {
        GameObject scene = GameObject.Find(sceneName);
        if (scene == null && Debug.isDebugBuild) Debug.LogWarning("Scene Streamer: Can't find loaded scene named '" + sceneName + "'.");

        loadingScenes.Remove(sceneName);
        loadedScenes.Add(sceneName);
        onLoaded.Invoke(sceneName);
        if (loadedHandler != null) loadedHandler(sceneName, distance);
    }

    #endregion

    #region Static Methods

    public static void SetActive(string sceneName)
    {
        instance.SetActiveScene(sceneName);
    }

    #endregion
}