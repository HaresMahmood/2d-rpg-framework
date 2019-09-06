using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

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
        //Debug.Log(activeScene.name);
        //Debug.Log(nextScene.name);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GameManager.instance.playerTag))
        {
            Debug.Log("ENTERED SCENE EDGE");
            bool hasLoaded = false;

            if (!hasLoaded)
            {
                SceneManager.LoadSceneAsync("SampleScene2", LoadSceneMode.Additive);
                SceneManager.sceneLoaded += OnSceneLoadedWrapper;
                
                hasLoaded = true;
            }
        }
    }

    void OnSceneLoadedWrapper(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name);
        StartCoroutine("OnSceneLoaded");
    }

    IEnumerator OnSceneLoaded()
    {
        yield return new WaitForEndOfFrame();

        GetTileMaps(SceneManager.GetSceneByName("SampleScene2").GetRootGameObjects());
        //Debug.Log(SceneManager.GetSceneByBuildIndex(2).name);
        //Debug.Log(SceneManager.GetSceneByBuildIndex(2).GetRootGameObjects().Length);
    }

    void GetTileMaps(GameObject[] objList)
    {
        Tilemap[] ground;
        Tilemap[] obstacles;

        Debug.Log(objList.Length);

        foreach (GameObject gObject in objList)
        {
            if (gObject.name == "Grid")
            {
                Transform[] grid = GetFirstChildren(gObject.transform);

                foreach (Transform gridObject in grid)
                {
                    if (gridObject.name == "Ground")
                    {
                        ground = TilemapManager.GetFirstChildren(gridObject.transform);
                        TilemapManager.groundTiles.AddRange(ground);
                    }
                    else if (gridObject.name == "Obstacles")
                    {
                        obstacles = TilemapManager.GetFirstChildren(gridObject.transform);
                        TilemapManager.obstacleTiles.AddRange(obstacles);
                    }
                }
            }
        }
    }

    public Transform[] GetFirstChildren(Transform parent)
    {
        Transform[] children = parent.GetComponentsInChildren<Transform>();
        Transform[] firstChildren = new Transform[parent.childCount];
        int index = 0;
        foreach (Transform child in children)
        {
            if (child.parent == parent)
            {
                firstChildren[index] = child;
                index++;
            }
        }
        return firstChildren;
    }
}
