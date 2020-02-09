using UnityEngine;

public class LoadManager : MonoBehaviour
{
    private bool objectExists;

    // Start is called before the first frame update
    void Start()
    {
        if (!objectExists)
        {
            objectExists = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            objectExists = false;
            Destroy(gameObject);
        }
    }
}
