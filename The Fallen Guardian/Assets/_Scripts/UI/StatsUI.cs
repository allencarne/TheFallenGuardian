using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] PlayerStats stats;

    [SerializeField] TextMeshProUGUI text_PlayerName;
    [SerializeField] TextMeshProUGUI text_PlayerClass;
    [SerializeField] TextMeshProUGUI text_PlayerLevel;
    [SerializeField] Image image_ClassIcon;

    [SerializeField] TextMeshProUGUI text_Health;
    [SerializeField] TextMeshProUGUI text_Damage;
    [SerializeField] TextMeshProUGUI text_Speed;
    [SerializeField] TextMeshProUGUI text_AttackSpeed;
    [SerializeField] TextMeshProUGUI text_CoolDown;

    public void UpdateStatsUI()
    {
        text_PlayerName.text = stats.PlayerName.ToString();
        text_PlayerClass.text = stats.PlayerClass.ToString();
        text_PlayerLevel.text = stats.PlayerLevel.ToString();
        image_ClassIcon.sprite = stats.PlayerClassIcon;

        text_Health.text = stats.Health.ToString();
        text_Damage.text = stats.CurrentDamage.ToString();
        text_Speed.text = stats.CurrentSpeed.ToString();
        text_AttackSpeed.text = stats.CurrentAttackSpeed.ToString();
        text_CoolDown.text = stats.CurrentCDR.ToString();
    }
}
