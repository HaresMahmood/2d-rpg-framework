using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    [UnityEngine.Header("Static data")]
    public static List<Tilemap> groundTiles = new List<Tilemap>();
    public static List<Tilemap> obstacleTiles = new List<Tilemap>();

    private void Start()
    {
        groundTiles.Clear();
        obstacleTiles.Clear();

        GetTileMaps();
    }

    public static void GetTileMaps()
    {
        Tilemap[] ground = GetFirstChildren(GameObject.Find("Grid").transform.Find("Ground").transform);
        Tilemap[] obstacles = GetFirstChildren(GameObject.Find("Grid").transform.Find("Objects").transform);


        //Debug.Log(ground.Length);
        //Debug.Log(obstacles.Length);

        groundTiles.AddRange(ground);
        obstacleTiles.AddRange(obstacles);
    }


    public static Tilemap[] GetFirstChildren(Transform parent)
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
