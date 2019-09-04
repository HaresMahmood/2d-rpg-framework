using UnityEngine;
using System.Collections;

public class GameObjectEvents : MonoBehaviour
{

    public static event System.Action<GameObject> notifyAwake;
    public static event System.Action<GameObject> notifyDeath;

    // Use this for initialization
    void Start()
    {
        //Notify if any listeners are present about its awake status
        if (notifyAwake != null)
            notifyAwake(gameObject);
    }

    void OnDestroy()
    {
        // tell any listeners if present that this gameobject is dying
        if (notifyDeath != null)
            notifyDeath(gameObject);
    }

}