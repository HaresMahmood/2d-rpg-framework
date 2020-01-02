using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class MoveSlot : MonoBehaviour
{
    #region Variables

    Image panel;

    Transform information;
    Transform statistics;
    TextMeshProUGUI description;

    TextMeshProUGUI moveNameText;
    TextMeshProUGUI ppText;

    GameObject physicalIcon;
    GameObject specialIcon;
    TextMeshProUGUI accuracyText;
    TextMeshProUGUI powertext;

    #endregion

    #region Miscellaneous Methods

    public void UpdateInformation(Pokemon.LearnedMove move)
    {
        moveNameText.SetText(move.move.name);
        /*
        info.Find("Typing/Typing").GetComponent<TextMeshProUGUI>().SetText(learnedMove.move.type.ToString());
        info.Find("Typing/Typing").GetComponent<TextMeshProUGUI>().color = learnedMove.move.UIColor;
        */
        ppText.SetText(move.remainingPp.ToString() + "/" + move.move.pp);

        physicalIcon.SetActive(move.move.category == Move.Category.Physical);
        specialIcon.SetActive(move.move.category == Move.Category.Special);
        accuracyText.SetText(move.move.accuracy.ToString());
        powertext.SetText(move.move.power.ToString());

        description.SetText(move.move.description);

        panel.color = move.move.UIColor;
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        panel = transform.GetComponent<Image>();

        information = transform.Find("Information");
        statistics = transform.Find("Stats");
        description = transform.Find("Description/Description").GetComponent<TextMeshProUGUI>();

        moveNameText = information.Find("Name").GetComponent<TextMeshProUGUI>();
        ppText = information.Find("PP/PP").GetComponent<TextMeshProUGUI>();

        physicalIcon = statistics.Find("Category/Category/Physical").gameObject;
        specialIcon = statistics.Find("Category/Category/Special").gameObject;
        accuracyText = statistics.Find("Accuracy/Accuracy").GetComponent<TextMeshProUGUI>();
        powertext = statistics.Find("Power/Power").GetComponent<TextMeshProUGUI>();
    }

    #endregion
}
