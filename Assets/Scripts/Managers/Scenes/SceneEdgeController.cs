using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneEdgeController : MonoBehaviour
{
    public static Scene activeScene;
    public static Scene nextScene;
    public string nextSceneName;

    // Start is called before the first frame update
    void Start()
    {
        activeScene = SceneManager.GetSceneByName("SampleScene");
        nextScene = SceneManager.GetSceneByName(nextSceneName);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(activeScene.name);
        Debug.Log(nextScene.name);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GameManager.instance.playerTag))
        {
            Debug.Log("ENTERED SCENE EDGE");
            bool hasLoaded = false;

            if (!hasLoaded)
            {
                SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
                TilemapManager.GetTileMaps(SceneManager.GetSceneByBuildIndex(2));
                hasLoaded = true;
            }
        }
    }
}
