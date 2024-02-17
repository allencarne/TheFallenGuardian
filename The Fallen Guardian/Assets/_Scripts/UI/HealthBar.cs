using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Player player;
    CharacterStats characterStats;

    [SerializeField] Image healthBarFront;
    [SerializeField] Image healthBarBack;
    [SerializeField] Canvas playerUI;
    [HideInInspector] public float chipSpeed = 2f;
    [HideInInspector] public float lerpTimer;

    private void Start()
    {
        characterStats = player.characterStats;
    }

    public void Update()
    {
        if (characterStats != null)
        {
            characterStats.health = Mathf.Clamp(characterStats.health, 0, characterStats.maxHealth);
            UpdateHealthUI();
        }
    }

    void UpdateHealthUI()
    {
        float fillFront = healthBarFront.fillAmount;
        float fillBack = healthBarBack.fillAmount;
        float healthFraction = characterStats.health / characterStats.maxHealth;

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
