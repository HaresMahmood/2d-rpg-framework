using System.Collections;
using UnityEngine;

/// <summary>
///
/// </summary>
public class OverworldItemController : MonoBehaviour
{
    #region Variables
    public static OverworldItemController instance;

    private SpriteRenderer image;
    private new Light light;
    private float speed;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        image = GetComponentInChildren<SpriteRenderer>();
        light = GetComponentInChildren<Light>();
        speed = Random.Range(0.3f, 1f);
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        Color color = image.color;
        color.a = PingPong(0.25f, 0.75f, speed);
        image.color = color;

        light.intensity = PingPong(3.5f, 5.5f, speed);
    }

    #endregion

    public float PingPong(float minRange, float maxRange, float speed)
    {
         return Mathf.Lerp(minRange, maxRange, Mathf.PingPong(Time.time, speed));
    }

    public void FadeItem(float targetOpacity, float duration)
    {
        StartCoroutine(image.gameObject.FadeObject(targetOpacity, duration));
        StartCoroutine(light.FadeLight(targetOpacity, duration));
    }
}