using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    //[UnityEngine.Header("Static data")]
    public static List<Tilemap> groundTiles = new List<Tilemap>();
    public static List<Tilemap> obstacleTiles = new List<Tilemap>();

    public static void GetTilemaps(GameObject[] objList)
    {
        Tilemap[] ground;
        Tilemap[] obstacles;

        //Debug.Log(objList.Length);

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
