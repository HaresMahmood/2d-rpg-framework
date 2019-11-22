using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// </summary>
public class SystemManager : MonoBehaviour
{
    #region Variables

    public static SystemManager instance;


    [Header("Values")]
    public ViewingMode viewingMode; //{ get; private set; }
    public bool isActive { get; set; } = false;

    [HideInInspector]
    public GameObject systemContainer
    {
        get;
        private set;
    }

    [HideInInspector]
    public string[] highlevelText { get; private set; } = new string[] { "Save", "Settings", "Tutorials", "Controls", "Quit" };
    [HideInInspector] 
    public string[] settingsText { get; private set; } = new string[] { "General", "Battle", "Customization", "Accessability", "Test" };

    public enum ViewingMode
    {
        Basic,
        Intermediate,
        Advanced
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
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
        systemContainer = PauseManager.instance.pauseContainer.transform.Find("System").gameObject;
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {

    }

    #endregion
}
