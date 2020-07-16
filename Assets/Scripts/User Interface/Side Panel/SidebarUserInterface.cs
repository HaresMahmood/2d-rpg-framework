using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class SidebarUserInterface : UserInterface
{
    #region Constants

    public override int MaxObjects => (Party.Count + 1);

    #endregion

    #region Properties

    public List<PartyMember> Party { get; set; }

    #endregion

    #region Variables

    private EditUserInterface editUserInterface;

    private List<SidebarSlot> slots;

    private Animator animator;
    private Transform editButton;

    #endregion

    #region Miscellaneous Methods

    public override void UpdateSelectedObject(int selectedValue, int increment)
    { 
        Transform selectedObject = null;
        int previousValue = ExtensionMethods.IncrementInt(selectedValue, 0, MaxObjects, increment);

        if (selectedValue != (MaxObjects - 1))
        {
            slots[selectedValue].AnimateSlot(true);

            selectedObject = slots[selectedValue].transform;
        }
        else
        {
            selectedObject = editButton;
        }

        if (previousValue != (MaxObjects - 1))
        {
            slots[previousValue].AnimateSlot(false);
        }

        StartCoroutine(UpdateSelector(increment == 0 ? null : selectedObject));
    }

    public void FadePanel(float opacity, float animationDuration = 0.15f)
    {
        StartCoroutine(gameObject.FadeOpacity(opacity, animationDuration));
    }

    public bool ActivateMenu(bool isActive, int selectedValue = -1)
    {
        if (selectedValue == (MaxObjects - 1) || selectedValue == -1)
        {
            animator.SetBool("isActive", isActive);
            StartCoroutine(EditUserInterfaceController.Instance.SetActive(isActive));
            PauseUserInterfaceController.Instance.AnimateMenu(isActive ? 0f : 1f);

            if (!isActive)
            {
                UpdateInformation(false);
            }

            return !isActive;
        }

        return true;
    }

    /// <summary>
    /// Animates and updates the position of the selector. Dynamically changes position and size of selector 
    /// depending on what situation it is used for. If no value is selected, the indicator completely fades out.
    /// </summary>
    /// <param name="objectSlots"> List of buttons at with the selector can be positioned. </param>
    /// <param name="selectedValue"> Index of the value currently selected. </param>
    /// <param name="animationDuration"> Duration of the animation/fade. </param>
    /// <returns> Co-routine. </returns>
    protected override IEnumerator UpdateSelector(Transform selectedObject = null, float animationDuration = 0.1f)
    {
        yield return null;

        StartCoroutine(base.UpdateSelector(selectedObject, animationDuration));

        if (selectedObject != null)
        {
            yield return new WaitForSecondsRealtime(animationDuration);

            selector.transform.Find(selectedObject.GetComponent<SidebarSlot>() == null ? "Edit" : "Party").gameObject.SetActive(true);
            selector.transform.Find(selectedObject.GetComponent<SidebarSlot>() == null ? "Party" : "Edit").gameObject.SetActive(false);
        }

    }

    private void UpdateInformation(bool isActive = true) // TODO: Bad name
    {
        int counter = 0;

        for (int i = 0; i < Party.Count; i++)
        {
            slots[i].UpdateInformation(Party[i]);
            counter = i;
        }

        if (isActive)
        {
            if (++counter < slots.Count)
            {
                for (int i = counter; i < slots.Count; i++)
                {
                    slots[i].DeactivateSlot();
                }
            }
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        editUserInterface = transform.Find("Full Party").GetComponent<EditUserInterface>();

        animator = GetComponent<Animator>();
        selector = transform.Find("Selectors").gameObject;
        editButton = transform.Find("Edit");

        slots = GetComponentsInChildren<SidebarSlot>().ToList();

        base.Awake();
    }

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        UpdateInformation();

        editUserInterface.UpdateInformation(Party);
        editUserInterface.gameObject.SetActive(false);

        editUserInterface.Party = Party;

        StartCoroutine(UpdateSelector());
    }

    #endregion
}

