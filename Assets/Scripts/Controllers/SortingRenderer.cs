using UnityEngine;

public class SortingRenderer : MonoBehaviour
{
    public int baseSortingOrder = 500;
    public int offset = -1;
    public bool runOnce = false;

    private float timer;
    private float timerMax = 0.1f;
    private Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = timerMax;
            rend.sortingOrder = (int)(baseSortingOrder - transform.position.y - offset);
            if (runOnce)
            {
                Destroy(this);
            }
        }
    }
}