using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    [UnityEngine.Header("General Settings")]
    public Transform player;

    [UnityEngine.Header("Global Settings")]
    public string playerTag = "Player";
}
