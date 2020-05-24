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

    [SerializeField] private Material material;

    private CanvasRenderer canvasRenderer;
    private RectTransform baseSprite; // TODO: Crap name...

    #endregion

    #region Miscellaneous Methods

    public void UpdateUserInterface(float hp, float attack, float defence, float spAttack, float spDefence, float speed) // TODO: Use list/dict
    {
        float height = baseSprite.sizeDelta.y / 2;
        float angle = -360f / 6;

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[7];
        Vector2[] uv = new Vector2[7];
        int[] triangles = new int[3 * 6];

        Vector3 hpVertex = (Quaternion.Euler(0, 0, angle * 0) * Vector3.up * height * (Mathf.Clamp(hp, 0, 300) / 300f));
        Vector3 attackVertex = (Quaternion.Euler(0, 0, angle * 1) * Vector3.up * height * (Mathf.Clamp(attack, 0, 300) / 300f));
        Vector3 defenceVertex = (Quaternion.Euler(0, 0, angle * 2) * Vector3.up * height * (Mathf.Clamp(defence, 0, 300) / 300f));
        Vector3 spAttackVertex = (Quaternion.Euler(0, 0, angle * 3) * Vector3.up * height * (Mathf.Clamp(spAttack, 0, 300) / 300f));
        Vector3 spDefenceVertex = (Quaternion.Euler(0, 0, angle * 4) * Vector3.up * height * (Mathf.Clamp(spDefence, 0, 300) / 300f));
        Vector3 speedVertex = (Quaternion.Euler(0, 0, angle * 5) * Vector3.up * height * (Mathf.Clamp(speed, 0, 300) / 300f));


        vertices[0] = Vector3.zero;
        vertices[1] = hpVertex;
        vertices[2] = attackVertex;
        vertices[3] = defenceVertex;
        vertices[4] = spAttackVertex;
        vertices[5] = spDefenceVertex;
        vertices[6] = speedVertex;

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        triangles[3] = 0;
        triangles[4] = 2;
        triangles[5] = 3;

        triangles[6] = 0;
        triangles[7] = 3;
        triangles[8] = 4;

        triangles[9] = 0;
        triangles[10] = 4;
        triangles[11] = 5;

        triangles[12] = 0;
        triangles[13] = 5;
        triangles[14] = 6;

        triangles[15] = 0;
        triangles[16] = 6;
        triangles[17] = 1;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        canvasRenderer.SetMesh(mesh);
        canvasRenderer.SetMaterial(material, null);
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

