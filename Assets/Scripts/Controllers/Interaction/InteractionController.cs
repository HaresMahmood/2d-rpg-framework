using System;
using UnityEngine;

/// <summary>
///
/// </summary>
public class InteractionController : MonoBehaviour
{
    #region Properties

    public ControllerFlags Flags { get; } = new ControllerFlags(true);

    #endregion

    #region Variables

    [Header("Settings")]
    [SerializeField] private LayerMask interactionLayer;

    private new BoxCollider2D collider;

    private Collider2D other;

    #endregion

    #region Events

    public event EventHandler OnInteract;

    #endregion

    #region Nested Classes

    public class ControllerFlags
    {
        public bool isActive { get; internal set; }

        internal ControllerFlags(bool isActive)
        {
            this.isActive = isActive;
        }
    }

    #endregion

    #region Miscellaneous Methods

    private void GetInput()
    {
        if (Input.GetButtonDown("Interact"))
        {
           other = Physics2D.OverlapCircle(collider.transform.position + new Vector3(collider.offset.x, collider.offset.y), 0.1f, interactionLayer);

            if (other)
            {
                Flags.isActive = false;
                other.GetComponentInParent<CharacterInteractionController>().Interact(GetComponent<PlayerMovement>().Orienation);
                DialogController.Instance.SetActive(true, DialogController.Instance.dialog.Data[0].LanguageData);
                OnInteract?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    #endregion

    #region Event Methods

    private void DialogController_OnDialogEnd(object sender, System.EventArgs e)
    {
        OnInteract?.Invoke(this, EventArgs.Empty);
        other.GetComponentInParent<CharacterInteractionController>().Interact(GetComponent<PlayerMovement>().Orienation);
        Flags.isActive = true;
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        collider = transform.Find("Interaction Collider").GetComponent<BoxCollider2D>();
    }

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        DialogController.Instance.OnDialogEnd += DialogController_OnDialogEnd;
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        if (Flags.isActive)
        {
            GetInput();
        }
    }

    #endregion
}

