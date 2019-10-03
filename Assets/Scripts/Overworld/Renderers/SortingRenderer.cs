using UnityEngine;

public class SortingRenderer : MonoBehaviour
{
    #region Variables

    private Renderer rend;
    private int baseSortingOrder = 500, offset = -1;
    private float timer, maxTimer = 0.1f;
    [SerializeField] public bool runOnce;

    #endregion

    #region Unity Methods

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    /// <summary>
    /// 
    /// </summary>
    private void LateUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = maxTimer;
            rend.sortingOrder = (int)(baseSortingOrder - transform.position.y - offset);
            if (runOnce)
                Destroy(this);
        }
    }

    #endregion
}