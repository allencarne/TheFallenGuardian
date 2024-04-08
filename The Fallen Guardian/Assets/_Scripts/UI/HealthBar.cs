using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PlayerStats stats;

    [HideInInspector] public float lerpTimer;
    [SerializeField] Image healthBarFront;
    [SerializeField] Image healthBarBack;
    float chipSpeed = 2f;

    private void Update()
    {
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        stats.Health = Mathf.Clamp(stats.Health, 0, stats.MaxHealth);

        float fillFront = healthBarFront.fillAmount;
        float fillBack = healthBarBack.fillAmount;
        float healthFraction = stats.Health / stats.MaxHealth;

        if (fillBack > healthFraction)
        {
            healthBarFront.fillAmount = healthFraction;
            healthBarBack.color = Color.white;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            healthBarBack.fillAmount = Mathf.Lerp(fillBack, healthFraction, percentComplete);
        }

        if (fillFront < healthFraction)
        {
            healthBarBack.color = Color.green;
            healthBarBack.fillAmount = healthFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            healthBarFront.fillAmount = Mathf.Lerp(fillFront, healthBarBack.fillAmount, percentComplete);
        }
    }
}
