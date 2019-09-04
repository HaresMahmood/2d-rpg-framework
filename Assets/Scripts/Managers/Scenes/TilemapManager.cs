using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    //[UnityEngine.Header("Static data")]
    public static List<Tilemap> groundTiles = new List<Tilemap>();
    public static List<Tilemap> obstacleTiles = new List<Tilemap>();

    private bool hasLoaded;

    public void Start()
    {
        /*
        Debug.Log(SceneManager.GetActiveScene().name);
        if (!hasLoaded)
            GetTileMaps(SceneManager.GetActiveScene());
        */
    }

    public void Update()
    {
        Debug.Log(SceneManager.GetActiveScene().name);

        if (!hasLoaded)
            GetTileMaps(SceneManager.GetActiveScene());

        /*
        //Debug.Log(SceneStreamer.IsSceneLoaded(SceneManager.GetActiveScene().name));

        if (!hasLoaded)
        {
            if (SceneStreamer.IsSceneLoaded(SceneManager.GetActiveScene().name))
            {
                GetTileMaps(SceneManager.GetActiveScene());
                hasLoaded = true;
            }
        }

        if (!SceneStreamer.IsSceneLoaded(SceneManager.GetActiveScene().name))
            hasLoaded = false;
            */
    }

    public void GetTileMaps(Scene scene)
    {
        Tilemap[] ground;
        Tilemap[] obstacles;

        List<GameObject> objList = GameObjectEventsHandler.specificSceneObjects[scene.name];
        Debug.Log(objList.Count);

        foreach (GameObject gObject in objList)
        {
            if (gObject.name == "Ground")
            {
                ground = GetFirstChildren(gObject.transform);
                groundTiles.AddRange(ground);
            }
            else if (gObject.name == "Obstacles")
            {
                obstacles = GetFirstChildren(gObject.transform);
                obstacleTiles.AddRange(obstacles);
            }
        }

        hasLoaded = true;
    }

    public Tilemap[] GetFirstChildren(Transform parent)
    {
        Transform[] children = parent.GetComponentsInChildren<Transform>();
        Tilemap[] firstChildren = new Tilemap[parent.childCount];
        int index = 0;
        foreach (Transform child in children)
        {
            if (child.parent == parent)
            {
                firstChildren[index] = child.GetComponent<Tilemap>();
                index++;
            }
        }
        return firstChildren;
    }
}
