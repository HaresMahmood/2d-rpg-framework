using UnityEngine;

/// <summary>
///
/// </summary>
public class PauseManager : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// Determines whether the game is paused or not.
    /// </summary>
    public bool isPaused; // Made public for debug purposes.

    [SerializeField] private GameObject pausePanel;

    #endregion

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        isPaused = false;
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        TogglePause();
    }

    #endregion

    public void TogglePause()
    {
        if (Input.GetButtonDown("Start"))
            isPaused = !isPaused;

        if (isPaused)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
