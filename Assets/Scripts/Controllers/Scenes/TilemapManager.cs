using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    public static TilemapManager instance;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    /// <summary>
    /// Adds Tilemaps from scene, which should be  pre-organized
    /// in seperate GameObjects, to Lists.
    /// </summary>
    /// <param name="sceneObjects"> All GameObjects found in the scene. </param>
    /// <param name="groundTiles"> List containing ground Tilemaps. </param>
    /// <param name="obstacleTiles"> List containing obstacle Tilemaps </param>
    public void GetTilemaps(GameObject[] sceneObjects, List<Tilemap> obstacleTiles)
    {
        Transform[] ground, obstacles;

        foreach (GameObject sceneObject in sceneObjects)
        {
            Transform[] sceneRootObjects = sceneObject.transform.GetChildren();

            foreach (Transform rootObject in sceneRootObjects)
            {
                Transform[] gridObjects = rootObject.GetChildren();
                foreach (Transform gridObject in gridObjects)
                {
                    if (gridObject.name == "Obstacles")
                    {
                        obstacles = gridObject.transform.GetChildren();
                        foreach (Transform tileMap in obstacles)
                            obstacleTiles.Add(tileMap.GetComponent<Tilemap>());
                    }
                }
            }
        }
    }

    public void RemoveTilemaps(List<Tilemap> obstacleTiles)
    {
        //groundTiles.ClearNullReferences();
        //obstacleTiles.ClearNullReferences();

        for (int i = obstacleTiles.Count - 1; i > -1; i--)
        {
            if (obstacleTiles[i] == null)
                obstacleTiles.RemoveAt(i);
        }

    }
}
