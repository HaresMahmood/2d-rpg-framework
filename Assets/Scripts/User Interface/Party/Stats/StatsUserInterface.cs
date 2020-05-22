using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsUserInterface : MonoBehaviour
{
    #region Variables

    private CanvasRenderer radarChartMesh;

    private Slider hpBar;

    private Transform hp;
    private Transform attack;
    private Transform defence;
    private Transform spAttack;
    private Transform spDefence;
    private Transform speed;

    private TextMeshProUGUI hpText;
    private TextMeshProUGUI attackText;
    private TextMeshProUGUI defenceText;
    private TextMeshProUGUI spAttackText;
    private TextMeshProUGUI spDefenceText;
    private TextMeshProUGUI speedText;

    #endregion

    #region Miscellaneous Methodss

    public void UpdateUserInterface(PartyMember member)
    {
        hpText.SetText($"{member.Stats.HP}/{member.Stats.Stats[Pokemon.Stat.HP]}");
        attackText.SetText(member.Stats.Stats[Pokemon.Stat.Attack].ToString());
        defenceText.SetText(member.Stats.Stats[Pokemon.Stat.Defence].ToString());
        spAttackText.SetText(member.Stats.Stats[Pokemon.Stat.SpAttack].ToString());
        spDefenceText.SetText(member.Stats.Stats[Pokemon.Stat.SpDefence].ToString());
        speedText.SetText(member.Stats.Stats[Pokemon.Stat.Speed].ToString());

        hpBar.value = member.Stats.HP / member.Stats.Stats[Pokemon.Stat.HP];
    }

    private void DrawStatChart()
    {
    }

    private TextMeshProUGUI GetUserInterfaceText(Transform parent)
    {
        return parent.Find("Amount").GetComponent<TextMeshProUGUI>();
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        hpBar = transform.Find("Health Bar").GetComponent<Slider>();

        Transform information = transform.Find("Information");

        hp = information.Find("HP");
        attack = information.Find("Attack");
        defence = information.Find("Defence");
        spAttack = information.Find("Sp. Attack");
        spDefence = information.Find("Sp. Defence");
        speed = information.Find("Speed");

        hpText = GetUserInterfaceText(hp);
        attackText = GetUserInterfaceText(attack);
        defenceText = GetUserInterfaceText(defence);
        spAttackText = GetUserInterfaceText(spAttack);
        spDefenceText = GetUserInterfaceText(spDefence);
        speedText = GetUserInterfaceText(speed);
    }

    #endregion
}
