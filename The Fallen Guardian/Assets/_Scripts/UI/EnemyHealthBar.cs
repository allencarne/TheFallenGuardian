using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    Enemy enemy;

    [HideInInspector] public float lerpTimer;
    [SerializeField] Image healthBarFront;
    [SerializeField] Image healthBarBack;
    float chipSpeed = 2f;

    public void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        enemy.health = Mathf.Clamp(enemy.health, 0, enemy.maxHealth);

        float fillFront = healthBarFront.fillAmount;
        float fillBack = healthBarBack.fillAmount;
        float healthFraction = enemy.health / enemy.maxHealth;

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
