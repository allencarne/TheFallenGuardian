using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] PlayerStats stats;

    [Header("Healthbar")]
    [SerializeField] GameObject healthBar;
    [SerializeField] Image healthBarBorder;
    [SerializeField] Image healthBarFront;
    [SerializeField] Image healthBarBack;

    [Header("Variables")]
    [SerializeField] Color borderColor;
    Coroutine lerpingCoroutine;
    float chipSpeed = .5f;
    bool isLerping = false;

    [Header("Components")]
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] SpriteRenderer bodySprite;
    public GameObject floatingText;
    public Image CastBar;

    private void Awake()
    {
        playerName.text = stats.PlayerName;

        // *Temporary* Player Stats
        stats.PlayerLevel = 1;
        stats.CurrentExperience = 0;
        stats.RequiredExperience = 3;

        stats.MaxHealth = 10;
        stats.BaseDamage = 1;
        stats.BaseSpeed = 8;
    }

    private void Start()
    {
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        if (isLerping)
        {
            StopCoroutine(lerpingCoroutine);
            isLerping = false;
        }

        lerpingCoroutine = StartCoroutine(LerpHealthBar());
    }

    IEnumerator LerpHealthBar()
    {
        isLerping = true;

        float fillFront = healthBarFront.fillAmount;
        float fillBack = healthBarBack.fillAmount;
        float healthFraction = stats.Health / stats.MaxHealth;

        if (fillBack > healthFraction)
        {
            healthBarFront.fillAmount = healthFraction;
            healthBarBack.color = Color.white;

            float elapsedTime = 0;

            while (elapsedTime < chipSpeed)
            {
                elapsedTime += Time.deltaTime;
                float percentComplete = elapsedTime / chipSpeed;
                percentComplete = percentComplete * percentComplete;
                healthBarBack.fillAmount = Mathf.Lerp(fillBack, healthFraction, percentComplete);
                yield return null;
            }
        }

        if (fillFront < healthFraction)
        {
            healthBarBack.color = Color.green;
            healthBarBack.fillAmount = healthFraction;

            float elapsedTime = 0;

            while (elapsedTime < chipSpeed)
            {
                elapsedTime += Time.deltaTime;
                float percentComplete = elapsedTime / chipSpeed;
                percentComplete = percentComplete * percentComplete;
                healthBarFront.fillAmount = Mathf.Lerp(fillFront, healthBarBack.fillAmount, percentComplete);
                yield return null;
            }
        }

        isLerping = false;
    }

    public IEnumerator FlashEffect(Color color)
    {
        float flashDuration = 0.1f;

        bodySprite.color = color;
        yield return new WaitForSeconds(flashDuration / 2);

        bodySprite.color = Color.white;
        yield return new WaitForSeconds(flashDuration / 2);

        bodySprite.color = color;
        yield return new WaitForSeconds(flashDuration / 2);

        bodySprite.color = Color.white;
        yield return new WaitForSeconds(flashDuration / 2);

        // Reset to original color
        bodySprite.color = Color.white;
    }

    public void ShowFloatingText(float amount, Color color)
    {
        Vector3 offset = new Vector3(0f, -1f, 0);

        if (floatingText)
        {
            GameObject textPrefab = Instantiate(floatingText, transform.position + offset, Quaternion.identity);
            TextMeshPro textMesh = textPrefab.GetComponentInChildren<TextMeshPro>();
            textMesh.text = amount.ToString();
            textMesh.color = color; // Set the color of the text
            Destroy(textPrefab, 1);
        }
    }

    public void UpdateCastBar(float fillAmount)
    {
        if (CastBar != null)
        {
            CastBar.fillAmount = fillAmount;
        }
    }

    public void CombatBorder()
    {
        healthBarBorder.color = borderColor;
    }

    public void LeaveCombatBorder()
    {
        healthBarBorder.color = Color.white;
    }
}
