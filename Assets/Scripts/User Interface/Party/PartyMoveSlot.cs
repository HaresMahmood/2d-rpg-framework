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
        PartyMember.MemberMove move = (PartyMember.MemberMove)slotObject;

        moveNameText.SetText(move.Value.name);
        
        //info.Find("Typing/Typing").GetComponent<TextMeshProUGUI>().SetText(learnedMove.move.type.ToString());
        //info.Find("Typing/Typing").GetComponent<TextMeshProUGUI>().color = learnedMove.move.UIColor;
   
        ppText.SetText(move.PP.ToString() + "/" + move.Value.pp);

        physicalIcon.SetActive(move.Value.category == Move.Category.Physical);
        specialIcon.SetActive(move.Value.category == Move.Category.Special);
        accuracyText.SetText(move.Value.accuracy.ToString());
        powertext.SetText(move.Value.power.ToString());

        descriptionText.SetText(move.Value.description);

        panel.color = move.Value.UIColor;
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

        Transform slot = transform.Find("Information Panel").transform;

        information = slot.Find("Information");
        statistics = slot.Find("Stats");
        descriptionText = slot.Find("Description/Value").GetComponent<TextMeshProUGUI>();

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
