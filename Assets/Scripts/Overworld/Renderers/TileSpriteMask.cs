using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
///
/// </summary>
public class TileSpriteMask : Tile
{
    #region Variables



    #endregion

    #region Unity Methods

    public override bool StartUp(Vector3Int location, ITilemap tilemap, GameObject go)
    {
        if (go != null)
        {
            SpriteMask mask = go.AddComponent<SpriteMask>();
            mask.sprite = this.sprite;
        }
        return true;
    }

    #endregion
}
