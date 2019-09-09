using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    public static TilemapManager instance;

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    //[UnityEngine.Header("Static data")]
    //public static List<Tilemap> groundTiles = new List<Tilemap>();
    //public static List<Tilemap> obstacleTiles = new List<Tilemap>();

    public void GetTilemaps(GameObject[] objList, List<Tilemap> groundTiles, List<Tilemap> obstacleTiles)
    {
        Tilemap[] ground;
        Tilemap[] obstacles;

        foreach (GameObject gObject in objList)
        {
            Transform[] rootObjects = GetFirstChildren(gObject.transform);

            foreach (Transform rootObject in rootObjects)
            {
                if (rootObject)
                {
                    Transform[] grid = GetFirstChildren(rootObject);

                    foreach (Transform gridObject in grid)
                    {
                        if (gridObject.name == "Ground")
                        {
                            ground = GetFirstTilemapChildren(gridObject.transform);
                            groundTiles.AddRange(ground);
                        }
                        else if (gridObject.name == "Obstacles")
                        {
                            obstacles = GetFirstTilemapChildren(gridObject.transform);
                            obstacleTiles.AddRange(obstacles);
                        }
                    }
                }
            }
        }
    }

    public void RemoveTilemaps(List<Tilemap> groundTiles, List<Tilemap> obstacleTiles)
    {
        Debug.Log("TilemapManager: Tilemap removal triggered.");

        //groundTiles.ClearNullReferences();
        //obstacleTiles.ClearNullReferences();

        
        //groundTiles.RemoveAll(GameObject => GameObject == null);
        //obstacleTiles.RemoveAll(GameObject => GameObject == null);

        for (var i = groundTiles.Count - 1; i > -1; i--)
        {
            if (groundTiles[i] == null)
                groundTiles.RemoveAt(i);
        }

        for (var i = obstacleTiles.Count - 1; i > -1; i--)
        {
            if (obstacleTiles[i] == null)
                obstacleTiles.RemoveAt(i);
        }
    }

    public static Transform[] GetFirstChildren(Transform parent)
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

    public static Tilemap[] GetFirstTilemapChildren(Transform parent)
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
