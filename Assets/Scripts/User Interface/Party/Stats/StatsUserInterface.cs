using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class StatsUserInterface : MonoBehaviour
{
    #region Variables

    private RadarChartUserInterface radarChart;

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
        float hp = (float)member.Stats.HP / (float)member.Stats.Stats[Pokemon.Stat.HP];
        string color = hp >= 0.5f ? "#67FF8F" : (hp >= 0.25f ? "#FFB766" : "#FF7766");

        hpText.SetText($"<color={color}>{member.Stats.HP}</color>/{member.Stats.Stats[Pokemon.Stat.HP]}");
        attackText.SetText(member.Stats.Stats[Pokemon.Stat.Attack].ToString());
        defenceText.SetText(member.Stats.Stats[Pokemon.Stat.Defence].ToString());
        spAttackText.SetText(member.Stats.Stats[Pokemon.Stat.SpAttack].ToString());
        spDefenceText.SetText(member.Stats.Stats[Pokemon.Stat.SpDefence].ToString());
        speedText.SetText(member.Stats.Stats[Pokemon.Stat.Speed].ToString());

        hpBar.fillRect.GetComponent<Image>().color = color.ToColor(); 
        StartCoroutine(LerpSlider(hpBar, hp, 0.15f));

        radarChart.UpdateUserInterface(member.Stats.Stats.Values.ToList());
    }

    private TextMeshProUGUI GetUserInterfaceText(Transform parent)
    {
        return parent.Find("Amount").GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name=""></param>
    /// <param name=""></param>
    /// <returns></returns>
    private IEnumerator LerpSlider(Slider slider, float targetValue, float duration)
    {
        float initialValue = slider.value;

        float t = 0; // Tracks how many seconds we've been fading.
        while (t < duration) // While time is less than the duration of the fade, ...
        {
            if (Time.timeScale == 0)
                t += Time.unscaledDeltaTime;
            else
                t += Time.deltaTime;
            float blend = Mathf.Clamp01(t / duration); // Turns the time into an interpolation factor between 0 and 1. 

            slider.value = Mathf.Lerp(initialValue, targetValue, blend);

            yield return null; // Wait one frame, then repeat.
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        radarChart = transform.Find("Chart").GetComponent<RadarChartUserInterface>();

        Transform information = radarChart.transform.Find("Base/Information");

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

        hpBar = hp.Find("Health Bar").GetComponent<Slider>();
    }

    #endregion
}
