using System.Collections;
using UnityEngine;

/// <summary>
///
/// </summary>
public class LightController : MonoBehaviour
{
    #region Variables
    public static LightController instance;
    private SpriteRenderer rend;

    //[UnityEngine.Header("Setup")]
    //[SerializeableField]
    private float minRange = 0.25f, maxRange = 0.75f, speed;

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
        rend = GetComponent<SpriteRenderer>();
        speed = Random.Range(0.3f, 1f);
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        //while (!item.isPickedUp)
        Color startColor = rend.color;
        startColor.a = PingPong(minRange, maxRange, speed);
        rend.color = startColor;
    }

    #endregion

    public float PingPong(float minRange, float maxRange, float speed)
    {
         return Mathf.Lerp(minRange, maxRange, Mathf.PingPong(Time.time, speed));
    }

    public void FadeLight(float targetIntensity, float duration)
    {
        StartCoroutine(rend.gameObject.FadeObject(targetIntensity, duration));
    }
}
