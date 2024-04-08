using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    [SerializeField] GameObject levelUpEffect;
    [SerializeField] PlayerStats stats;

    private float lerpTimer;
    private float delayTimer;

    [Header("UI")]
    Image frontXpBar;
    Image backXpBar;
    public TextMeshProUGUI levelText;

    TextMeshProUGUI experienceText;
    [SerializeField] GameObject floatingText;

    [Header("Multipliers")]
    [Range(1f, 300f)]
    public float additionMultiplier = 300;
    [Range(2f, 4f)]
    public float powerMultiplier = 2;
    [Range(7f, 14f)]
    public float divisionMultiplier = 7;

    private void Awake()
    {
        frontXpBar = GameObject.Find("Exp Bar Fill Front").GetComponent<Image>();
        backXpBar = GameObject.Find("Exp Bar Fill Back").GetComponent<Image>();
        experienceText = GameObject.Find("Text_Exp Bar").GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        frontXpBar.fillAmount = stats.currentExperience / stats.requiredExperience;
        backXpBar.fillAmount = stats.currentExperience / stats.requiredExperience;

        stats.requiredExperience = CalculateRequiredXp();

        levelText.text = stats.playerLevel.ToString();
    }

    void Update()
    {
        UpdateXpUI();

        if (Input.GetKeyDown(KeyCode.Equals))
        {
            GainExperienceFlatRate(5);
        }

        if (stats.currentExperience >= stats.requiredExperience)
        {
            LevelUp();
        }
    }

    public void UpdateXpUI()
    {
        float xpFraction = stats.currentExperience / stats.requiredExperience;
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

        experienceText.text = stats.currentExperience + "/" + stats.requiredExperience;
    }

    public void GainExperienceFlatRate(float xpGained)
    {
        ShowFloatingText(xpGained, Color.yellow);

        stats.currentExperience += xpGained;
        lerpTimer = 0f;
        delayTimer = 0f;
    }

    public void LevelUp()
    {
        stats.playerLevel++;

        // Inrease Player Stats

        frontXpBar.fillAmount = 0f;
        backXpBar.fillAmount = 0f;

        stats.currentExperience = Mathf.RoundToInt(stats.currentExperience - stats.requiredExperience);

        stats.requiredExperience = CalculateRequiredXp();

        levelText.text = stats.playerLevel.ToString();

        ShowLevelUpText(Color.white);
        Instantiate(levelUpEffect, transform.position, Quaternion.identity, transform);
    }

    private int CalculateRequiredXp()
    {
        int solveForRequiredXp = 0;
        for (int levelCycle = 1; levelCycle <= stats.playerLevel; levelCycle++)
        {
            solveForRequiredXp += (int)Mathf.Floor(levelCycle + additionMultiplier * Mathf.Pow(powerMultiplier, levelCycle / divisionMultiplier));
        }
        return solveForRequiredXp / 4;
    }

    void ShowFloatingText(float amount, Color color)
    {
        Vector3 offset = new Vector3(0f, 1, 0);

        if (floatingText)
        {
            GameObject textPrefab = Instantiate(floatingText, transform.position + offset, Quaternion.identity);
            TextMeshPro textMesh = textPrefab.GetComponentInChildren<TextMeshPro>();
            textMesh.text = "+" + amount.ToString() + " Exp";
            textMesh.fontSize = 4;
            textMesh.color = color; // Set the color of the text
            Destroy(textPrefab, 1.5f);
        }
    }

    void ShowLevelUpText(Color color)
    {
        Vector3 offset = new Vector3(0f, 2, 0);

        if (floatingText)
        {
            GameObject textPrefab = Instantiate(floatingText, transform.position + offset, Quaternion.identity);
            TextMeshPro textMesh = textPrefab.GetComponentInChildren<TextMeshPro>();
            textMesh.text = "Level Up";
            textMesh.color = color;
            Destroy(textPrefab, 2f);
        }
    }
}
