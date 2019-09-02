using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private AsyncOperation sceneAsync;
    public string sceneToLoad;
    public GameObject persistentObjects;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GameManager.instance.playerTag))
            StartCoroutine(LoadScene(sceneToLoad));
    }

    IEnumerator LoadScene(string sceneToLoad)
    {
        AsyncOperation scene = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        scene.allowSceneActivation = false;
        sceneAsync = scene;

        //Wait until we are done loading the scene
        while (scene.progress < 0.9f)
        {
            Debug.Log("Loading scene " + " [][] Progress: " + scene.progress);
            yield return null;
        }
        OnFinishedLoadingScenes();
    }

    void EnableScene(string loadScene)
    {
        //Activate the Scene
        sceneAsync.allowSceneActivation = true;


        Scene sceneToLoad = SceneManager.GetSceneByName(loadScene);
        if (sceneToLoad.IsValid())
        {
            Debug.Log("Scene is Valid");
            SceneManager.MoveGameObjectToScene(persistentObjects, sceneToLoad);
            SceneManager.SetActiveScene(sceneToLoad);
        }
    }

    void OnFinishedLoadingScenes()
    {
        Debug.Log("Done Loading Scene");
        EnableScene(sceneToLoad);
        TilemapManager.GetTileMaps();
        Debug.Log("Scene Activated!");
    }
}
