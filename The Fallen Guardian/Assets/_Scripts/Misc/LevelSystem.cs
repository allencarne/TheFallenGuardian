using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] PlayerStats stats;
    [SerializeField] Player player;

    [Header("Effects")]
    [SerializeField] GameObject levelUpEffectPrefab;
    GameObject levelUpEffect;
    [SerializeField] GameObject floatingText;
    [SerializeField] GameObject levelUpText;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI levelText;
    TextMeshProUGUI experienceText;
    Image frontXpBar;
    Image backXpBar;

    [Header("Variables")]
    private float lerpTimer;
    private float delayTimer;

    [Header("Multipliers")]
    [Range(1f, 300f)]
    public float additionMultiplier = 300;
    [Range(2f, 4f)]
    public float powerMultiplier = 2;
    [Range(7f, 14f)]
    public float divisionMultiplier = 7;

    public UnityEvent OnPlayerLevelUp;

    private void Awake()
    {
        frontXpBar = GameObject.Find("Exp Bar Fill Front").GetComponent<Image>();
        backXpBar = GameObject.Find("Exp Bar Fill Back").GetComponent<Image>();
        experienceText = GameObject.Find("Text_Exp Bar").GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        frontXpBar.fillAmount = stats.CurrentExperience / stats.RequiredExperience;
        backXpBar.fillAmount = stats.CurrentExperience / stats.RequiredExperience;

        stats.RequiredExperience = CalculateRequiredXp();

        levelText.text = stats.PlayerLevel.ToString();
    }

    void Update()
    {
        UpdateXpUI();

        if (stats.CurrentExperience >= stats.RequiredExperience)
        {
            LevelUp();
        }
    }

    public void UpdateXpUI()
    {
        float xpFraction = stats.CurrentExperience / stats.RequiredExperience;
        float FXP = frontXpBar.fillAmount;
        if (FXP < xpFraction)
        {
            delayTimer += Time.deltaTime;
            backXpBar.fillAmount = xpFraction;
            if (delayTimer > 0.5)
            {
                lerpTimer += Time.deltaTime;
                float percentComplete = lerpTimer / 4;
                frontXpBar.fillAmount = Mathf.Lerp(FXP, backXpBar.fillAmount, percentComplete);
            }
        }

        experienceText.text = stats.CurrentExperience + "/" + stats.RequiredExperience;
    }

    public void GainExperienceFlatRate(float xpGained)
    {
        ShowFloatingText(xpGained, floatingText);

        stats.CurrentExperience += xpGained;
        lerpTimer = 0f;
        delayTimer = 0f;
    }

    public void LevelUp()
    {
        // Increase Player Level
        stats.PlayerLevel++;
        levelText.text = stats.PlayerLevel.ToString();

        // Increase Player Health
        stats.MaxHealth++;
        float missingHealth = stats.MaxHealth - stats.Health;
        player.Heal(missingHealth);

        // Increase Player Damage
        stats.BaseDamage++;
        stats.CurrentDamage++;

        // Update Bar
        frontXpBar.fillAmount = 0f;
        backXpBar.fillAmount = 0f;
        stats.CurrentExperience = Mathf.RoundToInt(stats.CurrentExperience - stats.RequiredExperience);
        stats.RequiredExperience = CalculateRequiredXp();

        // Effects
        ShowLevelUpText(Color.white);
        levelUpEffect = Instantiate(levelUpEffectPrefab, transform.position, Quaternion.identity, transform);
        Destroy(levelUpEffect, 2);

        OnPlayerLevelUp?.Invoke();
    }

    private int CalculateRequiredXp()
    {
        int solveForRequiredXp = 0;
        for (int levelCycle = 1; levelCycle <= stats.PlayerLevel; levelCycle++)
        {
            solveForRequiredXp += (int)Mathf.Floor(levelCycle + additionMultiplier * Mathf.Pow(powerMultiplier, levelCycle / divisionMultiplier));
        }
        return solveForRequiredXp / 4;
    }

    void ShowFloatingText(float amount, GameObject text)
    {
        Vector3 offset = new Vector3(0f, 1, 0);

        if (text)
        {
            GameObject textPrefab = Instantiate(text, transform.position + offset, Quaternion.identity);
            TextMeshPro textMesh = textPrefab.GetComponentInChildren<TextMeshPro>();
            textMesh.text = "+" + amount.ToString() + " Exp";
            textMesh.fontSize = 4;
            Destroy(textPrefab, 1.5f);
        }
    }

    void ShowLevelUpText(Color color)
    {
        Vector3 offset = new Vector3(0f, 2, 0);

        if (levelUpText)
        {
            GameObject textPrefab = Instantiate(levelUpText, transform.position + offset, Quaternion.identity);
            TextMeshPro textMesh = textPrefab.GetComponentInChildren<TextMeshPro>();
            textMesh.text = "Level Up!";
            textMesh.color = color;
            Destroy(textPrefab, 2f);
        }
    }
}
