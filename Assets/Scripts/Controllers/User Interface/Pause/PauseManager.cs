using UnityEngine;

/// <summary>
///
/// </summary>
public class PauseManager : MonoBehaviour
{
    #region Constants

    public readonly string[] menuNames = new string[] { "Missions", "Party", "Inventory", "System" };

    #endregion

    #region Variables

    public static PauseManager instance;

    [Header("Setup")]
    [SerializeField] private PauseUserInterface userInterface;

    private readonly TestInput input = new TestInput();
    public Flags flags = new Flags(false, false, false, false);

    public int selectedSlot { get; private set; }
    private int selectedMenu;

    //public event EventHandler OnUserInput = delegate { };

    #endregion

    #region Structs

    public struct Flags
    {
        public bool isActive { get; set; }
        public bool isInPartyMenu { get; set; }
        public bool isUsingItem { get; set; }
        public bool isGivingItem { get; set; }

        public Flags(bool isActive, bool isInParty, bool isUsingItem, bool isGivingItem)
        {
            this.isActive = isActive;
            this.isInPartyMenu = isInParty;
            this.isUsingItem = isUsingItem;
            this.isGivingItem = isGivingItem;
        }
    }

    #endregion

    #region Behavior Definitions (Inventory) // TODO: Weird name.

    private void Use(Item item)
    {
        //item.amount--;
        //PartyManager.instance.party.playerParty[selectedSlot].stats.health = (int)PartyManager.instance.party.playerParty[selectedSlot].totalHealth;
        //userInterface.PopulateSideBar(PartyManager.instance.party);
    }

    private void Give(Item item)
    {
        //item.amount--;
        //PartyManager.instance.party.playerParty[selectedSlot].heldItem = item;
        //userInterface.PopulateSideBar(PartyManager.instance.party);
    }

    private void ApplyItemBehavior(Item item)
    {
        if (flags.isUsingItem)
        {
            Use(item);
        }
        else if (flags.isGivingItem)
        {
            Give(item);
        }
    }

    #endregion

    #region Miscellaneous Methods

    private void OnActive()
    {
        float zoom = flags.isActive ? 5.8f : CameraController.instance.startSize;

        Time.timeScale = flags.isActive ? 0 : 1;

        CameraController.instance.GetComponent<PostprocessingBlur>().enabled = flags.isActive;
        CameraController.instance.ZoomCamera(zoom, CameraController.instance.moveSpeed);

        userInterface.TogglePauseMenu(flags.isActive);

        if (flags.isActive)
        {
            UpdateMenus(2, -1, 0.1f, false);
            //userInterface.PopulateSideBar(PartyManager.instance.party);

            TimeManager.instance.UpdateTimeUserInterface();
            WeatherManager.instance.UpdateWeahterUserInterface();
        }
        else
        {
            if (flags.isInPartyMenu)
            {
                flags.isInPartyMenu = false;
                userInterface.UpdateSidePanel(selectedSlot, 0, 0.15f);
                selectedSlot = 0;
            }

            TimeManager.instance.SetPause(false);
        }

        //InventoryController.instance.Flags.isActive = flags.isActive;
    }

    public void InitializeSidePanel()
    {
        selectedSlot = 0;
        userInterface.UpdateSidePanel(0, -1, 0.15f);
        // yield return null; 
        flags.isInPartyMenu = true;
    }

    private void DeactivateSidePanel()
    {
        flags.isInPartyMenu = false;
        userInterface.UpdateSidePanel(selectedSlot, 0, 0.15f);
        selectedSlot = 0;
        //if (InventoryController.instance.Flags.isItemSelected)
        //{
        //    InventoryController.instance.CloseSelectionMenu();
        //}
        //StartCoroutine(InventoryController.instance.DeactivateSidePanel(0.2f));
    }

    private void UpdateMenus(int selectedMenu, int increment, float animationDuration, bool animate = true)
    {
        userInterface.UpdateMenus(selectedMenu, increment, animationDuration, animate);
    }

    private void AnimateBackground(float distance, int increment)
    {
        float direction = increment * distance;
        Vector2 targetPosition = new Vector2(CameraController.instance.GetPosition().x + direction, CameraController.instance.GetPosition().y);
        StartCoroutine(CameraController.instance.LerpCamera(targetPosition, 0.2f));
    }

    private void GetInput()
    {
        if (!flags.isInPartyMenu)
        {
            bool hasInput;
            (selectedMenu, hasInput) = input.GetInput("Face Trigger", TestInput.Axis.Horizontal, menuNames.Length, selectedMenu);
            if (hasInput)
            {
                UpdateMenus(selectedMenu, -(int)Input.GetAxisRaw("Face Trigger"), 0.15f);
                AnimateBackground(0.2f, -(int)Input.GetAxisRaw("Face Trigger"));
            }
        }
        else
        {
            bool hasInput = false;
            //(selectedSlot, hasInput) = input.GetInput("Vertical", TestInput.Axis.Vertical, (PartyManager.instance.party.playerParty.Count + 1), selectedSlot);
            if (hasInput)
            {
                userInterface.UpdateSidePanel(selectedSlot, (int)Input.GetAxisRaw("Vertical"), 0.1f);
            }

            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                DeactivateSidePanel();
            }

            if (Input.GetButtonDown("Interact"))
            {
                //ApplyItemBehavior(InventoryController.instance.selectedItem);
                //InventoryController.instance.UpdateItem();
                DeactivateSidePanel();
            }
        }

        if (Input.GetButtonDown("Start"))
        {
            //flags.isActive = !flags.isActive;
            flags.isActive = !userInterface.gameObject.activeSelf;
            OnActive();
        }
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
        selectedMenu = 2;
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        if (!DialogManager.instance.isActive)
        {
            GetInput();
        }
    }

    #endregion
}