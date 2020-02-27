using UnityEngine;
using TMPro;

/// <summary>
///
/// </summary>
public class QuantitySelectorUserInterface : UserInterface
{
    #region Constants

    public override int MaxObjects => SelectedItem.Quantity;

    #endregion

    #region Properties

    public int Value { get; private set; }

    public Item SelectedItem { get; private set; }

    #endregion

    #region Variables

    private TextMeshProUGUI quantityText;

    #endregion

    #region Miscellaneous Methods

    public void ToggleSelector(bool isActive, Item item, float xCoordinate)
    {
        float opacity = isActive ? 1f : 0f;

        FadeSelector(opacity);

        if (xCoordinate > -1)
        {
            MoveSelector(xCoordinate);
        }

        if (item != null)
        {
            SelectedItem = item;
            Value = 1;
            quantityText.SetText("1");
        }

        if (!isActive && SelectedItem != null)
        {
            SelectedItem.Quantity -= (Value > 1 ? (Value - 1) : Value);
        }
    }

    public override void UpdateSelectedObject(int selectedValue, int increment = -1)
    {
        Value = selectedValue;
        quantityText.SetText(selectedValue.ToString());
    }

    public void FadeSelector(float opacity, float animationDuration = 0.1f)
    {
        StartCoroutine(gameObject.FadeOpacity(opacity, animationDuration));
    }

    private void MoveSelector(float xCoordinate, float distance = 30f, float animationDuration = 0.1f)
    {
        StartCoroutine(transform.LerpPosition(new Vector2(xCoordinate + distance, transform.position.y), animationDuration));
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        quantityText = transform.Find("Value/Value").GetComponent<TextMeshProUGUI>();

        //base.Awake();
    }

    #endregion
}
