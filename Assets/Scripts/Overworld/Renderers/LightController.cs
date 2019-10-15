using System.Collections;
using UnityEngine;

/// <summary>
///
/// </summary>
public class LightController : MonoBehaviour
{
    #region Variables
    public static LightController instance;
    private Light lightSource;
    private Item item;
    private ItemInteraction interaction;

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
        lightSource = GetComponent<Light>();
        interaction = GetComponentInParent<ItemInteraction>();
        item = interaction.item;
        speed = Random.Range(0.3f, 1f);
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        //while (!item.isPickedUp)
            lightSource.intensity = PingPong(minRange, maxRange, speed);
    }

    #endregion

    public float PingPong(float minRange, float maxRange, float speed)
    {
         return Mathf.Lerp(minRange, maxRange, Mathf.PingPong(Time.time, speed));
    }

    public IEnumerator FadeLight(float targetIntensity, float duration)
    {
        float startIntensity = lightSource.intensity;

        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            float blend = Mathf.Clamp01(t / duration);
            float newIntensity = Mathf.Lerp(startIntensity, targetIntensity, blend);

            lightSource.intensity = newIntensity;

            yield return null;
        }
    }
}
