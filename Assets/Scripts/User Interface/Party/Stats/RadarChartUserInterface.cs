using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///
/// </summary>
public class RadarChartUserInterface : MonoBehaviour
{
    //#region Constants
    // TODO: Make serializable?

    // private const int margin = 20;

    //#endregion

    #region Variables

    [Header("Setup")]
    [SerializeField] private Material material;
    [SerializeField] private Texture2D texture;

    [Header("Settings")]
    [SerializeField] [Range(0, 500)] private int cutOff = 300;

    private CanvasRenderer canvasRenderer;
    private RectTransform baseSprite; // TODO: Crap name...

    #endregion

    #region Miscellaneous Methods

    public void UpdateUserInterface(List<int> stats)
    {
        float height = baseSprite.sizeDelta.y / 2;
        float angle = -360f / stats.Count;

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[stats.Count + 1];
        Vector2[] uv = new Vector2[stats.Count + 1];
        int[] triangles = new int[3 * stats.Count];

        vertices[0] = Vector3.zero;

        for (int i = 0; i < stats.Count; i++)
        {
            vertices[i + 1] = Quaternion.Euler(0, 0, angle * i) * Vector3.up * height * GetNormalizedStat(stats[i], cutOff);
        }

        int j = 0; // TODO: Pretty crappy name

        for (int i = 1; i < triangles.Length; i++)
        {
            triangles[i] = i % 3 == 0 ? 0 : ExtensionMethods.IncrementInt(j, 1, stats.Count + 1, 1);
            j = triangles[i] == 0 || (i + 1) % 3 == 0 ? j : triangles[i];
        }

        if (texture != null)
        {
            uv[0] = Vector2.zero;
            
            for (int i = 1; i < uv.Length; i++)
            {
                uv[i] = Vector2.one;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        canvasRenderer.SetMesh(mesh);
        canvasRenderer.SetMaterial(material, texture);
    }

    private float GetNormalizedStat(int value, float max)
    {
        return (Mathf.Clamp(value, 0, cutOff) / max);
    }

    #endregion
    
    #region Unity Methods
    
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        canvasRenderer = transform.Find("Mesh").GetComponent<CanvasRenderer>();
        baseSprite = transform.Find("Base").GetComponent<RectTransform>();
    }

    #endregion
}

