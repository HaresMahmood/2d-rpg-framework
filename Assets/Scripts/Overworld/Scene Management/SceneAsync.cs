using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAsync : MonoBehaviour
{
    public string scene;
    public string playerTag = "Player";
    public bool playerInRange;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        }
    }
}
