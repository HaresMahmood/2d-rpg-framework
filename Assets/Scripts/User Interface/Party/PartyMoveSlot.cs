using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class PartyMoveSlot : Slot
{
    #region Variables

    Image panel;

    Transform information;
    Transform statistics;
    TextMeshProUGUI descriptionText;

    TextMeshProUGUI moveNameText;
    TextMeshProUGUI ppText;

    GameObject physicalIcon;
    GameObject specialIcon;
    TextMeshProUGUI accuracyText;
    TextMeshProUGUI powertext;

    #endregion

    #region Miscellaneous Methods

    protected override void SetInformation(ScriptableObject slotObject)
    {
        /*
        Pokemon.LearnedMove move = (Pokemon.LearnedMove)slotObject;

        moveNameText.SetText(move.move.name);
        
        //info.Find("Typing/Typing").GetComponent<TextMeshProUGUI>().SetText(learnedMove.move.type.ToString());
        //info.Find("Typing/Typing").GetComponent<TextMeshProUGUI>().color = learnedMove.move.UIColor;
   
        ppText.SetText(move.remainingPp.ToString() + "/" + move.move.pp);

        physicalIcon.SetActive(move.move.category == Move.Category.Physical);
        specialIcon.SetActive(move.move.category == Move.Category.Special);
        accuracyText.SetText(move.move.accuracy.ToString());
        powertext.SetText(move.move.power.ToString());

        descriptionText.SetText(move.move.description);

        panel.color = move.move.UIColor;
        */ 
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        /*
        panel = transform.GetComponent<Image>();

        information = transform.Find("Information");
        statistics = transform.Find("Stats");
        descriptionText = transform.Find("Description/Value").GetComponent<TextMeshProUGUI>();

        moveNameText = information.Find("Name/Value").GetComponent<TextMeshProUGUI>();
        ppText = information.Find("Statistics/PP/Value").GetComponent<TextMeshProUGUI>();

        physicalIcon = statistics.Find("Category/Value/Physical").gameObject;
        specialIcon = statistics.Find("Category/Value/Special").gameObject;
        accuracyText = statistics.Find("Accuracy/Value").GetComponent<TextMeshProUGUI>();
        powertext = statistics.Find("Power/Value").GetComponent<TextMeshProUGUI>();
        */
    }

    #endregion
}
