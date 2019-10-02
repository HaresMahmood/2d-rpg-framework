using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

/// <summary>
///
/// </summary>
public class LightModulator : MonoBehaviour
{
    #region Variables

    private Light2D lightSource;

    //[UnityEngine.Header("Setup")]
    //[SerializeableField]
    private float minRange = 0.10f, maxRange = 0.40f, speed;
   
    #endregion

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        lightSource = GetComponent<Light2D>();
        speed = Random.Range(0.3f, 1f);
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        lightSource.intensity = Mathf.Lerp(minRange, maxRange, Mathf.PingPong(Time.time, speed));
    }

    #endregion
}
